using System;

namespace Zeng.mdx.commons
{
    public class Searches
    {
        public static bool IsStringInBytes(byte[] buffer, string target, int offset = 0, int length = int.MaxValue)
        {
            int start = Math.Max(offset, 0);
            int end = Math.Min(start + length, buffer.Length);
            int whichByte = 0;
            int targetByte = target[0];

            for (int i = start; i < end; i++)
            {
                byte byteValue = buffer[i];

                if (byteValue == targetByte)
                {
                    whichByte += 1;

                    if (whichByte == target.Length)
                    {
                        return true;
                    }

                    targetByte = target[whichByte];
                }
                else if (whichByte > 0)
                {
                    whichByte = 0;
                    targetByte = target[0];
                }
            }

            return false;
        }

        public static bool IsStringInString(string buffer, string target, int offset = 0, int length = int.MaxValue)
        {
            int start = Math.Max(offset, 0);
            int end = Math.Min(start + length, buffer.Length);
            int whichByte = 0;
            char targetByte = target[0];

            for (int i = start; i < end; i++)
            {
                char charValue = buffer[i];

                if (charValue == targetByte)
                {
                    whichByte += 1;

                    if (whichByte == target.Length)
                    {
                        return true;
                    }

                    targetByte = target[whichByte];
                }
                else if (whichByte > 0)
                {
                    whichByte = 0;
                    targetByte = target[0];
                }
            }

            return false;
        }

        public static int BoundIndexOf(byte[] buffer, int target, int offset = 0, int length = int.MaxValue)
        {
            int start = Math.Max(offset, 0);
            int end = Math.Min(start + length, buffer.Length);

            for (int i = start; i < end; i++)
            {
                if (buffer[i] == target)
                {
                    return i;
                }
            }

            return -1;
        }

    }
}
