using Newtonsoft.Json;
using SlackBot.UI.Dto.Event;

namespace SlackBot.UI.Dto
{
    public class EventDto
    {
        public string Token { get; set; }

        [JsonProperty("team_id")]
        public string TeamId { get; set; }

        [JsonProperty("api_app_id")]
        public string ApiAppId { get; set; }

        public MessageDto Event { get; set; }

        public string Type { get; set; }

        [JsonProperty("authed_users")]
        public string[] AuthedUsers { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("event_time")]
        public int EventTime { get; set; }
    }
}
