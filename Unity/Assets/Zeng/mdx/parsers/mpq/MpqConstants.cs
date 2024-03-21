

namespace Zeng.mdx.parsers.mpq
{
    public class MpqConstants
    {
        public const uint MAGIC = 0x1A51504D; // MPQ\x1A reversed
        public const uint HASH_TABLE_KEY = 0xC3AF3770; // Hash of (hashtable)
        public const uint HASH_TABLE_INDEX = 0;
        public const int HASH_NAME_A = 1;
        public const int HASH_NAME_B = 2;
        public const uint HASH_FILE_KEY = 3;
        public const uint HASH_ENTRY_DELETED = 0xFFFFFFFE;
        public const uint HASH_ENTRY_EMPTY = 0xFFFFFFFF;
        public const uint BLOCK_TABLE_KEY = 0xEC83B3A3; // Hash of (blocktable)
        public const uint FILE_IMPLODE = 0x00000100;
        public const uint FILE_COMPRESSED = 0x00000200;
        public const uint FILE_ENCRYPTED = 0x00010000;
        public const uint FILE_OFFSET_ADJUSTED_KEY = 0x00020000;
        public const uint FILE_PATCH_FILE = 0x00100000;
        public const uint FILE_SINGLE_UNIT = 0x01000000;
        public const uint FILE_DELETE_MARKER = 0x02000000;
        public const uint FILE_SECTOR_CRC = 0x04000000;
        public const uint FILE_EXISTS = 0x80000000;
        public const int COMPRESSION_HUFFMAN = 0x01;
        public const int COMPRESSION_DEFLATE = 0x02;
        public const int COMPRESSION_IMPLODE = 0x08;
        public const byte COMPRESSION_BZIP2 = 0x10;
        public const int COMPRESSION_ADPCM_MONO = 0x40;
        public const int COMPRESSION_ADPCM_STEREO = 0x80;

    }
}
