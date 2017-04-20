using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using SlackBot.UI.Dto;
using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using SlackBot.UI.Validator;
using SlackBot.UI.Extensions;
using SlackBot.UI.Service;
using SlackBot.UI.Settings;
using SlackBot.UI.Dto.Slack;

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
        public async Task<IActionResult> Get()
        {
            var service = new DocomoService(_options.AiDocomoToken);
            var dto = await service.Post("はじめまして");
            return Ok(dto);
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

            //var userLocal = new UserLocalService(_options.AiUserLocalToken);
            //var userLocalText = await userLocal.Get(dto.Event.Text);
            //if (userLocalText.Status != "success")
            //    return BadRequest();
            //_logger.LogDebug($"### {dto.Event.User} {WebUtility.HtmlDecode(dto.Event.User)}");

            var docomo = new DocomoService(_options.AiDocomoToken);
            var docomoText = await docomo.Post(dto.Event.Text);

            var service = new SlackChatMessageService(_options.BotAccessToken);
            var text = service.GetBotText(dto.Event.User, SlackConsts.BotMention, docomoText.Utt);
            await service.Post(dto.Event.Channel, text);

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
