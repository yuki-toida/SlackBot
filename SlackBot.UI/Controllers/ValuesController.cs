using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using SlackBot.UI.Dto;
using System.Net;
using SlackBot.UI.Dto.Event;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using SlackBot.App;
using SlackBot.UI.Validator;
using SlackBot.UI.Extensions;
using SlackBot.UI.Settings;

namespace SlackBot.UI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ValuesController : Controller
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly SlackOptions _options;

        public ValuesController(ILogger<ValuesController> logger, IOptions<SlackOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        [HttpGet]
        public IActionResult Get(string code)
        {
            return Ok(code);
        }

        /// <summary>
        /// EventAPIのエンドポイント
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Events([FromBody]EventDto dto)
        {
            var validator = new EventValidator(_options);
            if (!validator.Validate(dto))
                return BadRequest();

            var service = new ChatMessageService();
            var text = service.GetBotText(dto.Event.User, SlackConsts.BotName, SlackConsts.BotMention);
            await service.Post(_options.BotAccessToken, dto.Event.Channel, text);

            _logger.LogDebug($"### {dto.Event.Text} {text}");

            return Ok();
        }

        /// <summary>
        /// EventAPIのエンドポイント検証イベント用
        /// </summary>
        [HttpPost]
        public IActionResult Challenge([FromBody]UrlVerificationDto dto)
        {
            if (dto == null)
                return BadRequest();

            return Ok(WebUtility.UrlEncode(dto.Challenge));
        }
    }
}
