using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.DataProtection;

namespace JackSite.Identity.Server.Services
{
    public interface IDataProtectionService
    {
        string Protect(string plainText);
        string Unprotect(string protectedText);
    }
    
    public class DataProtectionService : IDataProtectionService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IDataProtector _protector;
        
        public DataProtectionService(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _protector = _dataProtectionProvider.CreateProtector("JackSite.Identity.Server.MfaProtection");
        }
        
        public string Protect(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return plainText;
            }
            
            return _protector.Protect(plainText);
        }
        
        public string Unprotect(string protectedText)
        {
            if (string.IsNullOrEmpty(protectedText))
            {
                return protectedText;
            }
            
            return _protector.Unprotect(protectedText);
        }
    }
}