using System;
using System.Net;
using System.Net.Mail;

namespace ProgLib.Network.Mail
{
    public class Email
    {
        /// <summary>
        /// Отправляет письмо на указанную электронную почту.
        /// </summary>
        /// <param name="Sender">Данные отправителя</param>
        /// <param name="Recipient">Данные получателя</param>
        /// <param name="Theme">Тема</param>
        /// <param name="Message">Сообщение</param>
        public static void Message(MailSender Sender, MailRecipient Recipient, String Theme, String Message)
        {
            MailMessage _message = new MailMessage(Sender.Mail.Address, Recipient.Mail.Address)
            {
                Subject = Theme,
                Body = Message,
                IsBodyHtml = true
            };

            SmtpClient SMTP = new SmtpClient("smtp." + Sender.Mail.Address.Substring(Sender.Mail.Address.LastIndexOf('@') + 1), 587)
            {
                Credentials = new NetworkCredential(Sender.Mail.Address, Sender.Password),
                EnableSsl = true,
            };
            SMTP.Send(_message);
        }

        /// <summary>
        /// Отправляет письмо на указанную электронную почту.
        /// </summary>
        /// <param name="Sender">Данные отправителя</param>
        /// <param name="Recipient">Данные получателя</param>
        /// <param name="Theme">Тема</param>
        /// <param name="Message">Сообщение</param>
        /// <param name="Files">Файлы</param>
        public static void Message(MailSender Sender, MailRecipient Recipient, String Theme, String Message, params String[] Files)
        {
            MailMessage _message = new MailMessage(Sender.Mail.Address, Recipient.Mail.Address)
            {
                Subject = Theme,
                Body = Message,
                IsBodyHtml = true,
            };

            if (Files.Length > 0)
            {
                foreach (String File in Files)
                    _message.Attachments.Add(new Attachment(File));
            }

            SmtpClient SMTP = new SmtpClient("smtp." + Sender.Mail.Address.Substring(Sender.Mail.Address.LastIndexOf('@') + 1), 587)
            {
                Credentials = new NetworkCredential(Sender.Mail.Address, Sender.Password),
                EnableSsl = true,
            };
            SMTP.Send(_message);
        }
    }
}
