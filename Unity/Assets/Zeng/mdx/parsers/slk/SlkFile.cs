using System;
using System.Collections.Generic;

namespace Zeng.mdx.parsers.slk
{
    public class SlkFile
    {
        public string[][] rows = new string[][] { };

        public void Load(string buffer)
        {
            if (!buffer.StartsWith("ID"))
            {
                throw new Exception("WrongMagicNumber");
            }

            string[][] rows = this.rows;
            int x = 0;
            int y = 0;

            foreach (string line in buffer.Split('\n'))
            {
                // The B command is supposed to define the total number of columns and rows, however in UbetSplatData.slk it gives wrong information
                // Therefore, just ignore it, since C# arrays grow as they want either way
                if (!string.IsNullOrEmpty(line) && line[0] != 'B')
                {
                    foreach (string token in line.Split(';'))
                    {
                        char op = token[0];
                        string valueString = token.Substring(1).Trim();
                        string value;

                        if (op == 'X')
                        {
                            x = int.Parse(valueString) - 1;
                        }
                        else if (op == 'Y')
                        {
                            y = int.Parse(valueString) - 1;
                        }
                        else if (op == 'K')
                        {
                            if (rows.Length <= y)
                            {
                                Array.Resize(ref rows, y + 1);
                            }

                            if (valueString[0] == '"')
                            {
                                value = valueString.Substring(1, valueString.Length - 2);
                            }
                            else
                            {
                                value = valueString;
                            }

                            if (rows[y] == null)
                            {
                                rows[y] = new string[x + 1];
                            }
                            else if (rows[y].Length <= x)
                            {
                                Array.Resize(ref rows[y], x + 1);
                            }

                            rows[y][x] = value;
                        }
                    }
                }
            }
            this.rows = rows;
        }

        public string Save()
        {
            string[][] rows = this.rows;
            int rowCount = rows.Length;
            List<string> lines = new List<string>();
            int biggestColumn = 0;

            for (int y = 0; y < rowCount; y++)
            {
                string[] row = rows[y];
                int columnCount = row.Length;

                if (columnCount > biggestColumn)
                {
                    biggestColumn = columnCount;
                }

                bool firstOfRow = true;

                for (int x = 0; x < columnCount; x++)
                {
                    string value = row[x];

                    if (value != null)
                    {
                        string encoded;

                        if (value is string)
                        {
                            encoded = $"\"{value}\"";
                        }
                        //else if (value is bool)
                        //{
                        //    if ((bool)value)
                        //    {
                        //        encoded = "TRUE";
                        //    }
                        //    else
                        //    {
                        //        encoded = "FALSE";
                        //    }
                        //}
                        else
                        {
                            encoded = value.ToString();
                        }

                        if (firstOfRow)
                        {
                            firstOfRow = false;

                            lines.Add($"C;X{x + 1};Y{y + 1};K{encoded}");
                        }
                        else
                        {
                            lines.Add($"C;X{x + 1};K{encoded}");
                        }
                    }
                }
            }

            return $"ID;P\r\nB;X{biggestColumn};Y{rowCount}\r\n{string.Join("\r\n", lines)}\r\nE";
        }
    }
}
