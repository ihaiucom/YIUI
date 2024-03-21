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
            //申请一块非托管内存  
            IntPtr ptr = Marshal.AllocHGlobal(intSize);
            //复制int数组到该内存块  
            Marshal.Copy(intArr, 0, ptr, intArr.Length);
            //复制回byte数组  
            Marshal.Copy(ptr, bytArr, 0, bytArr.Length);
            //释放申请的非托管内存  
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
        * 将int数值转换为占四个字节的byte数组，本方法适用于(低位在前，高位在后)的顺序。  
        * @param value  
        *            要转换的int值 
        * @return byte数组 
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
        /// 无符号右移, 相当于java里的 value>>>pos
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static int RightMove(this int value, int pos)
        {
            //移动 0 位时直接返回原值
            if (pos != 0)
            {
                // int.MaxValue = 0x7FFFFFFF 整数最大值
                int mask = int.MaxValue;
                //无符号整数最高位不表示正负但操作数还是有符号的，有符号数右移1位，正数时高位补0，负数时高位补1
                value = value >> 1;
                //和整数最大值进行逻辑与运算，运算后的结果为忽略表示正负值的最高位
                value = value & mask;
                //逻辑运算后的值无符号，对无符号的值直接做右移运算，计算剩下的位
                value = value >> pos - 1;
            }

            return value;
        }

    }
}
