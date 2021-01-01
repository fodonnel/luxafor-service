using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LuxaforService
{
    public class LuxaforClient
    {
        private readonly HttpClient _client;
        private readonly string _key;

        public LuxaforClient(HttpClient client)
        {
            _client = client;
            _key = Environment.GetEnvironmentVariable("LuxaforKey");
        }

        public async Task SetBusyStatus(bool isBusy)
        {
            if (isBusy)
            {
                await _client.PostAsJsonAsync("webhook/v1/actions/solid_color", new
                {
                    userId = _key,
                    actionFields = new { color = "red" }
                });
            }
            else
            {
                await _client.PostAsJsonAsync("webhook/v1/actions/solid_color", new
                {
                    userId = _key,
                    actionFields = new { color = "custom", custom_color = "000000" }
                });
            }
        }
    }
}
