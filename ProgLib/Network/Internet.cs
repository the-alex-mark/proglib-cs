using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Network
{
    public class Internet
    {
        /// <summary>
        /// Получает статус соединение с WEB страницей или сервисом.
        /// </summary>
        /// <param name="HostNameOrAddress"></param>
        /// <returns></returns>
        public static IPStatus Ping(String HostNameOrAddress)
        {
            return new Ping().Send(HostNameOrAddress).Status;
        }
    }
}
