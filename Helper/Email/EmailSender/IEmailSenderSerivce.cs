using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MimeKit;

namespace EmailSenderProject.Helper.Email.EmailSender
{
    public interface IEmailSenderSerivce
    {
        void SendEmail(EmailContent emailContent);
        Task SendEmailAsync(EmailContent emailContent);
    }
}