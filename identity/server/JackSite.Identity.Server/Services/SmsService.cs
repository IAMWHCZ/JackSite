using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace JackSite.Identity.Server.Services
{
    public interface ISmsService
    {
        Task<bool> SendSmsAsync(string phoneNumber, string message);
    }
    
    public class SmsService : ISmsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmsService> _logger;
        
        public SmsService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<SmsService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            
            // 配置 HttpClient
            var smsApiUrl = _configuration["Sms:ApiUrl"];
            if (!string.IsNullOrEmpty(smsApiUrl))
            {
                _httpClient.BaseAddress = new Uri(smsApiUrl);
            }
            
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            var apiKey = _configuration["Sms:ApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
            }
        }
        
        public async Task<bool> SendSmsAsync(string phoneNumber, string message)
        {
            try
            {
                var provider = _configuration["Sms:Provider"]?.ToLower();
                
                switch (provider)
                {
                    case "twilio":
                        return await SendTwilioSmsAsync(phoneNumber, message);
                    case "aliyun":
                        return await SendAliyunSmsAsync(phoneNumber, message);
                    case "custom":
                        return await SendCustomSmsAsync(phoneNumber, message);
                    default:
                        _logger.LogWarning("Unknown SMS provider: {Provider}", provider);
                        return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS to {PhoneNumber}", phoneNumber);
                return false;
            }
        }
        
        private async Task<bool> SendTwilioSmsAsync(string phoneNumber, string message)
        {
            var accountSid = _configuration["Sms:Twilio:AccountSid"];
            var authToken = _configuration["Sms:Twilio:AuthToken"];
            var fromNumber = _configuration["Sms:Twilio:FromNumber"];
            
            var requestContent = new
            {
                To = phoneNumber,
                From = fromNumber,
                Body = message
            };
            
            var content = new StringContent(
                JsonSerializer.Serialize(requestContent),
                Encoding.UTF8,
                "application/json");
                
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.twilio.com/2010-04-01/Accounts/{accountSid}/Messages.json");
            request.Content = content;
            
            // 添加 Basic 认证
            var authenticationString = $"{accountSid}:{authToken}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        
        private async Task<bool> SendAliyunSmsAsync(string phoneNumber, string message)
        {
            var accessKeyId = _configuration["Sms:Aliyun:AccessKeyId"];
            var accessKeySecret = _configuration["Sms:Aliyun:AccessKeySecret"];
            var signName = _configuration["Sms:Aliyun:SignName"];
            var templateCode = _configuration["Sms:Aliyun:TemplateCode"];
            
            // 提取验证码
            var code = ExtractVerificationCode(message);
            
            var requestContent = new
            {
                PhoneNumbers = phoneNumber,
                SignName = signName,
                TemplateCode = templateCode,
                TemplateParam = JsonSerializer.Serialize(new { code })
            };
            
            var content = new StringContent(
                JsonSerializer.Serialize(requestContent),
                Encoding.UTF8,
                "application/json");
                
            var response = await _httpClient.PostAsync("", content);
            return response.IsSuccessStatusCode;
        }
        
        private async Task<bool> SendCustomSmsAsync(string phoneNumber, string message)
        {
            var requestContent = new
            {
                phoneNumber,
                message
            };
            
            var content = new StringContent(
                JsonSerializer.Serialize(requestContent),
                Encoding.UTF8,
                "application/json");
                
            var response = await _httpClient.PostAsync("", content);
            return response.IsSuccessStatusCode;
        }
        
        private string ExtractVerificationCode(string message)
        {
            // 简单提取验证码，假设验证码是消息中的数字
            var code = string.Empty;
            foreach (var c in message)
            {
                if (char.IsDigit(c))
                {
                    code += c;
                }
            }
            
            return code;
        }
    }
}