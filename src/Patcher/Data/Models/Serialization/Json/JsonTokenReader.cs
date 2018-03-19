// Copyright(C) 2015,2016,2017,2018 Unforbidable Works
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or(at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models.Serialization.Json
{
    public class JsonTokenReader
    {
        readonly StreamReader reader;

        public JsonTokenReader(Stream stream)
        {
            reader = new StreamReader(stream);
        }

        /// <summary>
        /// Read JSON tokens from the stream.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<JsonToken> ReadTokens()
        {
            JsonToken token;
            while ((token = ReadNextToken()) != null)
            {
                yield return token;
            }
        }

        static char[] singleCharTokens = { '{', '}', '[', ']', ':', ',' };

        private JsonToken ReadNextToken()
        {
            // Skip white spaces
            while (!reader.EndOfStream && char.IsWhiteSpace((char)reader.Peek()))
                reader.Read();

            // Return null if end of stream
            if (reader.EndOfStream)
                return null;

            // Single char tokens
            char c = (char)reader.Read();
            if (singleCharTokens.Contains(c))
                return new JsonToken(c);

            // Expecting multi char token, so end of stream is an error
            if (reader.EndOfStream)
                throw new InvalidDataException("Unexpected end of stream.");

            var builder = new StringBuilder();
            if (c == '"')
            {
                // Is token in quotes? If so also read next char
                c = (char)reader.Read();

                while (true)
                {
                    // End of quoted string
                    if (c == '"')
                        return new JsonToken(builder.ToString(), true);

                    if (c == '\\')
                    {
                        // Next character is escaped
                        c = (char)reader.Read();
                        builder.Append(GetEscapedChar(c));

                        // In a quoted string stream must not end yet
                        if (reader.EndOfStream)
                            throw new InvalidDataException("Unexpected end of stream.");
                    }
                    else
                    {
                        // Character inside quotes
                        builder.Append(c);
                    }

                    c = (char)reader.Read();

                    if (reader.EndOfStream)
                    {
                        throw new InvalidDataException("Unexpected end of stream.");
                    }
                }
            }
            else
            {
                // Read token that isn't in quotes
                // Read until end of stream, a single char token or a while space
                while (!reader.EndOfStream)
                {
                    // Don't consume char from stream
                    c = (char)reader.Peek();
                    if (singleCharTokens.Contains(c) && !char.IsWhiteSpace(c))
                        break;

                    builder.Append(c);

                    // Advance stream pointer only when char is appended
                    reader.Read();
                }
                return new JsonToken(builder.ToString(), false);
            }
        }

        private char GetEscapedChar(char c)
        {
            switch (c)
            {
                case '"': return '"';
                case '\\': return '\\';
                default: throw new InvalidDataException(string.Format("Unexpected escaped character '{0}'", c));
            }
        }

        internal IEnumerable<JsonToken> ReadTokens(object readGameModel)
        {
            throw new NotImplementedException();
        }
    }
}
