using SlackBot.UI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlackBot.UI.Settings;
using SlackBot.UI.Dto.Slack;

namespace SlackBot.UI.Validator
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
