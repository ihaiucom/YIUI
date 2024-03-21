using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Zeng.mdx.commons
{
    public static class BytesUtils
    {
        public static byte[] ToBytes(this int[] intArr)
        {
            int intSize = sizeof(int) * intArr.Length;
            byte[] bytArr = new byte[intSize];
            //����һ����й��ڴ�  
            IntPtr ptr = Marshal.AllocHGlobal(intSize);
            //����int���鵽���ڴ��  
            Marshal.Copy(intArr, 0, ptr, intArr.Length);
            //���ƻ�byte����  
            Marshal.Copy(ptr, bytArr, 0, bytArr.Length);
            //�ͷ�����ķ��й��ڴ�  
            Marshal.FreeHGlobal(ptr);
            return bytArr;
        }

        public static byte[] ToBytes(this uint[] uintArr)
        {
            //byte[] bytes = new byte[intArr.Length * 4];
            //for(int i = 0; i < intArr.Length; i ++)
            //{
            //    byte[] items = intToBytes((int)intArr[i]);
            //    bytes[i * 4 + 0] = items[3];
            //    bytes[i * 4 + 1] = items[2];
            //    bytes[i * 4 + 2] = items[1];
            //    bytes[i * 4 + 3] = items[0];
            //}
            //return bytes;
            int[] intArr = new int[uintArr.Length];
            for(int i = 0; i < uintArr.Length; i ++)
            {
                intArr[i] = (int)uintArr[i];
            }
            return ToBytes(intArr);
        }


        /**  
        * ��int��ֵת��Ϊռ�ĸ��ֽڵ�byte���飬������������(��λ��ǰ����λ�ں�)��˳��  
        * @param value  
        *            Ҫת����intֵ 
        * @return byte���� 
        */
        public static byte[] intToBytes(int value)
        {
            byte[] byte_src = new byte[4];
            byte_src[3] = (byte)((value & 0xFF000000) >> 24);
            byte_src[2] = (byte)((value & 0x00FF0000) >> 16);
            byte_src[1] = (byte)((value & 0x0000FF00) >> 8);
            byte_src[0] = (byte)((value & 0x000000FF));
            return byte_src;
        } 


        public static UInt32[] Uint32Array(this byte[] bytes)
        {
            UInt32[] uint32array = new UInt32[bytes.Length / 4];
            for(int i = 0, length = uint32array.Length; i < length; i ++)
            {
                int startIndex = i * 4;
                uint32array[i] = BitConverter.ToUInt32(bytes, startIndex);
            }

            return uint32array;
        }

        /// <summary>
        /// �޷�������, �൱��java��� value>>>pos
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static int RightMove(this int value, int pos)
        {
            //�ƶ� 0 λʱֱ�ӷ���ԭֵ
            if (pos != 0)
            {
                // int.MaxValue = 0x7FFFFFFF �������ֵ
                int mask = int.MaxValue;
                //�޷����������λ����ʾ�����������������з��ŵģ��з���������1λ������ʱ��λ��0������ʱ��λ��1
                value = value >> 1;
                //���������ֵ�����߼������㣬�����Ľ��Ϊ���Ա�ʾ����ֵ�����λ
                value = value & mask;
                //�߼�������ֵ�޷��ţ����޷��ŵ�ֱֵ�����������㣬����ʣ�µ�λ
                value = value >> pos - 1;
            }

            return value;
        }

    }
}
