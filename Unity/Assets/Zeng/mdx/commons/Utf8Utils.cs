using System;
using System.Collections.Generic;
namespace Zeng.mdx.commons
{
    public class TextDecoder
    {
        public static string DecodeUtf8(byte[] buffer)
        {
            return System.Text.Encoding.UTF8.GetString(buffer);
        }
    }

    public class TextEncoder
    {
        public static byte[] Encode(string utf8)
        {
            return System.Text.Encoding.UTF8.GetBytes(utf8);
        }
    }

    public class Utf8Utils
    {
        public static int ByteLength(string str)
        {
            int s = str.Length;
            for (int i = str.Length - 1; i >= 0; i--)
            {
                int code = (int)str[i];
                if (code > 0x7f && code <= 0x7ff) s++;
                else if (code > 0x7ff && code <= 0xffff) s += 2;
                if (code >= 0xDC00 && code <= 0xDFFF) i--; //trail surrogate
            }
            return s;
        }

        public static string[] SplitByteLength(string str, int chunkBytelength)
        {
            var chunks = new List<string>();
            int pos = 0;
            int bytes = 0;

            for (int i = 0, l = str.Length; i < l; i++)
            {
                int code = (int)str[i];

                if (code < 0x80)
                {
                    bytes += 1;
                }
                else if (code < 0x800)
                {
                    bytes += 2;
                }
                else if (code < 0xd800 || code >= 0xe000)
                {
                    bytes += 3;
                }
                else
                {
                    i++;
                    bytes += 4;
                }

                if (bytes >= chunkBytelength - 3)
                {
                    chunks.Add(str.Substring(pos, i));

                    pos += i;
                    bytes = 0;
                }
            }

            if (bytes > 0)
            {
                chunks.Add(str.Substring(pos));
            }

            return chunks.ToArray();
        }
    }
}