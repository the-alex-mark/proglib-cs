using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Text.Encoding
{
    public class UTF8
    {
        public static String Encode(String Value)
        {
            Byte[] buffer = System.Text.Encoding.Convert(
                System.Text.Encoding.GetEncoding("Windows-1251"),
                System.Text.Encoding.GetEncoding("UTF-8"),
                System.Text.Encoding.GetEncoding("UTF-8").GetBytes(Value));

            return System.Text.Encoding.GetEncoding("UTF-8").GetString(buffer);
        }

        public static String Decode(String Value)
        {
            Byte[] bytes = System.Text.Encoding.GetEncoding("Windows-1251").GetBytes(Value);
            Byte[] bytes2 = System.Text.Encoding.Convert(System.Text.Encoding.GetEncoding("UTF-8"), System.Text.Encoding.GetEncoding("Windows-1251"), bytes);

            return System.Text.Encoding.GetEncoding("Windows-1251").GetString(bytes2);
        }
    }
}
