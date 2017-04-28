using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SlackBot.Dto.Slack;
using SlackBot.Service;
using SlackBot.Settings;
using SlackBot.Validator;

namespace SlackBot.Controllers
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
            var service = new DocomoService(_options.AiDocomoToken, SlackConsts.BotMention);
            var dto = await service.Trend();
            var trend = dto.ArticleContents.Shuffle();
            var response = $"{trend.ContentData.Title}\n{trend.ContentData.LinkUrl}\n{trend.ContentData.ImageUrl}";
            return Ok(response);
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

            var docomoService = new DocomoService(_options.AiDocomoToken, SlackConsts.BotMention);
            var response = await docomoService.GetResponse(dto.Event.Text);

            // POST Slack
            await new SlackChatMessageService(_options.BotAccessToken).Post(dto.Event.Channel, response);

            _logger.LogDebug($"### req {dto.Event.Text}");
            _logger.LogDebug($"### res {response}");

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
