using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SlackBot.App
{
    public class ChatMessageService
    {
        private const string Url = "https://slack.com/api/chat.postMessage";

        public async Task<HttpResponseMessage> Post(string token, string channel, string text)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "token", token },
                    { "channel", channel },
                    { "text", text }
                });

                return await client.PostAsync(Url, content);
            }
        }

        public string GetBotText(string user, string botName, string botMention)
        {
            return $"<@{user}>\n我こそは{botName}".Replace(botMention, "");
        }
    }
}
