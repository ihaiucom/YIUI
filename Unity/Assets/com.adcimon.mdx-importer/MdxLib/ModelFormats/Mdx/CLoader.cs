using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MdxLib.ModelFormats.Attacher;
using MdxLib.Primitives;
using UnityEngine;
using UnityEngine.XR;

namespace MdxLib.ModelFormats.Mdx
{
	public sealed class CLoader
	{
		private string _Name = "";

		private CAttacherContainer _Attacher;

		public BinaryReader Reader;

		private LinkedList<int> LocationStack;

		public long Location => Reader.BaseStream.Position;

		public string Name => _Name;

		public CAttacherContainer Attacher => _Attacher;

		public CLoader(string Name, Stream Stream)
		{
			_Name = Name;
			_Attacher = new CAttacherContainer();
			Reader = new BinaryReader(Stream, CConstants.SimpleTextEncoding);
			LocationStack = new LinkedList<int>();
		}

		public bool Eof()
		{
			return Reader.BaseStream.Position >= Reader.BaseStream.Length;
		}

		public byte[] Read(int Size)
		{
			return Reader.ReadBytes(Size);
		}

		public byte ReadByte()
		{
			return Reader.ReadByte();
		}

		public int ReadInt8()
		{
			return Reader.ReadByte();
		}

		public int ReadInt16()
		{
			return Reader.ReadInt16();
		}

		public int ReadInt32()
		{
			return Reader.ReadInt32();
        }

        public uint ReadUInt32()
        {
            return Reader.ReadUInt32();
        }


        public float ReadFloat()
		{
			return Reader.ReadSingle();
		}

		public double ReadDouble()
		{
			return Reader.ReadDouble();
		}

		public uint[] readUint32Array(uint[] view) {

			byte[] bytes = this.Read(view.Length * 4);
            view = Uint32Array(bytes, view);
			return view;
        }


        public static UInt32[] Uint32Array(byte[] bytes, uint[] uint32array)
        {
            for (int i = 0, length = uint32array.Length; i < length; i++)
            {
                int startIndex = i * 4;
                uint32array[i] = BitConverter.ToUInt32(bytes, startIndex);
            }

            return uint32array;
        }


        public int[] readInt32Array(int[] view)
        {

            byte[] bytes = this.Read(view.Length * 4);
            view = Int32Array(bytes, view);
            return view;
        }


        public static int[] Int32Array(byte[] bytes, int[] uint32array)
        {
            for (int i = 0, length = uint32array.Length; i < length; i++)
            {
                int startIndex = i * 4;
                uint32array[i] = BitConverter.ToInt32(bytes, startIndex);
            }

            return uint32array;
        }

        public float[] readFloat32Array(float[] view)
        {

            byte[] bytes = this.Read(view.Length * 4);
            view = Float32Array(bytes, view);
            return view;
        }


        public static float[] Float32Array(byte[] bytes, float[] uint32array)
        {
            for (int i = 0, length = uint32array.Length; i < length; i++)
            {
                int startIndex = i * 4;
                uint32array[i] = BitConverter.ToSingle(bytes, startIndex);
            }

            return uint32array;
        }



        public byte[] readUint8Array(byte[] view)
        {

            byte[] bytes = this.Read(view.Length);
            Array.Copy(bytes, 0, view, 0, view.Length);
            return view;
        }



        public string ReadNull()
		{
			List<byte> list = new List<byte>();
			while (Reader.BaseStream.Position < Reader.BaseStream.Length)
			{
				byte v = Reader.ReadByte();
				if (v == 0) {
					break;
                }
                list.Add(v);
            }
            byte[] bytes = list.ToArray();
            UTF8Encoding utf8 = new UTF8Encoding(true, true);
            String str = utf8.GetString(bytes, 0, bytes.Length);
            return str;
        }

		public string ReadString(int Length)
		{
			int num = Length;
			char[] array = Reader.ReadChars(Length);
			while (num > 0 && array[num - 1] == '\0')
			{
				num--;
			}
			return new string(array, 0, num);
		}

		public string ReadTag()
		{
			return ReadString(4);
		}

		public CVector2 ReadVector2()
		{
			float x = ReadFloat();
			float y = ReadFloat();
			return new CVector2(x, y);
		}

		public CVector3 ReadVector3()
		{
			float x = ReadFloat();
			float y = ReadFloat();
			float z = ReadFloat();
			return new CVector3(x, y, z);
		}

		public CVector4 ReadVector4()
		{
			float x = ReadFloat();
			float y = ReadFloat();
			float z = ReadFloat();
			float w = ReadFloat();
			return new CVector4(x, y, z, w);
		}

		public CExtent ReadExtent()
		{
			float radius = ReadFloat();
			CVector3 min = ReadVector3();
			CVector3 max = ReadVector3();
			return new CExtent(min, max, radius);
		}

		public void ExpectTag(string ExpectedTag)
		{
			string text = ReadTag();
			if (text != ExpectedTag)
			{
				throw new Exception("Error at location " + Location + ", expected \"" + ExpectedTag + "\", got \"" + text + "\"!");
			}
		}

		public void Skip(int NrOfBytes)
		{
			Reader.ReadBytes(NrOfBytes);
		}

		public void PushLocation()
		{
			LocationStack.AddLast((int)Reader.BaseStream.Position);
		}

		public int PopLocation()
		{
			int value = LocationStack.Last.Value;
			LocationStack.RemoveLast();
			return (int)Reader.BaseStream.Position - value;
		}
	}
}
