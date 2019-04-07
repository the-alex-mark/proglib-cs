using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProgLib.Network
{
    public class IP
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        private static extern Int32 SendARP(Int32 DestinationIP, Int32 SourceIP, [Out] Byte[] pMacAddr, ref Int32 PhyAddrLen);

        /// <summary>
        /// Получает IPV4 адрес.
        /// </summary>
        /// <returns></returns>
        public static String IPV4()
        {
            return Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
        }

        /// <summary>
        /// Получает IPV6 адрес.
        /// </summary>
        /// <returns></returns>
        public static String IPV6()
        {
            var locationResponse = new WebClient().DownloadString("https://freegeoip.net/xml/");
            var responseXml = XDocument.Parse(locationResponse).Element("Response");

            return responseXml.Element("IP").Value;
        }

        /// <summary>
        /// Получает MAC адрес.
        /// </summary>
        /// <returns></returns>
        public static String MAC()
        {
            IPAddress ip = Dns.GetHostByName(Dns.GetHostName()).AddressList[0];

            byte[] addr = new byte[6];
            int length = addr.Length;

            SendARP(ip.GetHashCode(), 0, addr, ref length);

            return BitConverter.ToString(addr, 0, 6);
        }
    }
}
