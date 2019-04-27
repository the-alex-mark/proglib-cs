using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un4seen
{
    internal sealed class PrivateImplementationDetails
    {
        internal static uint ComputeStringHash(string s)
        {
            uint num = new uint();
            if (s != null)
            {
                num = 0x811c9dc5;
                for (int i = 0; i < s.Length; i++)
                {
                    num = (s[i] ^ num) * 0x1000193;
                }
            }
            return num;
        }
    }
}

namespace radio42
{
    internal sealed class PrivateImplementationDetails
    {
        internal static uint ComputeStringHash(string s)
        {
            uint num = new uint();
            if (s != null)
            {
                num = 0x811c9dc5;
                for (int i = 0; i < s.Length; i++)
                {
                    num = (s[i] ^ num) * 0x1000193;
                }
            }
            return num;
        }
    }
}