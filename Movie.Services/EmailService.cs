using Movie.Core.Model.MovieService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Movie.Services
{
    public class EmailService : IEmailService
    {
        private readonly AppConfiguration _appSettings;
        System.Net.Mail.SmtpClient smtpClient;
        MailAddress mailAddress;

        public EmailService(AppConfiguration appSettings)
        {
            _appSettings = appSettings;
            smtpClient = new SmtpClient(_appSettings.SmtpInfo.smtp, Convert.ToInt32(_appSettings.SmtpInfo.port)) { EnableSsl = false };
            mailAddress = new MailAddress(_appSettings.SmtpInfo.from);
        }

        public async Task<bool> Send(string body, string subject, IEnumerable<string> toList)
        {
            try
            {
                var msg = new MailMessage { From = mailAddress };

                if (toList != null && toList.Any())
                {
                    foreach (var item in toList)
                    {
                        if (!string.IsNullOrEmpty(item))
                            msg.To.Add(item);
                    }
                }

                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;
               smtpClient.Send(msg);
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
