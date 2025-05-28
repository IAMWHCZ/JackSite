using System.Collections.Generic;
using System.Linq;
using JackSite.Authentication.PermissionServer.Models;
using Newtonsoft.Json.Linq;

namespace JackSite.Authentication.PermissionServer.Helpers
{
    public static class DataHelper
    {
        public static JToken GetDataResource(IDictionary<string, object> properties, string dataResourceName)
        {
            if (!properties.TryGetValue("Permissions", out var permissions)) return null;

            var resourcePermissions = permissions as List<ResourcePermission>;
            if (resourcePermissions != null)
            {
                var dataResources = resourcePermissions.Select(x => x.DataResource).ToList();
            }

            return (from permission in resourcePermissions
                select permission.DataResource
                into dataResource
                where !string.IsNullOrEmpty(dataResource)
                select JObject.Parse(dataResource)
                into jsonObj
                select jsonObj[dataResourceName]).FirstOrDefault(jsonData => jsonData != null);
        }
    }
}