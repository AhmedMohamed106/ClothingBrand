using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Contract
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string mailTo = null, string subject = null, string body = null, IList<IFormFile> attachments = null);
       // Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
