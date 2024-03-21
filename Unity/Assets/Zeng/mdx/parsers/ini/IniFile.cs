
using System.Collections.Generic;
using System.Text.RegularExpressions;

using IniSection = System.Collections.Generic.Dictionary<string, string>;

namespace Zeng.mdx.parsers.ini
{
    public class IniFile
    {
        private Dictionary<string, string> properties = new Dictionary<string, string>();
        public Dictionary<string, IniSection> sections = new Dictionary<string, IniSection>();

        public void Load(string buffer)
        {
            // All properties added until a section is reached are added to the properties map.
            // Once a section is reached, any further properties will be added to it until matching another section, etc.
            IniSection section = properties;
            Dictionary<string, IniSection> sections = this.sections;

            foreach (string line in buffer.Split("\r\n"))
            {
                // INI defines comments as starting with a semicolon ';'.
                // However, Warcraft 3 INI files use normal C comments '//'.
                // In addition, Warcraft 3 files have empty lines.
                // Therefore, ignore any line matching any of these conditions.
                if (line.Length > 0 && !line.StartsWith("//") && !line.StartsWith(";"))
                {
                    Match match = Regex.Match(line, @"^\[(.+?)\]");

                    if (match.Success)
                    {
                        string name = match.Groups[1].Value.Trim();

                        if (!sections.ContainsKey(name))
                        {
                            sections[name] = new IniSection();
                        }

                        section = sections[name];
                    }
                    else
                    {
                        match = Regex.Match(line, @"^(.+?)=(.*?)$");

                        if (match.Success)
                        {
                            string key = match.Groups[1].Value;
                            string value = match.Groups[2].Value;

                            if (!string.IsNullOrEmpty(value) && value[0] == '"')
                            {
                                value = value.Substring(1, value.Length - 2);
                            }

                            section[key] = value;
                        }
                    }
                }
            }
        }

        public string Save()
        {
            List<string> lines = new List<string>();

            foreach (KeyValuePair<string, string> property in properties)
            {
                lines.Add($"{property.Key}={property.Value}");
            }

            foreach (KeyValuePair<string, IniSection> section in sections)
            {
                lines.Add($"[{section.Key}]");

                foreach (KeyValuePair<string, string> entry in section.Value)
                {
                    lines.Add($"{entry.Key}={entry.Value}");
                }
            }

            return string.Join("\r\n", lines);
        }

        public IniSection GetSection(string name)
        {
            if (sections.ContainsKey(name))
            {
                return sections[name];
            }

            return null;
        }
    }
}
