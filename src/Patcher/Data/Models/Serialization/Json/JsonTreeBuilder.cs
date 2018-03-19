using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models.Serialization.Json
{
    public static class JsonTreeBuilder
    {
        public static JsonArray BuildJsonTree(IEnumerable<JsonToken> tokens)
        {
            var enumerator = tokens.GetEnumerator();
            enumerator.MoveNext();
            return BuildArray(enumerator);
        }

        private static JsonArray BuildArray(IEnumerator<JsonToken> tokens)
        {
            // Ensure [
            if (!tokens.Current.IsBeginArray)
            {
                throw new InvalidDataException("Expected the beginning of array.");
            }

            // Consume [
            tokens.MoveNext();

            var a = new JsonArray();
            while (!tokens.Current.IsEndArray)
            {
                a.Add(BuildObject(tokens));

                if (tokens.Current.IsSeparator && !tokens.MoveNext())
                    throw new InvalidDataException("Unexpected end of data.");
            }

            // Consume ]
            tokens.MoveNext();

            return a;
        }

        private static JsonObject BuildObject(IEnumerator<JsonToken> tokens)
        {
            // Ensure {
            if (!tokens.Current.IsBeginObject)
            {
                throw new InvalidDataException("Expected the beginning of object.");
            }

            // Consume {
            tokens.MoveNext();

            var o = new JsonObject();
            while (!tokens.Current.IsEndObject)
            {
                string propertyName = tokens.Current.Text;

                if (!tokens.MoveNext())
                    throw new InvalidDataException("Unexpected end of data.");

                if (!tokens.Current.IsAssignment)
                {
                    throw new InvalidDataException("Expected object property assignement.");
                }

                if (!tokens.MoveNext())
                    throw new InvalidDataException("Unexpected end of data.");

                if (tokens.Current.IsText)
                {
                    // Property value is a string
                    o.Properties.Add(propertyName, new JsonString(tokens.Current.Text, tokens.Current.IsQuoted));

                    if (!tokens.MoveNext())
                        break;
                }
                else if (tokens.Current.IsBeginArray)
                {
                    o.Properties.Add(propertyName, BuildArray(tokens));
                }
                else if (tokens.Current.IsBeginObject)
                {
                    o.Properties.Add(propertyName, BuildObject(tokens));
                }
                else
                {
                    throw new InvalidDataException("Expected text or the beginning of object or array");
                }

                if (tokens.Current.IsSeparator && !tokens.MoveNext())
                    throw new InvalidDataException("Unexpected end of data.");
            }

            // Consume }
            tokens.MoveNext();

            return o;
        }
    }
}
