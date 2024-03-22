using System;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Diagnostics;

namespace ElectronicCommerce.Areas.Admin.Helpers
{
    public class MailHelper
    {
        private IConfiguration configuration;
        public MailHelper(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        // Send without attachment
        public bool Send(string from, string to, string subject, string content)
        {
            try
            {
                var host = configuration["Gmail:Host"].ToString();
                var port = int.Parse(configuration["Gmail:Port"].ToString());
                var username = configuration["Gmail:Username"].ToString();
                var password = configuration["Gmail:Password"].ToString();
                var enable = bool.Parse(configuration["Gmail:SMTP:StartTLS:Enable"].ToString());

                var smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                };
                var mailMessage = new MailMessage(from, to, subject, content);
                mailMessage.Body = content;
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                smtpClient.UseDefaultCredentials = false;
                Debug.WriteLine(username + " " + password);
                smtpClient.Credentials = new NetworkCredential(username, password);
                smtpClient.Send(mailMessage);
                smtpClient.EnableSsl = true;

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        // Send with single attachment
        public bool Send(string from, string to, string subject, string content, string attachment)
        {
            try
            {
                var host = configuration["Gmail:Host"].ToString();
                var port = int.Parse(configuration["Gmail:Port"].ToString());
                var username = configuration["Gmail:Username"].ToString();
                var password = configuration["Gmail:Password"].ToString();
                var enable = bool.Parse(configuration["Gmail:SMTP:StartTLS:Enable"].ToString());
                var smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                    Credentials = new NetworkCredential(username, password)
                };
                var mailMessage = new MailMessage(from, to, subject, content);
                mailMessage.IsBodyHtml = true;
                if (attachment != null)
                {
                    mailMessage.Attachments.Add(new Attachment(attachment));
                }
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Send with multiple attachment
        public bool Send(string from, string to, string subject, string content, List<string> attachments)
        {
            try
            {
                var host = configuration["Gmail:Host"].ToString();
                var port = int.Parse(configuration["Gmail:Port"].ToString());
                var username = configuration["Gmail:Username"].ToString();
                var password = configuration["Gmail:Password"].ToString();
                var enable = bool.Parse(configuration["Gmail:SMTP:StartTLS:Enable"].ToString());
                var smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                    Credentials = new NetworkCredential(username, password)
                };
                var mailMessage = new MailMessage(from, to, subject, content);
                mailMessage.IsBodyHtml = true;
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (var attachment in attachments)
                    {
                        mailMessage.Attachments.Add(new Attachment(attachment));
                    }
                }
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
