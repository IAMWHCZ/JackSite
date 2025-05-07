using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JackSite.Identity.Server.Interfaces
{
     public interface ISmsService
    {
        Task<bool> SendSmsAsync(string phoneNumber, string message);
    }
}