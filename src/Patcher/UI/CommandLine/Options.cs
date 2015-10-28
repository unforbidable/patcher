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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Patcher.UI.CommandLine
{
    public abstract class Options
    {
        [Option("help", 'h')]
        [DefaultValue(false)]
        [Description("Print this help screen (again). ")]
        public bool ShowHelp { get; set; }

        private List<Option> options = new List<Option>();

        private string usage;

        public Options()
        {
            // Get usage
            object[] a = GetType().GetCustomAttributes(typeof(UsageAttribute), true);
            if (a.Length > 0)
            {
                usage = ((UsageAttribute)a[0]).Text;
            }

            // Create options meta data
            foreach (var prop in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var option = new Option()
                {
                    PropertyInfo = prop
                };

                foreach (var attr in prop.GetCustomAttributes(true))
                {
                    if (attr.GetType() == typeof(OptionAttribute))
                    {
                        option.LongName = ((OptionAttribute)attr).LongName;
                        option.ShortName = ((OptionAttribute)attr).ShortName;
                    }
                    else if (attr.GetType() == typeof(DefaultValueAttribute))
                    {
                        option.DefaultValue = ((DefaultValueAttribute)attr).Value;
                    }
                    else if (attr.GetType() == typeof(DescriptionAttribute))
                    {
                        option.Description = ((DescriptionAttribute)attr).Text;
                    }
                }

                if (!string.IsNullOrEmpty(option.LongName))
                    options.Add(option);
            }

            // Set default values
            foreach (var opt in options)
            {
                if (opt.DefaultValue != null)
                {
                    opt.PropertyInfo.SetValue(this, opt.DefaultValue, null);
                }
            }
        }

        public bool TryLoad(string[] args)
        {
            try
            {
                Load(args);
                if (ShowHelp)
                {
                    PrintOptions();
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Bad arguments: " + e.Message);
                Console.WriteLine();
                PrintOptions();
                return false;
            }

            return true;
        }

        private void PrintOptions()
        {
            StringWriter writer = new StringWriter();
            if (usage != null)
            {
                StringReader reader = new StringReader(usage);
                string line;
                int i = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (i == 0)
                        writer.Write("Usage: ");
                    else
                        writer.Write("         ");

                    writer.WriteLine(line);
                    i++;
                }
                writer.WriteLine();
            }

            int pad = options.Select(o => o.LongName.Length).Max() + 2;
            foreach (var opt in options)
            {
                string buffer;
                if (opt.ShortName != '\0')
                    buffer = string.Format(" -{0}, ", opt.ShortName);
                else
                    buffer = "     ";

                buffer = string.Format("{0}{1,-" + pad + "}  ", buffer, "--" + opt.LongName);

                int col = buffer.Length;
                string indent = new string(Enumerable.Range(0, col).Select(i => ' ').ToArray());

                writer.Write(buffer);

                StringReader reader = new StringReader(opt.Description);
                string line;
                bool firstLine = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!firstLine)
                        writer.Write(indent);

                    writer.WriteLine(line);
                    firstLine = false;
                }
            }

            Program.Display.ShowPreRunMessage(writer.ToString(), false);
        }

        private void Load(string[] args)
        {
            int i = 0;
            while (i < args.Length)
            {
                string token = args[i++];
                string prefix = new string(token.TakeWhile(c => c == '-').ToArray());
                if (prefix.Length < 1 || prefix.Length > 2)
                    throw new ArgumentException("Invalid token '" + token + "'");

                bool isLongName = prefix.Length == 2;
                bool isShortName = prefix.Length == 1;

                string tokenWithoutPrefix = token.Substring(prefix.Length);
                string name = new string(tokenWithoutPrefix.TakeWhile(c => char.IsLetterOrDigit(c) || c == '-').ToArray());

                if (isShortName && name.Length != 1 || isLongName && name.Length < 1)
                    throw new ArgumentException("Invalid option name '" + prefix + name + "'");

                Option option = options.Where(o => isLongName && string.Equals(o.LongName, name) || isShortName && o.ShortName == name[0]).FirstOrDefault();
                if (option == null)
                    throw new ArgumentException("Unknown option '" + prefix + name + "'");

                string tokenAfterName = tokenWithoutPrefix.Substring(name.Length);
                if (option.Type == typeof(bool) && tokenAfterName.Length == 0)
                {
                    option.PropertyInfo.SetValue(this, true, null);
                }
                else
                {
                    if (tokenAfterName.Length == 0 || tokenAfterName[0] != '=')
                        throw new ArgumentException("Expected '=' after option '" + prefix + name + "'");

                    string value = tokenAfterName.Substring(1);
                    if (value.Length == 0)
                        throw new ArgumentException("Expected value after option '" + prefix + name + "'");

                    if (option.Type == typeof(string))
                    {
                        option.PropertyInfo.SetValue(this, value, null);
                    }
                    else if (option.Type == typeof(int))
                    {
                        option.PropertyInfo.SetValue(this, Convert.ToInt32(value), null);
                    }
                    else if (option.Type == typeof(float))
                    {
                        option.PropertyInfo.SetValue(this, Convert.ToSingle(value), null);
                    }
                    else if (option.Type == typeof(bool))
                    {
                        option.PropertyInfo.SetValue(this, Convert.ToBoolean(value), null);
                    }
                    else
                    {
                        throw new ArgumentException("Unsupported option type '" + option.Type.FullName + "'");
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Join(" ", options.Select(o => string.Format("--{0}={1}", o.LongName, o.PropertyInfo.GetValue(this, null))));
        }

        class Option
        {
            public PropertyInfo PropertyInfo { get; set; }
            public Type Type { get { return PropertyInfo.PropertyType; } }

            public string LongName { get; set; }
            public char ShortName { get; set; }
            public string Description { get; set; }
            public object DefaultValue { get; set; }
        }
    }
}
