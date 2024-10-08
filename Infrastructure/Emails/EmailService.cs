using ClothingBrand.Application.Contract;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
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
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task<bool> SendEmailAsync(string mailTo = null, string subject = null, string body = null, IList<IFormFile> attachments = null)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject
            };

            email.To.Add(MailboxAddress.Parse(mailTo));

            var builder = new BodyBuilder();

            if (attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));


            try
            {
                

                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
                await smtp.SendAsync(email);

                smtp.Disconnect(true);

            }
            catch (Exception ex)
            {
               var s= ex.Message;
            }




            return true;




        }





        
    }
}



//using var smtp = new SmtpClient()
//{

//};
//smtp.AuthenticationMechanisms.Remove("XOAUTH2");
//smtp.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);

////var oauth2 = new SaslMechanismOAuth2("Ahmed.m.Shaban@outlook.com", "*88888888");
////smtp.Authenticate(oauth2);

////  smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

//// smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
// smtp.Authenticate("Ahmed.m.Shaban@outlook.com", "********");

// smtp.Send(email);

//smtp.Disconnect(true);
//return true;

//using (var client = new SmtpClient())
//{
//    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

//    var oauth2 = new SaslMechanismOAuth2("axcvz185@gmail.com", "aassdd456");
//    client.Authenticate(oauth2);

//    client.Send(email);
//    client.Disconnect(true);
//}
//return true;












//var smtpclient = new SmtpClient("smtp.gmail.com");

//smtpclient.Port = 587;
//smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
//smtpclient.UseDefaultCredentials = false;
//smtpclient.Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password);
//smtpclient.EnableSsl = true;




//var mailmassege = new MailMessage
//{
//    From = new MailAddress(_mailSettings.Email),
//    Subject = "Ahmed shaban",
//    Body = "gsdfgfdg",
//    IsBodyHtml = true,
//};


//mailmassege.To.Add("ahmedshapan183@gmail.com");

//await smtpclient.SendMailAsync(mailmassege);