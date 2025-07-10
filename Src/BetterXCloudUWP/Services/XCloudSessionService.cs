using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BetterXCloudUWP.Services
{
    public class XCloudSessionService
    {
        public async Task<(string streamUrl, string licenseUrl, string token)> GetStreamSessionAsync(
            string xstsToken, string gameId = null, string region = null)
        {
            var playEndpoint = SettingsManager.Instance.Get<string>("XCloudEndpoint", 
                "https://xbox-cloud-gaming-endpoint/sessions/cloud/play");
            if (string.IsNullOrEmpty(region))
                region = SettingsManager.Instance.Get<string>("XCloudDefaultRegion", "eastus");
            if (string.IsNullOrEmpty(gameId))
                gameId = SettingsManager.Instance.Get<string>("XCloudDefaultTitleId", "");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"XBL3.0 x={xstsToken}");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                // Добавить другие необходимые заголовки

                var body = new JObject
                {
                    ["titleId"] = gameId,
                    ["preferredRegion"] = region,
                    ["settings"] = new JObject
                    {
                        ["osName"] = "windows10",
                        ["locale"] = "ru-RU"
                    }
                };
                var response = await client.PostAsync(playEndpoint, new StringContent(body.ToString()));
                response.EnsureSuccessStatusCode();
                var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                string streamUrl = json["streamUrl"]?.ToString();
                string licenseUrl = json["licenseUrl"]?.ToString();
                string token = json["token"]?.ToString();
                return (streamUrl, licenseUrl, token);
            }
        }
    }
}
