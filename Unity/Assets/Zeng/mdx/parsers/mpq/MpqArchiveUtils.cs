
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Zeng.mdx.parsers.mpq
{
    public class MpqArchiveUtils
    {
        public static int searchHeader(byte[] bytes)
        {
            int offset = -1;
            for (int i = 0,  l = Mathf.CeilToInt(1f * bytes.Length / 512); i < l; i ++)
            {
                int b = i * 512;

                // Test 'MPQ\x1A'.
                if (bytes[b] == 77 && bytes[b + 1] == 80 && bytes[b + 2] == 81 && bytes[b + 3] == 26)
                {
                    offset = b;
                }
            }
            Debug.Log("offset=" + offset);
            return offset;
        }

        public static bool isArchive(byte[] bytes) {
            // Check for the map identifier - HM3W
            if (bytes[0] == 72 && bytes[1] == 77 && bytes[2] == 51 && bytes[3] == 87)
            {
                return true;
            }

            // Look for an MPQ header.
            return searchHeader(bytes) != -1;
        }
    }
}
