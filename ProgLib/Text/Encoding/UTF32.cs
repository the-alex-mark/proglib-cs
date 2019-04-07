using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Text.Encoding
{
    public class UTF32
    {
        public static String Encode(String Text)
        {
            System.Text.Encoding UTF32 = System.Text.Encoding.UTF32;
            return Convert.ToBase64String(UTF32.GetBytes(Text));
        }

        public static String Decode(String Text)
        {
            System.Text.Encoding UTF32 = System.Text.Encoding.UTF32;
            return Convert.ToBase64String(UTF32.GetBytes(Text));
        }
    }
}
