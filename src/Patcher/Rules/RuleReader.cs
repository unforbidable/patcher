/// Copyright(C) 2015 Unforbidable Works
///
/// This program is free software; you can redistribute it and/or
/// modify it under the terms of the GNU General Public License
/// as published by the Free Software Foundation; either version 2
/// of the License, or(at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program; if not, write to the Free Software
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Patcher.Rules
{
    sealed class RuleReader : IDisposable
    {
        readonly XDocument document;

        public RuleReader(Stream stream)
        {
            // Take rule filename from stream if it is a file stream
            FileStream fileStream = stream as FileStream;

            document = XDocument.Load(stream);
            
            if (document.Root.Name != "rules")
            {
                throw new InvalidDataException("Unexpected root element name");
            }
        }

        static Regex matchIllegalRuleNameCharactersRegex = new Regex("[^a-zA-Z0-9_]", RegexOptions.Compiled);

        public IEnumerable<RuleEntry> ReadRules()
        {
            int ruleNumber = 0;
            foreach (var element in document.Root.Elements("rule"))
            {
                RuleEntry entry = new RuleEntry();

                var nameElements = element.Elements("name").ToArray();
                entry.Name = nameElements.Length > 0 ? matchIllegalRuleNameCharactersRegex.Replace(nameElements[0].Value, "_") : ruleNumber.ToString();
                ruleNumber++;

                //Log.Fine("Reading rule " + entry.Name);

                if (element.Elements("description").Any())
                    entry.Description = element.Elements("description").First().Value;

                if (element.Elements("from").Any())
                    entry.From = new RuleEntry.RuleEntryFrom() { FormKind = element.Elements("from").First().Value.ToUpper() };

                var whereElements = element.Elements("where").Take(2).ToArray();
                if (whereElements.Length > 0)
                {
                    entry.Where = new RuleEntry.RuleEntryWhere() { Code = whereElements[0].Value };

                    if (whereElements.Length > 1)
                    {
                        Log.Warning("Extra where elements will be ignored");
                    }
                }

                var selectElements = element.Elements("select").Take(2).ToArray();
                if (selectElements.Length > 0)
                {
                    entry.Select = new RuleEntry.RuleEntrySelect() { Code = selectElements[0].Value };

                    if (selectElements.Length > 1)
                    {
                        Log.Warning("Extra select elements will be ignored");
                    }
                }

                var updateElements = element.Elements("update").Take(2).ToArray();
                if (updateElements.Length > 0)
                {
                    entry.Update = new RuleEntry.RuleEntryUpdate() { Code = updateElements[0].Value };

                    if (updateElements.Length > 1)
                    {
                        Log.Warning("Extra update elements will be ignored");
                    }
                }

                var insertElements = element.Elements("insert");
                if (insertElements.Any())
                {
                    List<RuleEntry.RuleEntryInsert> inserts = new List<RuleEntry.RuleEntryInsert>();

                    foreach (var insertElement in insertElements)
                    {
                        var intoAttribute = insertElement.Attribute("into");
                        if (intoAttribute == null)
                            throw new InvalidDataException("Insert is missing mandatory attribute: into");

                        string insertFormKind = intoAttribute.Value;

                        var copy = false;
                        var copyAttribute = insertElement.Attribute("copy");
                        if (copyAttribute != null)
                        {
                            if (!bool.TryParse(copyAttribute.Value.ToLower(), out copy))
                            {
                                Log.Warning("Attribute 'copy' present on insert but the value '{0}' could not be parsed. Expected 'true' or 'false'.", copyAttribute.Value);
                            }

                            // Warn when copy attribute will be ignored - non-query and incompatible inserts
                            if (entry.From == null || !entry.From.FormKind.Equals(insertFormKind))
                            {
                                Log.Warning("Attribute 'copy' present on a non-query incompatible insert will be ignored.");
                            }
                        }

                        uint insertedFormId = 0;
                        var asAttribute = insertElement.Attribute("as");
                        if (asAttribute != null)
                        {
                            if (!uint.TryParse(asAttribute.Value.ToLower(), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out insertedFormId))
                            {
                                Log.Warning("Attribute 'as' present on insert but the value '{0}' could not be parsed.", asAttribute.Value);
                            }
                        }

                        inserts.Add(new RuleEntry.RuleEntryInsert() { InsertedFormId = insertedFormId, InsertedFormKind = insertFormKind, Copy = copy, Code = insertElement.Value });
                    }

                    entry.Inserts = inserts;
                }

                yield return entry;
            }
        }

        public void Dispose()
        {
        }
    }
}
