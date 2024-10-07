using ClothingBrand.Application.Contract;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Infrastructure.Emails
{
    public class EmailService : IEmailService
    {
        //private readonly MailSettings _mailSettings;

        //public EmailService(IOptions<MailSettings> mailSettings)
        //{
        //    _mailSettings = mailSettings.Value;
        //}
        //public async Task<bool> SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null)
        //{
        //    //var email = new MimeMessage
        //    //{
        //    //    Sender = MailboxAddress.Parse("axcvz185@gmail.com"),
        //    //    Subject = subject
        //    //};

        //    //email.To.Add(MailboxAddress.Parse(mailTo));

        //    //var builder = new BodyBuilder();

        //    //if (attachments != null)
        //    //{
        //    //    byte[] fileBytes;
        //    //    foreach (var file in attachments)
        //    //    {
        //    //        if (file.Length > 0)
        //    //        {
        //    //            using var ms = new MemoryStream();
        //    //            file.CopyTo(ms);
        //    //            fileBytes = ms.ToArray();

        //    //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
        //    //        }
        //    //    }
        //    //}

        //    //builder.HtmlBody = body;
        //    //email.Body = builder.ToMessageBody();
        //    //email.From.Add(new MailboxAddress(_mailSettings.DisplayName, "axcvz185@gmail.com"));

        //    //using var smtp = new SmtpClient()
        //    //{

        //    //};
        //    //smtp.AuthenticationMechanisms.Remove("XOAUTH2");
        //    //smtp.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);

        //    ////var oauth2 = new SaslMechanismOAuth2("Ahmed.m.Shaban@outlook.com", "*88888888");
        //    ////smtp.Authenticate(oauth2);

        //    ////  smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

        //    //// smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
        //    // smtp.Authenticate("Ahmed.m.Shaban@outlook.com", "********");

        //    // smtp.Send(email);

        //    //smtp.Disconnect(true);
        //    //return true;

        //    //using (var client = new SmtpClient())
        //    //{
        //    //    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

        //    //    var oauth2 = new SaslMechanismOAuth2("axcvz185@gmail.com", "aassdd456");
        //    //    client.Authenticate(oauth2);

        //    //    client.Send(email);
        //    //    client.Disconnect(true);
        //    //}
        //    //return true;








        //    //var smtpclient = new SmtpClient("smtp.gmail.com")
        //    //{
        //    //    Port = 587,
        //    //    Credentials = new NetworkCredential("aaxcvz185@gmail.com", "aassdd456"),
        //    //    EnableSsl = true,
        //    //    DeliveryMethod = SmtpDeliveryMethod.Network,
        //    //    UseDefaultCredentials = false
        //    //};


        //    //var mailmassege = new MailMessage
        //    //{
        //    //    From = new MailAddress("aaxcvz185@gmail.com"),
        //    //    Subject = subject,
        //    //    Body = body,
        //    //    IsBodyHtml = true,
        //    //};


        //    //mailmassege.To.Add(mailTo);

        //    //await smtpclient.SendMailAsync(mailmassege);
        //    //return true;




        //}





        private readonly MailSettings _emailSettings;

        public EmailService(IOptions<MailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = message };

            using var smtp = new SmtpClient();
            try
            {
                smtp.Connect(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);
                await smtp.SendAsync(email);
            }
            finally
            {
                smtp.Disconnect(true);
            }
        }
    }
}
