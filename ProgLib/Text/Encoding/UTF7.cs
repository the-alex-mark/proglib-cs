using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Text.Encoding
{
    public class UTF7
    {
        public static String Encode(String Text)
        {
            System.Text.Encoding UTF7 = System.Text.Encoding.UTF7;
            return Convert.ToBase64String(UTF7.GetBytes(Text));
        }

        public static String Decode(String Text)
        {
            System.Text.Encoding UTF7 = System.Text.Encoding.UTF7;
            return Convert.ToBase64String(UTF7.GetBytes(Text));
        }
    }
}
