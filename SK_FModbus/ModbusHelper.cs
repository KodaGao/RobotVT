using System;

namespace SK_FModbus
{
    public class ModbusHelper
    {
        /// <summary>
        /// 将Float字节数组转换成Modbus字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] FloatToModbus(byte[] value)
        {
            byte[] _Result = new byte[4];
            if (value.Length == _Result.Length)
                _Result = new byte[] { value[1], value[0], value[3], value[2] };
            return _Result;
        }

        /// <summary>
        /// 将Short字节数组转换成Modbus字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ShortToModbus(byte[] value)
        {
            byte[] _Result = new byte[2];
            if (value.Length == _Result.Length)
                _Result = new byte[] { value[1], value[0] };
            return _Result;
        }

        /// <summary>
        /// 将Short字节数组转换成Modbus字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Int32ToModbus(byte[] value)
        {
            byte[] _Result = new byte[4];
            if (value.Length == _Result.Length)
                _Result = new byte[] { value[3],value[2], value[1], value[0] };
            return _Result;
        }

        /// <summary>
        /// 将Short字节数组转换成Modbus字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Int16ToModbus(byte[] value)
        {
            byte[] _Result = new byte[2];
            if (value.Length == _Result.Length)
                _Result = new byte[] { value[1], value[0] };
            return _Result;
        }

        /// <summary>
        /// 将Modbus字节数组转换成Float类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static float ModbusToFloat(byte[] value, int digits)
        {
            float _Result;
            byte[] _Value;
            if (value.Length == 4)
            {
                _Value = new byte[] { value[1], value[0], value[3], value[2] };
                _Result = (float)Math.Round(BitConverter.ToSingle(_Value, 0), digits);
            }
            else
                _Result = -1;
            return _Result;
        }

        /// <summary>
        /// 将Modbus字节数组转换成Int16类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int16 ModbusToTnt16(byte[] value)
        {
            Int16 _Result;
            if (value.Length == 2)
                _Result = BitConverter.ToInt16(new byte[] { value[1], value[0] }, 0);
            else
                _Result = -1;
            return _Result;
        }

        /// <summary>
        /// 将Modbus字节数组转换成Int16类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 ModbusToTnt32(byte[] value)
        {
            Int32 _Result;
            if (value.Length == 4)
                _Result = BitConverter.ToInt32(new byte[] { value[1], value[0], value[3], value[2] }, 0);
            else
                _Result = -1;
            return _Result;
        }
    }
}