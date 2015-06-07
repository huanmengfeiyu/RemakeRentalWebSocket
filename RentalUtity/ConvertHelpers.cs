using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RentalUtity
{
    public class ConvertHelpers
    {
        /// <summary>
        /// 将int转换成BYTE[2]
        /// </summary>
        public static byte[] IntToByteTwoByHignFirst(int number)
        {
            byte[] resultByte = new byte[2];
            resultByte[0] = (byte)((0xff00 & number) >> 8);
            resultByte[1] = (byte)(0xff & number);
            return resultByte;
        }

        /// <summary>
        /// 将int转换成BYTE[2]
        /// </summary>
        public static byte[] IntToByteTwoByLowFirst(int number)
        {
            byte[] resultByte = new byte[2];
            resultByte[0] = (byte)(0xff & number);
            resultByte[1] = (byte)((0xff00 & number) >> 8);
            return resultByte;
        }

        /// <summary>
        /// 将int数值转换为占四个字节的byte数组，本方法适用于(低位在前，高位在后)的顺序。 和bytesToInt（）配套使用
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] intToBytes(int value)
        {
            byte[] src = new byte[4];
            src[3] = (byte)((value >> 24) & 0xFF);
            src[2] = (byte)((value >> 16) & 0xFF);
            src[1] = (byte)((value >> 8) & 0xFF);
            src[0] = (byte)(value & 0xFF);
            return src;
        }

        /// <summary>
        /// 将int数值转换为占四个字节的byte数组，本方法适用于(高位在前，低位在后)的顺序。  和bytesToInt2（）配套使用
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] intToBytes2(uint value)
        {
            byte[] src = new byte[4];
            src[0] = (byte)((value >> 24) & 0xFF);
            src[1] = (byte)((value >> 16) & 0xFF);
            src[2] = (byte)((value >> 8) & 0xFF);
            src[3] = (byte)(value & 0xFF);
            return src;
        }
        /// <summary>
        /// 高位在前，8字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] intToBytes2(ulong value)
        {
            byte[] src = new byte[8];
            src[0] = (byte)((value >> 56) & 0xFF);
            src[1] = (byte)((value >> 48) & 0xFF);
            src[2] = (byte)((value >> 40) & 0xFF);
            src[3] = (byte)((value >> 32) & 0xFF);
            src[4] = (byte)((value >> 24) & 0xFF);
            src[5] = (byte)((value >> 16) & 0xFF);
            src[6] = (byte)((value >> 8) & 0xFF);
            src[7] = (byte)(value & 0xFF);
            return src;
        }
        /// <summary>
        /// byte数组中取int数值，本方法适用于(低位在前，高位在后)的顺序，和和intToBytes（）配套使用
        /// </summary>
        /// <param name="src"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int bytesToInt(byte[] src, int offset)
        {
            int value;
            value = (int)((src[offset] & 0xFF)
                    | ((src[offset + 1] & 0xFF) << 8)
                    | ((src[offset + 2] & 0xFF) << 16)
                    | ((src[offset + 3] & 0xFF) << 24));
            return value;
        }

        public static int bytesToInt(byte[] src)
        {
            int value;
            value = (int)((src[0] & 0xFF)
                    | ((src[1] & 0xFF) << 8));
            return value;
        }

        /// <summary>
        /// byte数组中取int数值，本方法适用于(低位在后，高位在前)的顺序。和intToBytes2（）配套使用
        /// </summary>
        /// <param name="src"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int bytesToInt2(byte[] src, int offset)
        {
            int value;
            value = (int)(((src[offset] & 0xFF) << 24)
                    | ((src[offset + 1] & 0xFF) << 16)
                    | ((src[offset + 2] & 0xFF) << 8)
                    | (src[offset + 3] & 0xFF));
            return value;
        }

        /// <summary>
        /// 16进制字符串转字节数组
        /// </summary>
        public static byte[] hexStrToByte(string hexString)
        {
            hexString = hexString.Replace("0x", "");
            if ((hexString.Length % 2) != 0)
                hexString = "0" + hexString;
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        public static byte[] IntPtrToBytes(IntPtr value, int length)
        {
            byte[] src = new byte[length];
            Marshal.Copy(value, src, 0, length);
            return src;
        }

        public static T[] ParseObjectToArray<T>(object ambiguousObject)
        {
            var json = ambiguousObject.ToString();
            if (String.IsNullOrWhiteSpace(json))
            {
                return new T[0];
            }
            else if (json.TrimStart().StartsWith("["))
            {
                return JsonConvert.DeserializeObject<T[]>(json);
            }
            else
            {
                return new T[1] { JsonConvert.DeserializeObject<T>(json) };
            }
        }
    }
}