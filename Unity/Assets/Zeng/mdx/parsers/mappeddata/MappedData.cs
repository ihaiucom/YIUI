

using System.Collections.Generic;
using Zeng.mdx.parsers.ini;
using Zeng.mdx.parsers.slk;

namespace Zeng.mdx.parsers
{
    public class MappedData
    {
        private Dictionary<string, MappedDataRow> map = new Dictionary<string, MappedDataRow>();
        public Dictionary<string, MappedDataRow> Map { get { return map; } }

        public MappedData(string buffer = null)
        {
            if (buffer != null)
            {
                Load(buffer);
            }
        }

        /**
         * Load data from an SLK file or an INI file.
         * 
         * Note that this may override previous properties!
         */
        public void Load(string buffer)
        {
            if (buffer.StartsWith("ID;"))
            {
                SlkFile file = new SlkFile();
                file.Load(buffer);

                string[][] rows = file.rows;
                string[] header = rows[0];

                for (int i = 1; i < rows.Length; i++)
                {
                    string[] row = rows[i];

                    // DialogueDemonBase.slk has an empty row.
                    if (row != null)
                    {
                        string name = row[0];

                        // DialogueDemonBase.slk also has rows containing only a single underline.
                        if (name != null && name != "_")
                        {
                            if (!map.ContainsKey(name))
                            {
                                map[name] = new MappedDataRow();
                            }

                            MappedDataRow mapped = map[name];

                            for (int j = 0; j < header.Length; j++)
                            {
                                string key = header[j];

                                // UnitBalance.slk doesn't define the name of one column.
                                if (key == null)
                                {
                                    key = "column" + j;
                                }

                                mapped.Set(key.ToLower(), j < row.Length ? row[j] : "");
                            }
                        }
                    }
                }
            }
            else
            {
                IniFile file = new IniFile();
                file.Load(buffer);

                Dictionary<string, Dictionary<string, string>> sections = file.sections;

                foreach (KeyValuePair<string, Dictionary<string, string>> entry in sections)
                {
                    string row = entry.Key;

                    if (!map.ContainsKey(row))
                    {
                        map[row] = new MappedDataRow();
                    }

                    MappedDataRow mapped = map[row];

                    foreach (KeyValuePair<string, string> property in entry.Value)
                    {
                        mapped.Set(property.Key.ToLower(), property.Value);
                    }
                }
            }
        }

        public MappedDataRow GetRow(string key)
        {
            if (map.ContainsKey(key))
            {
                return map[key];
            }
            return null;
        }

        public string GetProperty(string key, string name)
        {
            if (map.ContainsKey(key))
            {
                MappedDataRow row = map[key];
                return row.GetString(name);
            }
            return null;
        }

        public void SetRow(string key, MappedDataRow values)
        {
            map[key] = values;
        }

        public MappedDataRow FindRow(string key, string expectedValue)
        {
            foreach (MappedDataRow row in map.Values)
            {
                if (row.GetString(key) == expectedValue)
                {
                    return row;
                }
            }
            return null;
        }
    }
}
