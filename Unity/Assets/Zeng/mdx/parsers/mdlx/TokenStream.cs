using System;

namespace Zeng.mdx.parsers.mdx
{
    public class TokenStream
    {

        private string buffer;
        private int index = 0;
        private int ident = 0;
        private int indentSpaces = 4;
        private int precision = 1000000; // 6 digits after the decimal point.

        public TokenStream(string buffer = "")
        {
            this.buffer = buffer;
        }

        /**
         * Clear the stream from whatever buffer it had.
         */
        public void Clear()
        {
            this.buffer = "";
            this.index = 0;
            this.ident = 0;
        }

        /**
         * 读取流中的下一个标记。
         * 在形式为""的字符串之外，忽略空格。
         * 忽略形式为//的注释。
         * 逗号和冒号也被忽略。
         * 大括号通常用作分隔符，用于表示文本块。
         *
         * 例如，给定以下字符串：
         *
         *     Header "A String" {
         *         Name Value, // A Comment
         *     }
         *
         * 读取将按顺序返回值：
         *
         *     Header
         *     "A String"
         *     {
         *     Name
         *     Value
         *     }
         *
         * 下面有一些包装器，可以帮助读取结构化代码，请查看它们！
         */


        public string ReadToken()
        {
            string buffer = this.buffer;
            int length = buffer.Length;
            bool inComment = false;
            bool inString = false;
            string token = "";

            while (this.index < length)
            {
                char c = buffer[this.index++];

                if (inComment)
                {
                    if (c == '\n')
                    {
                        inComment = false;
                    }
                }
                else if (inString)
                {
                    if (c == '\\')
                    {
                        token += c + buffer[this.index++];
                    }
                    else if (c == '\n')
                    {
                        token += "\\n";
                    }
                    else if (c == '\r')
                    {
                        token += "\\r";
                    }
                    else if (c == '"')
                    {
                        return token;
                    }
                    else
                    {
                        token += c;
                    }
                }
                else if (c == ' ' || c == ',' || c == '\t' || c == '\n' || c == ':' || c == '\r')
                {
                    if (token.Length > 0)
                    {
                        return token;
                    }
                }
                else if (c == '{' || c == '}')
                {
                    if (token.Length > 0)
                    {
                        this.index--;
                        return token;
                    }
                    else
                    {
                        return c.ToString();
                    }
                }
                else if (c == '/' && buffer[this.index] == '/')
                {
                    if (token.Length > 0)
                    {
                        this.index--;
                        return token;
                    }
                    else
                    {
                        inComment = true;
                    }
                }
                else if (c == '"')
                {
                    if (token.Length > 0)
                    {
                        this.index--;
                        return token;
                    }
                    else
                    {
                        inString = true;
                    }
                }
                else
                {
                    token += c;
                }
            }

            return null;
        }

        /**
         * Same as ReadToken, but if the end of the stream was encountered, an exception will be thrown.
         */
        public string Read()
        {
            string value = ReadToken();

            if (value == null)
            {
                throw new Exception("End of stream reached prematurely");
            }

            return value;
        }

        /**
         * Reads the next token without advancing the stream.
         */
        public string Peek()
        {
            int index = this.index;
            string value = Read();

            this.index = index;

            return value;
        }

        /**
         * Reads the next token, and parses it as an integer.
         */
        public int ReadInt()
        {
            return int.Parse(Read());
        }

        /**
         * Reads the next token, and parses it as a float.
         */
        public float ReadFloat()
        {
            return float.Parse(Read());
        }


        /**
         * { Number0, Number1, ..., NumberN }
         */
        public float[] ReadVector(float[] view)
        {
            Read(); // {

            for (int i = 0, l = view.Length; i < l; i++)
            {
                view[i] = ReadFloat();
            }

            Read(); // }

            return view;
        }


    }
}
