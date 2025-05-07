using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JackSite.Identity.Server.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false);
    }
}