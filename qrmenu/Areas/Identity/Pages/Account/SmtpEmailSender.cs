using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace qrmenu.Areas.Identity.Pages.Account
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
            var smtpHost = _configuration["Smtp:Host"];
            if (string.IsNullOrWhiteSpace(smtpHost))
                throw new ArgumentNullException(nameof(smtpHost), "SMTP host ayarı bulunamadı.");

            var smtpPortString = _configuration["Smtp:Port"];
            if (string.IsNullOrWhiteSpace(smtpPortString))
                throw new ArgumentNullException(nameof(smtpPortString), "SMTP port ayarı bulunamadı.");

            if (!int.TryParse(smtpPortString, out int smtpPort))
                throw new ArgumentException("SMTP port ayarı geçerli bir sayı değil.", nameof(smtpPortString));

            var smtpUser = _configuration["Smtp:User"];
            if (string.IsNullOrWhiteSpace(smtpUser))
                throw new ArgumentNullException(nameof(smtpUser), "SMTP kullanıcı adı bulunamadı.");

            var smtpPass = _configuration["Smtp:Pass"];
            if (string.IsNullOrWhiteSpace(smtpPass))
                throw new ArgumentNullException(nameof(smtpPass), "SMTP şifresi bulunamadı.");

    
            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpUser),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

           
            await client.SendMailAsync(mailMessage);
        }
    }
}
