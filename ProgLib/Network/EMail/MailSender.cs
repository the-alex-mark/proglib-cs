using System;
using System.Net.Mail;

namespace ProgLib.Network.EMail
{
    /// <summary>
    /// Представляет адрес отправителя электронной почты.
    /// </summary>
    public struct MailSender
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Address">Адрес электронной почты</param>
        /// <param name="Password">Пароль от учётной записи</param>
        public MailSender(MailAddress Address, String Password)
        {
            this.Mail = Address;
            this.Password = Password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Address">Адрес электронной почты</param>
        /// <param name="Password">Пароль от учётной записи</param>
        public MailSender(String Address, String Password)
        {
            this.Mail = new MailAddress(Address);
            this.Password = Password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Address">Адрес электронной почты</param>
        /// /// <param name="DisplayName">Отображаемое имя связанное с электронной почтой</param>
        /// <param name="Password">Пароль от учётной записи</param>
        public MailSender(String Address, String DisplayName, String Password)
        {
            this.Mail = new MailAddress(Address, DisplayName);
            this.Password = Password;
        }

        public MailAddress Mail { get; set; }
        public String Password { get; set; }
    }
}
