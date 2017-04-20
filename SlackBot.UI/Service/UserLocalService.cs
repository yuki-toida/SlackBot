using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SlackBot.UI.Extensions;
using SlackBot.UI.Dto;

namespace SlackBot.UI.Service
{
    public class UserLocalService
    {
        private const string Url = "https://chatbot-api.userlocal.jp/api/chat";
        private readonly string _token;

        public UserLocalService(string token)
        {
            _token = token;
        }

        public async Task<UserLocalDto> Get(string message)
        {
            using (var client = new HttpClient())
            {
                var buildUrl = $"{Url}?key={_token}&message={WebUtility.UrlEncode(message)}";
                var response = await client.GetAsync(buildUrl);
                return await response.Content.ReadAsJsonAsync<UserLocalDto>();
            }
        }
    }
}
