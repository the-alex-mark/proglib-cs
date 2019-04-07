using System;
using System.Net.Mail;

namespace ProgLib.Network.Mail
{
    /// <summary>
    /// Представляет адрес получателя электронной почты
    /// </summary>
    public struct MailRecipient
    {
        /// <summary>
        /// Представляет адрес получателя электронной почты
        /// </summary>
        /// <param name="Address">Адрес электронной почты</param>
        public MailRecipient(String Address)
        {
            this.Mail = new MailAddress(Address);
        }

        public MailAddress Mail { get; set; }
    }
}
