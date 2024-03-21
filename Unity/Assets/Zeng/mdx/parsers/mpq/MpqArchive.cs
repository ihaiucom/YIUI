
using MdxLib.ModelFormats.Mdx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Zeng.mdx.parsers.mpq
{
    public class MpqArchive
    {
        public long headerOffset;
        public long sectorSize;
        public MpqCrypto c;
        public MpqHashTable hashTable;
        public MpqBlockTable blockTable;
        public Dictionary<uint, MpqFile> files;
        public bool isReadonly = false;

        public MpqArchive()
        {
            this.headerOffset = 0;
            this.sectorSize = 4096;
            this.c = new MpqCrypto();
            this.hashTable = new MpqHashTable(this.c);
            this.blockTable = new MpqBlockTable(this.c);
            this.files = new Dictionary<uint, MpqFile>();
        }


        public void load(byte[] buffer, Stream stream, CLoader cloader,  bool isReadonly = false)
        {
            this.isReadonly = isReadonly;

            int headerOffset = MpqArchiveUtils.searchHeader(buffer);

            if (headerOffset == -1)
            {
                throw new Exception("No MPQ header");
            }

            stream.Position = headerOffset;
            cloader.Reader.ReadUInt32();
            uint headerSize = cloader.Reader.ReadUInt32();
            uint archiveSize = cloader.Reader.ReadUInt32();
            uint formatVersionSectorSize = cloader.Reader.ReadUInt32();

            uint hashPos = cloader.Reader.ReadUInt32() + (uint)headerOffset;
            uint blockPos = cloader.Reader.ReadUInt32() + (uint)headerOffset;
            uint hashSize = cloader.Reader.ReadUInt32();
            uint blockSize = cloader.Reader.ReadUInt32();

            if (blockSize > hashSize) {
                blockSize = hashSize;
            }

            this.headerOffset = headerOffset;
            this.sectorSize = 512 * (1 << ((int)formatVersionSectorSize >> 16)); // Generally 4096

            // Read the hash table.
            // Also clears any existing entries.
            // Have to copy the data, because hashPos is not guaranteed to be a multiple of 4.
            this.hashTable.load(buffer[(int)hashPos..(int)(hashPos + hashSize * 16)]);


            // Read the block table.
            // Also clears any existing entries.
            // Have to copy the data, because blockPos is not guaranteed to be a multiple of 4.
            this.blockTable.load(buffer[(int)blockPos .. (int)(blockPos + blockSize * 16)]);


            // Clear any existing files.
            this.files.Clear();


            // Read the files.
            foreach (var hash in this.hashTable.entries) {
                uint blockIndex =  hash.blockIndex;

                // If the block index is valid, load a file.
                // This isn't the case when the block is marked as deleted with HASH_ENTRY_DELETED.
                // This also isn't the case for archives with fake block indices.
                if (blockIndex < this.blockTable.entries.Count)
                {
                    this.files[blockIndex] = new MpqFile(this, hash, this.blockTable.entries[(int)blockIndex], buffer, null);
                }
            }

            // If there is a listfile, use all of the file names in it.
            MpqFile listfile = this.get("(listfile)");
            if (listfile != null)
            {
                string list = listfile.text();

                if (list != null)
                {
                    string[] arr = list.Split("\r\n");
                    foreach (string name in arr)
                    {
                        // get() internally also sets the file's name to the given one.
                        this.get(name);
                    }
                }
            }

        }


        /**
         * Gets a file from this archive.
         * If the file doesn't exist, null is returned.
         */
        public MpqFile get(string name)
        {
            MpqHash hash = this.hashTable.get(name);

            if (hash != null)
            {
                uint blockIndex = hash.blockIndex;

                // Check if the block exists.
                if (blockIndex < MpqConstants.HASH_ENTRY_DELETED)
                {
                    MpqFile file = this.files[blockIndex];

                    if (file != null)
                    {
                        // Save the name in case it wasn't already resolved.
                        file.name = name;
                        file.nameResolved = true;

                        return file;
                    }
                }
            }

            return null;
        }

    }

}
