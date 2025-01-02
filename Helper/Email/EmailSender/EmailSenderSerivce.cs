using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.Extensions.Options;

using Microsoft.Extensions.DependencyInjection;


namespace EmailSenderProject.Helper.Email.EmailSender
{
    public class EmailSenderSerivce : IEmailSenderSerivce
    {
        private readonly ILogger Logger;
        public EmailSenderSerivce(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<EmailSenderSerivce>(); ;
        }

        public void SendEmail(EmailContent emailContent)
        {
            var emailMessage = CreateEmailMessage(emailContent);

            Send(emailMessage);
        }


        public async Task SendEmailAsync(EmailContent emailContent)
        {
            try
            {
                var mailMessage = CreateEmailMessage(emailContent);
                await SendAsync(mailMessage);
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        private MimeMessage CreateEmailMessage(EmailContent emailContent)
        {
            var emailMessage = new MimeMessage();
            
            emailMessage.From.Add(new MailboxAddress("Ministry Of Social Solidarity", "<From Email>"));
            emailMessage.To.AddRange(emailContent.To);
            emailMessage.Subject = emailContent.Subject;
            
            var bodyBuilder = new BodyBuilder { HtmlBody = emailContent.Content};

            if (emailContent.Attachments != null && emailContent.Attachments.Any())
            {
                foreach (var attach in emailContent.Attachments)
                {
                    bodyBuilder.Attachments.Add(attach);

                }
            }

            if(emailContent.EmbededImageAttachements != null && emailContent.EmbededImageAttachements.Any())
            {
                foreach (var attach in emailContent.EmbededImageAttachements)
                {
                    bodyBuilder.LinkedResources.Add(attach);
                }
            }
            emailMessage.Bcc.AddRange(emailContent.Cc.ToList());

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    //Replace 9999 with local port
                    client.Connect( "<SmtpServer>",  9999, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate( "<UserName>",  "<Password>");

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    try
                    {

                        client.CheckCertificateRevocation = false;
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;
#if Publish
                        await client.ConnectAsync(_appConfigurationService.SmtpServer, _appConfigurationService.PortPublished, false);
#else

                        //Replace 9999 with PortLocal 
                        await client.ConnectAsync("<SmtpServer>", 9999, MailKit.Security.SecureSocketOptions.StartTls);
#endif

                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        await client.AuthenticateAsync("<UserName>", "<Password>");

                        await client.SendAsync(mailMessage);
                        Logger.LogInformation($"Email Send To {string.Join(',', mailMessage.To)}");
                    }
                    catch (Exception exp)
                    {
                        Logger.LogError($"Error While Send Emial To {string.Join(',', mailMessage.To)}, Err: {exp.Message}");
                        throw;
                    }
                    finally
                    {
                        await client.DisconnectAsync(true);
                        client.Dispose();
                    }
                }
            }
            catch (Exception exp)
            {
                throw;
            }

        }
    }
}
