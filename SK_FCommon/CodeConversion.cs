using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK_FCommon
{
    /// <summary>
    /// 原码、反码、补码转换类
    /// </summary>
    public class CodeConversion
    {
        /// <summary>
        /// 原码转补码
        /// </summary>
        /// <param name="code">原码的二进制字符串</param>
        public static string TrueCodeToComplement(string code)
        {
            try
            {
                if (code.Substring(0, 1) == "0")
                {
                    return code;
                }
                if (code.Substring(0, 1) == "1")
                {
                    int num;
                    int testLength;
                    int allLength;
                    allLength = code.Length;
                    int lastStringBinaryLength;
                    string lastStringBinary;
                    string S = "";
                    int[] Data = new int[1000];
                    string test = code.Substring(1, code.Length - 1);
                    testLength = test.Length;
                    for (int i = 0; i < test.Length; i++)
                    {
                        Data[i] = Convert.ToInt32(test.Substring(i, 1));
                        if (Data[i] == 0)
                        {
                            Data[i] = 1;
                        }
                        else
                        {
                            Data[i] = 0;
                        }
                        S = S + Convert.ToString(Data[i]);
                    }
                    num = Convert.ToInt32(S, 2);
                    num = num + 1;
                    lastStringBinary = Convert.ToString(num, 2);
                    lastStringBinaryLength = lastStringBinary.Length;
                    if (lastStringBinaryLength < testLength)
                    {
                        int Dvalue;
                        Dvalue = testLength - lastStringBinaryLength;
                        for (int i = 0; i < Dvalue; i++)
                        {
                            lastStringBinary = "0" + lastStringBinary;

                        }
                        code = "1" + lastStringBinary;
                    }
                    else if (lastStringBinaryLength > testLength)
                    {
                        lastStringBinary = "";
                        for (int i = 0; i < allLength; i++)
                        {
                            lastStringBinary = "0" + lastStringBinary;

                        }
                        code = lastStringBinary;
                    }
                    else
                    {
                        code = "1" + lastStringBinary;
                    }
                    return code;
                }
                return code;
            }
            catch (Exception ex)
            {
                throw new Exception("源码转换补码溢出，错误信息：" + ex.Message);
            }
        }
        private class CalculateCathe
        {
            public bool IsCarry = false;
            public char Result;
        }

        private CalculateCathe CalculateBitResult(char c1, char c2, bool carry)
        {
            CalculateCathe cathe = new CalculateCathe();

            int intResult = c1 - '0' + c2 - '0' + (carry ? 1 : 0);
            cathe.IsCarry = intResult > 1;
            cathe.Result = Convert.ToChar((intResult % 2) + '0');

            return cathe;
        }

        private bool IsCarry(char c1, char c2)
        {
            return (c1 == SECONT_CHAR_B) &&
                (c2 == SECONT_CHAR_B);
        }

        public string CalculateCodeResult(string data1st, string data2st)
        {
            StringBuilder result = new StringBuilder();
            CalculateCathe buffer = new CalculateCathe();

            for (int i = data1st.Length - 1; i >= 0; i--)
            {
                buffer = CalculateBitResult(data1st[i], data2st[i], buffer.IsCarry);
                result.Insert(0, buffer.Result);
            }

            return result.ToString();
        }

        private string CalculateTrueForm(int originalValue)
        {
            StringBuilder buffer = new StringBuilder();

            int quotient = 0;
            int remainder = 0;

            int tmp = Math.Abs(originalValue);

            do
            {
                quotient = tmp / 2;
                remainder = tmp % 2;

                buffer.Insert(0, Convert.ToString(remainder));

                tmp = quotient;
            } while (tmp != 0);

            string result = buffer.ToString().TrimStart(FIRST_CHAR_B).PadLeft(7, FIRST_CHAR_B);

            return (Convert.ToString(originalValue < 0 ? SECONT_CHAR_B : FIRST_CHAR_B)) + result;
        }

        private char POSITIVE_SIGN = '0';
        private char NEGATIVE_SIGN = '1';
        private char FIRST_CHAR_B = '0';
        private char SECONT_CHAR_B = '1';


        /// <summary>
        /// 原码转补码
        /// </summary>
        /// <param name="code">原码的二进制字符串</param>
        public string CalculateComplement(string dataF)
        {
            if (dataF[0] == POSITIVE_SIGN)
            {
                return dataF;
            }

            StringBuilder result = new StringBuilder();

            bool carry = dataF.Last() == SECONT_CHAR_B;
            result.Append(carry ? FIRST_CHAR_B : SECONT_CHAR_B);

            for (int i = dataF.Length - 2; i >= 0; i--)
            {
                if (carry)
                {
                    carry = dataF[i] == SECONT_CHAR_B;
                    result.Insert(0, carry ? FIRST_CHAR_B : SECONT_CHAR_B);

                    continue;
                }

                result.Insert(0, dataF[i]);
            }

            return result.ToString();
        }

        public string CalculateRadixMinusOneComplement(string dataY)
        {
            if (dataY[0] == POSITIVE_SIGN)
            {
                return dataY;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(dataY[0]);

            for (int i = 1; i < dataY.Length; i++)
            {
                sb.Append(dataY[i] == FIRST_CHAR_B ? SECONT_CHAR_B : FIRST_CHAR_B);
            }

            return sb.ToString();
        }
    }
}
