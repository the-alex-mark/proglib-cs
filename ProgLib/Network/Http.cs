using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Network
{
    public class Http
    {
        /// <summary>
        /// Получает весь контент WEB страницы.
        /// </summary>
        /// <param name="HostNameOrAddress"></param>
        /// <returns></returns>
        public static string GetContents(String HostNameOrAddress)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(HostNameOrAddress);
            httpWebRequest.Method = "GET";
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
            httpWebRequest.ProtocolVersion = HttpVersion.Version11;
            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.CookieContainer = new CookieContainer();
            httpWebRequest.Method = "GET";
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            return new StreamReader(httpWebResponse.GetResponseStream(), Encoding.Default).ReadToEnd();
        }

        /// <summary>
        /// Проверяет есть ли соединение с WEB страницей или сервисом.
        /// </summary>
        /// <param name="HostNameOrAddress"></param>
        /// <returns></returns>
        public static Boolean Connection(String HostNameOrAddress)
        {
            IPStatus Status = IPStatus.Unknown;
            try
            {
                Status = new Ping().Send(HostNameOrAddress).Status;
            }
            catch { }

            return (Status == IPStatus.Success) ? true : false;
        }

        /// <summary>
        /// Получает статус соединение с WEB страницей или сервисом.
        /// </summary>
        /// <param name="HostNameOrAddress"></param>
        /// <returns></returns>
        public static IPStatus Status(String HostNameOrAddress)
        {
            return new Ping().Send(HostNameOrAddress).Status;
        }
    }
}
