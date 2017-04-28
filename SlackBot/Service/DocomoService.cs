using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SlackBot.Dto.Docomo;
using SlackBot.Extensions;

namespace SlackBot.Service
{
    public class DocomoService
    {
        private readonly string _token;
        private readonly string _botMention;

        public DocomoService(string token, string botMention)
        {
            _token = token;
            _botMention = botMention;
        }

        /// <summary>
        /// Botの返信取得
        /// </summary>
        public async Task<string> GetResponse(string text)
        {
            string response;
            if (text.Contains("ニュース") || text.Contains("news"))
            {
                var trends = await Trend();
                var trend = trends.ArticleContents.Shuffle();
                response = $"{trend.ContentData.Title}\n{trend.ContentData.LinkUrl}";
                if (!string.IsNullOrWhiteSpace(trend.ContentData.ImageUrl))
                    response += $"\n{trend.ContentData.ImageUrl}";
            }
            else
            {
                var dialog = await Dialog(text);
                response = dialog.Utt;
            }

            return $"{response}".Replace(_botMention, "");
        }

        /// <summary>
        /// 対話
        /// </summary>
        public async Task<DocomoDialogDto> Dialog(string text)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(new
                {
                    utt = text,
                    sex = (new[] { "男", "女" }).Shuffle(),
                    bloodtype = (new[] { "A", "B", "O", "AB" }).Shuffle(),
                    birthdateY = (Enumerable.Range(1980, 35)).Shuffle(),
                    birthdateM = (Enumerable.Range(1, 12)).Shuffle(),
                    birthdateD = (Enumerable.Range(1, 31)).Shuffle(),
                    age = (Enumerable.Range(20, 40)).Shuffle(),
                    constellations = (new[] { "牡羊座", "牡牛座", "双子座", "蟹座", "獅子座", "乙女座", "天秤座", "蠍座", "射手座", "山羊座", "水瓶座", "魚座" }).Shuffle(),
                    place = "東京",
                });

                const string baseUrl = "https://api.apigw.smt.docomo.ne.jp/dialogue/v1/dialogue";
                var url = $"{baseUrl}?APIKEY={_token}";
                var response = await client.PostAsync(url, new StringContent(json));
                return await response.Content.ReadAsJsonAsync<DocomoDialogDto>();
            }
        }

        /// <summary>
        /// トレンド記事
        /// </summary>
        public async Task<DocomoTrendDto> Trend()
        {
            using (var client = new HttpClient())
            {
                // https://dev.smt.docomo.ne.jp/?p=docs.api.page&api_name=trend_article_extraction&p_name=api_usage_scenario
                var genreId = new[] { 1, 2 }.Shuffle();
                const int num = 20;

                const string baseUrl = "https://api.apigw.smt.docomo.ne.jp/webCuration/v3/contents";
                var url = $"{baseUrl}?genreId={genreId}&n={num}&APIKEY={_token}";
                var response = await client.GetAsync(url);
                return await response.Content.ReadAsJsonAsync<DocomoTrendDto>();
            }
        }
    }
}

