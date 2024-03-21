using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using Zeng.mdx.commons;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

namespace Zeng.mdx.parsers.mpq
{
    public class MpqFile
    {
        public MpqArchive archive;
        public MpqCrypto c;
        public string name;
        public bool nameResolved;
        public MpqHash hash;
        public MpqBlock block;
        public byte[] rawBuffer;
        public byte[] buffer;


        public MpqFile(MpqArchive archive, MpqHash hash, MpqBlock block, byte[] rawBuffer, byte[] buffer) {
            long headerOffset = archive.headerOffset;

            this.archive = archive;
            this.c = archive.c;
            this.name = $"File{ hash.blockIndex.ToString().PadLeft(8, '0') }";
            this.nameResolved = false;
            this.hash = hash;
            this.block = block;

            if (rawBuffer != null)
            {
                //long start = headerOffset + block.offset;
                //long end = headerOffset + block.offset + block.compressedSize;
                //long length = end - start;
                //this.rawBuffer = new byte[length];
                //Array.Copy(rawBuffer, start, this.rawBuffer, 0, length);

                int start = (int)(headerOffset + block.offset);
                int end = (int) (headerOffset + block.offset + block.compressedSize);
                this.rawBuffer = rawBuffer[start .. end];
            }

            if (buffer != null) { 
                this.buffer = buffer;
            }
        }


        /**
         * Gets this file's data as an ArrayBuffer.
         * 
         * An exception will be thrown if the file needs to be decoded, and decoding fails.
         */

        public byte[] arrayBuffer()
        {
            return this.bytes();
        }


        /**
         * Gets this file's data as a UTF8 string.
         * 
         * An exception will be thrown if the file needs to be decoded, and decoding fails.
         */
        public string text(){
            byte[] bytes = this.bytes();
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public byte[] bytes() {
            if (this.buffer == null) {
                this.decode();
            }

            return this.buffer;
        }

        public byte[] decompressSector(byte[] bytes, int decompressedSize) {

            // If the size of the data is the same as its decompressed size, it's not compressed.
            if (bytes.Length == decompressedSize)
            {
                return bytes;
            }
            else
            {
                byte compressionMask = bytes[0];

                if ((compressionMask & MpqConstants.COMPRESSION_BZIP2) != 0)
                {
                    throw new Exception($"File { this.name }: compression type 'bzip2' not supported");
                }

                if ((compressionMask & MpqConstants.COMPRESSION_IMPLODE) != 0)
                {
                    try
                    {
                        bytes = MqlExplode.explode(bytes[1..bytes.Length]);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"File { this.name }: failed to decompress with 'explode': { e}");
                    }
                }

                if ((compressionMask & MpqConstants.COMPRESSION_DEFLATE) != 0)
                {
                    //try
                    //{
                        bytes = MqlInflate.inflate(bytes[1..bytes.Length]);
                    //}
                    //catch (Exception e)
                    //{
                    //    throw new Exception($"File { this.name }: failed to decompress with 'zlib': ${ e}");
                    //}
                }

                if ((compressionMask & MpqConstants.COMPRESSION_HUFFMAN) != 0)
                {
                    // try {
                    //   bytes = decodeHuffman(bytes.subarray(1));
                    // } catch (e) {
                    //   throw new Error("File ${this.name}: failed to decompress with 'huffman': ${e}");
                    // }
                    throw new Exception($"File { this.name }: compression type 'huffman' not supported");
                }

                if ((compressionMask & MpqConstants.COMPRESSION_ADPCM_STEREO) != 0)
                {
                    throw new Exception($"File { this.name }: compression type 'adpcm stereo' not supported");
                }

                if ((compressionMask & MpqConstants.COMPRESSION_ADPCM_MONO) != 0)
                {
                    throw new Exception($"File { this.name }: compression type 'adpcm mono' not supported");
                }

                return bytes;
            }
        }

        public void decode()
        {
            if (this.rawBuffer == null)
            {
                throw new Exception($"File {this.name}: Nothing to decode");
            }

            uint encryptionKey = c.computeFileKey(this.name, block);

            byte[] data = this.rawBuffer;
            uint flags = block.flags;


            // One buffer of raw data.
            // I don't know why having no flags means it's a chunk of memory rather than sectors.
            // After all, there is no flag to say there are indeed sectors.
            if (flags == MpqConstants.FILE_EXISTS)
            {
                this.buffer = data[0..(int)block.normalSize];
            }
            else if ((flags & MpqConstants.FILE_SINGLE_UNIT) != 0)
            { 
                // One buffer of possibly encrypted and/or compressed data.
                // Read the sector
                byte[] sector;

                // If this block is encrypted, decrypt the sector.
                if ((flags & MpqConstants.FILE_ENCRYPTED) != 0)
                {
                    sector = c.decryptBlock(data[0 .. ((int)block.compressedSize)], encryptionKey);
                }
                else
                {
                    sector = data[0..(int)block.compressedSize];
                }

                // If this block is compressed, decompress the sector.
                // Otherwise, copy the sector as-is.
                if ((flags & MpqConstants.FILE_COMPRESSED) != 0)
                {
                    sector = this.decompressSector(sector, (int) block.normalSize);
                }
                else
                {
                    sector = sector[0..sector.Length];
                }

                this.buffer = sector;
            }
            else
            { // One or more sectors of possibly encrypted and/or compressed data.
                int sectorCount = Mathf.CeilToInt(1f * block.normalSize / archive.sectorSize);

                // Alocate a buffer for the uncompressed block size
                byte[] buffer =new byte[block.normalSize];

                // Get the sector offsets
                int iend = (sectorCount + 1) * 4;
                uint[] sectorOffsets = BytesUtils.Uint32Array(data[0 ..iend]);

                // If this file is encrypted, copy the sector offsets and decrypt them.
                if ((flags & MpqConstants.FILE_ENCRYPTED) != 0)
                {

                    sectorOffsets = c.decryptBlock(sectorOffsets[0..sectorOffsets.Length], encryptionKey - 1);
                }

                int start = (int)sectorOffsets[0];
                int end = (int)sectorOffsets[1];
                int offset = 0;

                for (int i = 0; i < sectorCount; i++)
                {
                    byte[] sector;

                    // If this file is encrypted, copy the sector and decrypt it.
                    // Otherwise a view can be used directly.
                    if ((flags & MpqConstants.FILE_ENCRYPTED) != 0)
                    {
                        sector = c.decryptBlock(data[start .. end], (uint)(encryptionKey + i));
                    }
                    else
                    {
                        sector = data[start..end];
                    }

                    // Decompress the sector
                    if ((flags & MpqConstants.FILE_COMPRESSED) != 0)
                    {
                        int uncompressedSize = (int)archive.sectorSize;

                        // If this is the last sector, its uncompressed size might not be the size of a sector.
                        if (block.normalSize - offset < uncompressedSize)
                        {
                            uncompressedSize = (int)(block.normalSize - offset);
                        }

                        sector = this.decompressSector(sector, uncompressedSize);
                    }

                    // Some sectors have this flags instead of the compression flag + algorithm byte.
                    if ((flags & MpqConstants.FILE_IMPLODE) != 0)
                    {
                        sector = MqlExplode.explode(sector);
                    }

                    // Add the sector bytes to the buffer
                    Array.Copy(sector, 0, buffer, offset, sector.Length);
                    offset += sector.Length;

                    // Prepare for the next sector
                    if ((i + 1) < sectorCount)
                    {
                        start = end;
                        end = (int)sectorOffsets[i + 2];
                    }
                }

                this.buffer = buffer;
            }

            // If the archive is in read-only mode, the raw buffer isn't needed anymore, so free the memory.
            if (archive.isReadonly) {
                this.rawBuffer = null;
            }
        }


    }
}
