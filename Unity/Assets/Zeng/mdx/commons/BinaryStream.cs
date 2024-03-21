using System;
using System.Text;

namespace Zeng.mdx.commons
{
    /**
    * A binary stream.
    */
    public class BinaryStream
    {
        private byte[] buffer;
        private byte[] uint8array;
        private int index = 0;
        private int byteLength;
        private int remaining;

        public int Index
        {
            get { return index; }
        }

        public int Remaining
        {
            get { return remaining; }
        }

        public byte[] Uint8array { 
            get { return uint8array; }
        }

        public BinaryStream(byte[] buffer, int byteOffset = 0, int byteLength = -1)
        {
            if (byteLength == -1)
            {
                byteLength = buffer.Length - byteOffset;
            }

            this.buffer = buffer;
            this.uint8array = new byte[byteLength];
            Array.Copy(buffer, byteOffset, this.uint8array, 0, byteLength);
            this.byteLength = byteLength;
            this.remaining = byteLength;
        }

        /**
         * Create a subreader of this reader, at its position, with the given byte length.
         */
        public BinaryStream Substream(int byteLength)
        {
            if (this.remaining < byteLength)
            {
                throw new Exception($"ByteStream: substream: want {byteLength} bytes but have {this.remaining}");
            }

            int index = this.index;

            this.index += byteLength;

            return new BinaryStream(this.uint8array[index..(index + byteLength)]);
        }

        /**
         * Skip a number of bytes.
         */
        public void Skip(int bytes)
        {
            if (this.remaining < bytes)
            {
                throw new Exception($"ByteStream: skip: premature end - want {bytes} bytes but have {this.remaining}");
            }

            this.index += bytes;
            this.remaining -= bytes;
        }

        /**
         * Set the reader's index.
         */
        public void Seek(int index)
        {
            this.index = index;
            this.remaining = this.byteLength - index;
        }

        /**
         * Read a UTF8 string with the given number of bytes.
         * 
         * The entire size will be read, however the string returned is NULL terminated in its memory block.
         * 
         * For example, the MDX format has many strings that have a constant maximum size, where any bytes after the string are NULLs.
         * Such strings will be loaded correctly given the maximum size.
         */
        public string Read(int bytes)
        {
            if (this.remaining < bytes)
            {
                throw new Exception($"ByteStream: read: premature end - want {bytes} bytes but have {this.remaining}");
            }

            byte[] uint8array = this.uint8array;
            int start = this.index;
            int end = Searches.BoundIndexOf(uint8array, 0, start, bytes);

            if (end == -1)
            {
                end = start + bytes;
            }

            this.index += bytes;
            this.remaining -= bytes;

            return Encoding.UTF8.GetString(uint8array[start..end]);
        }
        /**
         * Read a UTF8 NULL terminated string.
         */
        public string ReadNull()
        {
            if (this.remaining < 1)
            {
                throw new Exception("ByteStream: readNull: premature end - want at least 1 byte but have 0");
            }

            byte[] uint8array = this.uint8array;
            int start = this.index;
            int end = Array.IndexOf(uint8array, (byte)0, start);

            if (end == -1)
            {
                end = uint8array.Length - 1;
            }

            int bytes = end - start + 1;

            this.index += bytes;
            this.remaining -= bytes;

            return Encoding.UTF8.GetString(uint8array, start, end - start);
        }

        /**
         * Read a binary string with the given number of bytes.
         */
        public string ReadBinary(int bytes)
        {
            if (this.remaining < bytes)
            {
                throw new Exception($"ByteStream: readBinary: premature end - want {bytes} bytes but have {this.remaining}");
            }

            byte[] uint8array = this.uint8array;
            int index = this.index;
            string data = "";

            for (int i = 0; i < bytes; i++)
            {
                data += (char)uint8array[index + i];
            }

            this.index += bytes;
            this.remaining -= bytes;

            return data;
        }


        /**
         * Read a 8 bit signed integer.
         */
        public sbyte ReadInt8()
        {
            if (remaining < 1)
            {
                throw new Exception($"ByteStream: readInt8: premature end - want 1 byte but have {remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;
            sbyte data = Convert.ToSByte(uint8array[index]);

            this.index += 1;
            this.remaining -= 1;

            return data;
        }

        /**
         * Read a 16 bit signed integer.
         */
        public short ReadInt16()
        {
            if (this.remaining < 2)
            {
                throw new Exception($"ByteStream: readInt16: premature end - want 2 bytes but have {this.remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;
            short data = BitConverter.ToInt16(uint8array, index);

            this.index += 2;
            this.remaining -= 2;

            return data;
        }

        /**
         * Read a 32 bit signed integer.
         */
        public int ReadInt32()
        {
            if (remaining < 4)
            {
                throw new Exception($"ByteStream: readInt32: premature end - want 4 bytes but have {remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;
            int data = BitConverter.ToInt32(uint8array, index);

            this.index += 4;
            this.remaining -= 4;

            return data;
        }


        /**
         * Read a 8 bit unsigned integer.
         */
        public byte ReadUint8()
        {
            if (remaining < 1)
            {
                throw new Exception($"ByteStream: readUint8: premature end - want 1 byte but have {remaining}");
            }

            byte data = uint8array[index];

            index += 1;
            remaining -= 1;

            return data;
        }

        /// <summary>
        /// Read a 16 bit unsigned integer.
        /// </summary>
        /// <returns>The read unsigned integer.</returns>
        public ushort ReadUint16()
        {
            if (remaining < 2)
            {
                throw new Exception($"ByteStream: readUint16: premature end - want 2 bytes but have {remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;
            ushort data = BitConverter.ToUInt16(uint8array, index);

            this.index += 2;
            this.remaining -= 2;

            return data;
        }

        /**
         * Read a 32 bit unsigned integer.
         */
        public uint ReadUint32()
        {
            if (remaining < 4)
            {
                throw new Exception($"ByteStream: readUint32: premature end - want 4 bytes but have {remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;
            uint data = BitConverter.ToUInt32(uint8array, index);

            this.index += 4;
            this.remaining -= 4;

            return data;
        }

        /**
         * Read a 32 bit float.
         */
        public float ReadFloat32()
        {
            if (remaining < 4)
            {
                throw new Exception($"ByteStream: ReadFloat32: premature end - want 4 bytes but have {remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;
            float data = BitConverter.ToSingle(uint8array, index);

            this.index += 4;
            this.remaining -= 4;

            return data;
        }


        /**
         * Read a 64 bit float.
         */
        public double ReadFloat64()
        {
            if (remaining < 8)
            {
                throw new Exception($"ByteStream: ReadFloat64: premature end - want 8 bytes but have {remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;
            double data = BitConverter.ToDouble(uint8array, index);

            this.index += 8;
            this.remaining -= 8;

            return data;
        }


        /**
         * Read an array of 8 bit signed integers.
         */

        public sbyte[] ReadInt8Array(int length)
        {
            return ReadInt8Array(new sbyte[length]);
        }
        public sbyte[] ReadInt8Array(sbyte[] view)
        {

            if (this.remaining < view.Length)
            {
                throw new Exception($"ByteStream: ReadInt8Array: premature end - want {view.Length} bytes but have {this.remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = view.Length; i < l; i++)
            {
                view[i] = Convert.ToSByte(uint8array[index + i]);
            }

            this.index += view.Length;
            this.remaining -= view.Length;

            return view;
        }


        /**
        * Read an array of 16 bit signed integers.
        */

        public short[] ReadInt16Array(int length)
        {
            return ReadInt16Array(new short[length]);
        }
        public short[] ReadInt16Array(short[] view)
        {
            int byteLength = view.Length * 2;
            if (this.remaining < view.Length * 2)
            {
                throw new Exception($"ByteStream: ReadInt16Array: premature end - want {byteLength} bytes but have {this.remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = view.Length; i < l; i++)
            {
                int offset = index + i * 2;
                view[i] = BitConverter.ToInt16(uint8array, offset);
            }

            this.index += byteLength;
            this.remaining -= byteLength;

            return view;
        }


        /**
         * Read an array of 32 bit signed integers.
         */
        public int[] ReadInt32Array(int length)
        {
            return ReadInt32Array(new int[length]);
        }

        public int[] ReadInt32Array(int[] view)
        {
            int byteLength = view.Length * 4;
            if (this.remaining < byteLength)
            {
                throw new Exception($"ByteStream: ReadInt32Array: premature end - want {byteLength} bytes but have {this.remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = view.Length; i < l; i++)
            {
                int offset = index + i * 4;

                view[i] = BitConverter.ToInt32(uint8array, offset);
            }

            this.index += byteLength;
            this.remaining -= byteLength;

            return view;
        }


        /**
         * Read an array of 8 bit unsigned integers.
         */
        public byte[] ReadUint8Array(int length)
        {
            return ReadUint8Array(new byte[length]);
        }
        public byte[] ReadUint8Array(byte[] byteArray)
        {
            int byteLength = byteArray.Length;
            if (this.remaining < byteLength)
            {
                throw new Exception($"ByteStream: ReadUint8Array: premature end - want {byteLength} bytes but have {this.remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = byteArray.Length; i < l; i++)
            {
                byteArray[i] = uint8array[index + i];
            }

            this.index += byteLength;
            this.remaining -= byteLength;

            return byteArray;
        }


        /**
         * Read an array of 16 bit unsigned integers.
         */
        public ushort[] ReadUint16Array(int length)
        {
            return ReadUint16Array(new ushort[length]);
        }
        public ushort[] ReadUint16Array(ushort[] result)
        {

            int byteLength = result.Length * 2;
            if (this.remaining < byteLength)
            {
                throw new Exception($"ByteStream: ReadUint16Array: premature end - want {byteLength} bytes but have {this.remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = result.Length; i < l; i++)
            {
                int offset = index + i * 2;

                result[i] = BitConverter.ToUInt16(uint8array, offset);
            }

            this.index += byteLength;
            this.remaining -= byteLength;

            return result;
        }



        /**
         * Read an array of 32 bit unsigned integers.
         */
        public uint[] ReadUint32Array(int length)
        {
            return ReadUint32Array(new uint[length]);
        }
        public uint[] ReadUint32Array(uint[] view)
        {
            int byteLength = view.Length * 4;
            if (this.remaining < byteLength)
            {
                throw new Exception($"ByteStream: ReadUint32Array: premature end - want {byteLength} bytes but have {this.remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = view.Length; i < l; i++)
            {
                int offset = index + i * 4;

                view[i] = BitConverter.ToUInt32(uint8array, offset);
            }

            this.index += byteLength;
            this.remaining -= byteLength;

            return view;
        }

        /**
         * Read an array of 32 bit floats.
         */

        public float[] ReadFloat32Array(int length)
        {
            return ReadFloat32Array(new float[length]);
        }

        public float[] ReadFloat32Array(float[] view)
        {
            int byteLength = view.Length * 4;
            if (this.remaining < byteLength)
            {
                throw new Exception($"ByteStream: readFloat32Array: premature end - want {byteLength} bytes but have {this.remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0; i < view.Length; i++)
            {
                int offset = index + i * 4;

                view[i] = BitConverter.ToSingle(uint8array, offset);
            }

            this.index += byteLength;
            this.remaining -= byteLength;

            return view;
        }

        /**
         * Read an array of 64 bit floats.
         */
        public double[] ReadFloat64Array(int length)
        {
            return ReadFloat64Array(new double[length]);
        }

        public double[] ReadFloat64Array(double[] view)
        {
            int byteLength = view.Length * 8;
            if (this.remaining < view.Length * 8)
            {
                throw new Exception($"ByteStream: readFloat64Array: premature end - want {byteLength} bytes but have {this.remaining}");
            }

            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0; i < view.Length; i++)
            {
                int offset = index + i * 8;

                view[i] = BitConverter.ToDouble(uint8array, offset);
            }

            this.index += byteLength;
            this.remaining -= byteLength;

            return view;
        }

        /**
         * Write a UTF8 string.
         * 
         * Returns the number of bytes that were written,
         */
        public int Write(string utf8)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(utf8);

            this.WriteUint8Array(bytes);

            return bytes.Length;
        }

        /**
         * Write a UTF8 string as a NULL terminated string.
         * 
         * Returns the number of bytes that were written, including the terminating NULL.
         */
        public int WriteNull(string utf8)
        {
            int bytes = this.Write(utf8);

            this.index++;
            this.remaining--;

            return bytes + 1;
        }

        /**
         * Write a binary string.
         */
        public void WriteBinary(string value)
        {
            int index = this.index;
            byte[] uint8array = this.uint8array;
            int count = value.Length;

            for (int i = 0; i < count; i++)
            {
                uint8array[index + i] = (byte)value[i];
            }

            this.index += count;
        }

        /**
         * Write a 8 bit signed integer.
         */
        public void WriteInt8(sbyte value)
        {
            this.uint8array[this.index] = Convert.ToByte(value);
            this.index += 1;
        }


        /**
         * Write a 16 bit signed integer.
         */
        public void WriteInt16(short value)
        {
            int index = this.index;
            byte[] uint8array = this.uint8array;

            byte[] data = BitConverter.GetBytes(value);
            uint8array[index] = data[0];
            uint8array[index + 1] = data[1];


            this.index += 2;
        }

        /**
         * Write a 32 bit signed integer.
         */
        public void WriteInt32(int value)
        {
            int index = this.index;
            byte[] uint8array = this.uint8array;


            byte[] data = BitConverter.GetBytes(value);
            uint8array[index] = data[0];
            uint8array[index + 1] = data[1];
            uint8array[index + 2] = data[2];
            uint8array[index + 3] = data[3];


            this.index += 4;
        }

        /**
         * Write a 8 bit unsigned integer.
         */
        public void WriteUint8(byte value)
        {
            this.uint8array[this.index] = value;
            this.index += 1;
        }


        /**
         * Write a 16 bit unsigned integer.
         */
        public void WriteUint16(ushort value)
        {
            int index = this.index;
            byte[] uint8array = this.uint8array;

            byte[] data = BitConverter.GetBytes(value);
            uint8array[index] = data[0];
            uint8array[index + 1] = data[1];


            this.index += 2;
        }

        /**
         * Write a 32 bit unsigned integer.
         */
        public void WriteUint32(uint value)
        {
            uint[] uint32 = new uint[4];
            uint32[0] = value & 0xFF;
            uint32[1] = (value >> 8) & 0xFF;
            uint32[2] = (value >> 16) & 0xFF;
            uint32[3] = (value >> 24) & 0xFF;

            uint8array[index] = (byte)uint32[0];
            uint8array[index + 1] = (byte)uint32[1];
            uint8array[index + 2] = (byte)uint32[2];
            uint8array[index + 3] = (byte)uint32[3];

            this.index += 4;
        }

        /**
         * Write a 32 bit float.
         */
        public void WriteFloat32(float value)
        {
            byte[] float32 = BitConverter.GetBytes(value);

            uint8array[index] = float32[0];
            uint8array[index + 1] = float32[1];
            uint8array[index + 2] = float32[2];
            uint8array[index + 3] = float32[3];

            this.index += 4;
        }
        /**
         * Write a 64 bit float.
         */
        public void WriteFloat64(double value)
        {
            int index = this.index;
            byte[] uint8array = this.uint8array;

            byte[] data = BitConverter.GetBytes(value);


            uint8array[index] = data[0];
            uint8array[index + 1] = data[1];
            uint8array[index + 2] = data[2];
            uint8array[index + 3] = data[3];
            uint8array[index + 4] = data[4];
            uint8array[index + 5] = data[5];
            uint8array[index + 6] = data[6];
            uint8array[index + 7] = data[7];

            this.index += 8;
        }

        /**
         * Write an array of 8 bit signed integers.
         */
        public void WriteInt8Array(sbyte[] view)
        {
            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = view.Length; i < l; i++)
            {
                uint8array[index + i] = Convert.ToByte(view[i]);
            }

            this.index += view.Length;
        }

        /**
         * Write an array of 16 bit signed integers.
         */
        public void WriteInt16Array(short[] view)
        {
            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = view.Length; i < l; i++)
            {
                int offset = index + i * 2;

                byte[] data = BitConverter.GetBytes(view[i]);

                uint8array[offset] = data[0];
                uint8array[offset + 1] = data[1];
            }

            this.index += view.Length * 2;
        }

        /**
         * Write an array of 32 bit signed integers.
         */
        public void WriteInt32Array(int[] view)
        {
            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = view.Length; i < l; i++)
            {
                int offset = index + i * 4;

                byte[] data = BitConverter.GetBytes(view[i]);


                uint8array[offset] = data[0];
                uint8array[offset + 1] = data[1];
                uint8array[offset + 2] = data[2];
                uint8array[offset + 3] = data[3];
            }

            this.index += view.Length * 4;
        }

        /**
         * Write an array of 8 bit unsigned integers.
         */
        public void WriteUint8Array(byte[] view)
        {
            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = view.Length; i < l; i++)
            {
                uint8array[index + i] = view[i];
            }

            this.index += view.Length;
        }

        /**
         * Write an array of 16 bit unsigned integers.
         */
        public void WriteUint16Array(ushort[] view)
        {
            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = view.Length; i < l; i++)
            {
                int offset = index + i * 2;

                byte[] uint8 = BitConverter.GetBytes(view[i]);

                uint8array[offset] = uint8[0];
                uint8array[offset + 1] = uint8[1];
            }

            this.index += view.Length * 2;
        }

        /**
         * Write an array of 32 bit unsigned integers.
         */
        public void WriteUint32Array(uint[] view)
        {
            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = view.Length; i < l; i++)
            {
                int offset = index + i * 4;

                byte[] uint8 = BitConverter.GetBytes(view[i]);


                uint8array[offset] = uint8[0];
                uint8array[offset + 1] = uint8[1];
                uint8array[offset + 2] = uint8[2];
                uint8array[offset + 3] = uint8[3];
            }

            this.index += view.Length * 4;
        }

        /**
         * Write an array of 32 bit floats.
         */
        public void WriteFloat32Array(float[] view)
        {
            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = view.Length; i < l; i++)
            {
                int offset = index + i * 4;

                byte[] uint8 = BitConverter.GetBytes(view[i]);


                uint8array[offset] = uint8[0];
                uint8array[offset + 1] = uint8[1];
                uint8array[offset + 2] = uint8[2];
                uint8array[offset + 3] = uint8[3];
            }

            this.index += view.Length * 4;
        }

        /**
         * Write an array of 64 bit floats.
         */
        public void WriteFloat64Array(double[] view)
        {
            int index = this.index;
            byte[] uint8array = this.uint8array;

            for (int i = 0, l = view.Length; i < l; i++)
            {
                int offset = index + i * 8;

                byte[] uint8 = BitConverter.GetBytes(view[i]);


                uint8array[offset] = uint8[0];
                uint8array[offset + 1] = uint8[1];
                uint8array[offset + 2] = uint8[2];
                uint8array[offset + 3] = uint8[3];
                uint8array[offset + 4] = uint8[4];
                uint8array[offset + 5] = uint8[5];
                uint8array[offset + 6] = uint8[6];
                uint8array[offset + 7] = uint8[7];
            }

            this.index += view.Length * 8;
        }

    }
}
