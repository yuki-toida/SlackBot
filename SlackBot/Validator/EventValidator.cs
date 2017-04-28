using SlackBot.Dto.Slack;
using SlackBot.Settings;

namespace SlackBot.Validator
{
    public class EventValidator
    {
        private readonly SlackOptions _options;

        public EventValidator(SlackOptions optios)
        {
            _options = optios;
        }

        /// <summary>
        /// EventAPIのリクエストデータを検証します
        /// </summary>
        public bool Validate(EventDto dto)
        {
            if (dto == null)
                return false;

            if (!dto.Event.Text.Contains($"{SlackConsts.BotMention}"))
                return false;

            return dto.Token == _options.VerificationToken
                && dto.TeamId == _options.TeamId
                && dto.ApiAppId == _options.ApiAppId;
        }
    }
}
