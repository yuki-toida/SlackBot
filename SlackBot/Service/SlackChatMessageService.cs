using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SlackBot.Service
{
    public class SlackChatMessageService
    {
        private const string Url = "https://slack.com/api/chat.postMessage";
        private readonly string _token;

        public SlackChatMessageService(string token)
        {
            _token = token;
        }

        public async Task<HttpResponseMessage> Post(string channel, string text)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "token", _token },
                    { "channel", channel },
                    { "text", text },
                    { "icon_url", "http://freeiconbox.com/icon/256/34429.png" }
                });

                return await client.PostAsync(Url, content);
            }
        }

        public string GetBotText(string user, string botMention, string text)
        {
            //return $"<@{user}>".Replace(botMention, "");
            return $"{text}".Replace(botMention, "");
        }
    }
}
