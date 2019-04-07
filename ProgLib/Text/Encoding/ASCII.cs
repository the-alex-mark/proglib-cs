using System;

namespace ProgLib.Text.Encoding
{
    public class ASCII
    {
        public static String Encode(String Text)
        {
            System.Text.Encoding ASCII = System.Text.Encoding.ASCII;
            return Convert.ToBase64String(ASCII.GetBytes(Text));
        }

        public static String Decode(String Text)
        {
            System.Text.Encoding ASCII = System.Text.Encoding.ASCII;
            return Convert.ToBase64String(ASCII.GetBytes(Text));
        }
    }
}
