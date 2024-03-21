
using Ionic.Zlib;
using System;
using System.IO;

namespace Zeng
{
    public class GZipUtils
    {
        //压缩字节
        //1.创建压缩的数据流 
        //2.设定compressStream为存放被压缩的文件流,并设定为压缩模式
        //3.将需要压缩的字节写到被压缩的文件流
        public static byte[] CompressBytes(byte[] bytes)
        {
            return Deflate(bytes);
            //using (MemoryStream compressStream = new MemoryStream())
            //{
            //    using (var zipStream = new GZipStream(compressStream, CompressionMode.Compress))
            //        zipStream.Write(bytes, 0, bytes.Length);
            //    return compressStream.ToArray();
            //}
        }
        //解压缩字节
        //1.创建被压缩的数据流
        //2.创建zipStream对象，并传入解压的文件流
        //3.创建目标流
        //4.zipStream拷贝到目标流
        //5.返回目标流输出字节
        public static byte[] Decompress(byte[] bytes)
        {
            return Inflate(bytes);
            //using (var compressStream = new MemoryStream(bytes))
            //{
            //    using (var zipStream = new GZipStream(compressStream, CompressionMode.Decompress))
            //    {
            //        using (var resultStream = new MemoryStream())
            //        {
            //            zipStream.CopyTo(resultStream);
            //            return resultStream.ToArray();
            //        }
            //    }
            //}
        }

        public static byte[] Inflate(byte[] data)
        {
            int outputSize = 1024;
            byte[] output = new Byte[outputSize];
            bool expectRfc1950Header = true;
            using (MemoryStream ms = new MemoryStream())
            {
                ZlibCodec compressor = new ZlibCodec();
                compressor.InitializeInflate(expectRfc1950Header);

                compressor.InputBuffer = data;
                compressor.AvailableBytesIn = data.Length;
                compressor.NextIn = 0;
                compressor.OutputBuffer = output;

                foreach (var f in new FlushType[] { FlushType.None, FlushType.Finish })
                {
                    int bytesToWrite = 0;
                    do
                    {
                        compressor.AvailableBytesOut = outputSize;
                        compressor.NextOut = 0;
                        compressor.Inflate(f);

                        bytesToWrite = outputSize - compressor.AvailableBytesOut;
                        if (bytesToWrite > 0)
                            ms.Write(output, 0, bytesToWrite);
                    }
                    while ((f == FlushType.None && (compressor.AvailableBytesIn != 0 || compressor.AvailableBytesOut == 0)) ||
                        (f == FlushType.Finish && bytesToWrite != 0));
                }

                compressor.EndInflate();

                return ms.ToArray();
            }
        }

        public static byte[] Deflate(byte[] data)
        {
            int outputSize = 1024;
            byte[] output = new Byte[outputSize];
            int lengthToCompress = data.Length;

            // If you want a ZLIB stream, set this to true.  If you want
            // a bare DEFLATE stream, set this to false.
            bool wantRfc1950Header = true;

            using (MemoryStream ms = new MemoryStream())
            {
                ZlibCodec compressor = new ZlibCodec();
                compressor.InitializeDeflate(Ionic.Zlib.CompressionLevel.BestCompression, wantRfc1950Header);

                compressor.InputBuffer = data;
                compressor.AvailableBytesIn = lengthToCompress;
                compressor.NextIn = 0;
                compressor.OutputBuffer = output;

                foreach (var f in new FlushType[] { FlushType.None, FlushType.Finish })
                {
                    int bytesToWrite = 0;
                    do
                    {
                        compressor.AvailableBytesOut = outputSize;
                        compressor.NextOut = 0;
                        compressor.Deflate(f);

                        bytesToWrite = outputSize - compressor.AvailableBytesOut;
                        if (bytesToWrite > 0)
                            ms.Write(output, 0, bytesToWrite);
                    }
                    while ((f == FlushType.None && (compressor.AvailableBytesIn != 0 || compressor.AvailableBytesOut == 0)) ||
                        (f == FlushType.Finish && bytesToWrite != 0));
                }

                compressor.EndDeflate();

                ms.Flush();
                return ms.ToArray();
            }
        }
    }
}
