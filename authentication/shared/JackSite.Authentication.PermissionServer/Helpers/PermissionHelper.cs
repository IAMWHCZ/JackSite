using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JackSite.Authentication.PermissionServer.Helpers
{
    public class PermissionHelper
    {
        private readonly string _authorizeApi;

        public PermissionHelper()
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            var json = File.ReadAllText(configPath);
            var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            _authorizeApi = config["AuthorizeAPIURL"];
        }

        public async Task<List<PermissionsDataSource>> GetPermissionsDataSourcesByIdAsync(
            List<PermissionResultBase> bases)
        {
            try
            {
                var json = JsonConvert.SerializeObject(bases);
                var content =
                    new StringContent(json, Encoding.UTF8, "application/json"); // Convert RequestParam to HttpContent
                var response =
                    await HttpClientHelper.HttpClient.PostAsync(
                        $"{_authorizeApi}/authorization/GetPermissionsDataSources", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<PermissionsDataSource>>(jsonString);
                    return result;
                }
                else
                {
                    throw new HttpRequestException("Failed to retrieve permissions from API.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<APIPermissionResult> NewCheckPermissionAsync(RequestParam requestParam)
        {
            try
            {
                var json = JsonConvert.SerializeObject(requestParam);
                var content =
                    new StringContent(json, Encoding.UTF8, "application/json"); // Convert RequestParam to HttpContent
                var response =
                    await HttpClientHelp.HttpClient.PostAsync($"{_authorizeApi}/authorization/NewCheckPermission",
                        content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonstr = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<APIPermissionResult>(jsonstr);
                    return result;
                }
                else
                {
                    throw new HttpRequestException("Failed to retrieve permissions from API.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}