using Hexa_Hub.Interface;
using System.Net.Mail;
using System.Net;
using Hexa_Hub.Models;
using Microsoft.Extensions.Options;
namespace Hexa_Hub.Repository
{
    public class EmailService : IEmail
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string fromEmail, string fromName, string toEmail, string subject, string message)
        {
            var smtpClient = new SmtpClient(_configuration["EmailSettings:SMTPServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:Port"]),
                Credentials = new NetworkCredential(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),  // Use dynamic sender
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }


}

