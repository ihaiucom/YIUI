using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mpq
{
    public class MpqCrypto
    {
        public uint[] cryptTable = new uint[0x500];

        public MpqCrypto()
        {
            uint seed = 0x00100001;
            uint temp1 = 0;
            uint temp2 = 0;


            for (int index1 = 0; index1 < 0x100; index1++)
            {
                for (int index2 = index1, i = 0; i < 5; i += 1, index2 += 0x100)
                {
                    seed = (seed * 125 + 3) % 0x2AAAAB;
                    temp1 = (seed & 0xFFFF) << 0x10;

                    seed = (seed * 125 + 3) % 0x2AAAAB;
                    temp2 = (seed & 0xFFFF);

                    this.cryptTable[index2] = temp1 | temp2;
                }
            }
        }

        public uint hash(string name, uint key)
        {
            uint seed1 = 0x7FED7FED;
            uint seed2 = 0xEEEEEEEE;

            name = name.ToUpper();

            for (int i = 0; i < name.Length; i++)
            {
                char ch = name[i];

                seed1 = cryptTable[(key << 8) + ch] ^ (seed1 + seed2);
                seed2 = ch + seed1 + seed2 + (seed2 << 5) + 3;
            }

            // Convert the seed to an unsigned integer
            return seed1 >> 0;
        }

        public uint[] decryptBlock(uint[] data, uint key)
        {
            byte[] bytes = BytesUtils.ToBytes(data);
            bytes = decryptBlock(bytes, key);
            uint[] result = BytesUtils.Uint32Array(bytes);
            return result;
        }

        public byte[] decryptBlock(byte[] data, uint key) {

            long seed = 0xEEEEEEEE;
            byte[] bytes = new byte[data.Length];
            Array.Copy(data, bytes, data.Length);

            byte[] bytesHeap = new byte[4];
            for (int i = 0, l = bytes.Length >> 2; i < l; i ++)
            {
                // Update the seed.
                uint ii = 0x400 + (key & 0xFF);
                uint cv = cryptTable[ii];
                seed += cv;

                // Get 4 encrypted bytes.
                bytesHeap[0] = bytes[i * 4];
                bytesHeap[1] = bytes[i * 4 + 1];
                bytesHeap[2] = bytes[i * 4 + 2];
                bytesHeap[3] = bytes[i * 4 + 3];

                // Decrypted 32bit integer.
                uint longHeap = BitConverter.ToUInt32(bytesHeap, 0);
                long keySeed = key + seed;
                long longHeap2 = (longHeap ^ keySeed);
                longHeap = (uint)longHeap2;

                // Update the seed.
                key = (uint)(((~key << 0x15) + 0x11111111) | (BytesUtils.RightMove((int)key, 0x0B)));
                //seed = longHeap + seed + (seed << 5) + 3;
                uint ss = ((uint)(seed << 5));
                long seed2 = longHeap + seed + ss + 3;
                seed = seed2;
                bytesHeap = BitConverter.GetBytes(longHeap);
                

                // Set 4 decryped bytes.
                bytes[i * 4] = bytesHeap[0];
                bytes[i * 4 + 1] = bytesHeap[1];
                bytes[i * 4 + 2] = bytesHeap[2];
                bytes[i * 4 + 3] = bytesHeap[3];

            }

            return bytes;
        }

        public uint computeFileKey(string name, MpqBlock block )
        {
            int sepIndex = name.LastIndexOf('\\');
            string pathlessName = name.Substring(sepIndex + 1);
            uint encryptionKey = this.hash(pathlessName, MpqConstants.HASH_FILE_KEY);

            if ((block.flags & MpqConstants.FILE_OFFSET_ADJUSTED_KEY) != 0)
            {
                encryptionKey = (encryptionKey + block.offset) ^ block.normalSize;
            }

            return encryptionKey;

        }
    }
}
