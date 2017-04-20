using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SlackBot.UI.Extensions;

namespace SlackBot.UI.Service
{
    public class AiService
    {
        private const string Url = "https://chatbot-api.userlocal.jp/api/chat";
        private readonly string _token;

        public AiService(string token)
        {
            _token = token;
        }

        public async Task<T> Get<T>(string message)
        {
            using (var client = new HttpClient())
            {
                var buildUrl = $"{Url}?key={_token}&message={WebUtility.UrlEncode(message)}";
                var response = await client.GetAsync(buildUrl);
                return await response.Content.ReadAsJsonAsync<T>();
            }
        }
    }
}
