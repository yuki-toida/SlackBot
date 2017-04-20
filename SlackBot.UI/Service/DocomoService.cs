using SlackBot.UI.Extensions;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using SlackBot.UI.Dto;

namespace SlackBot.UI
{
    public static class DocomoContext
    {
        // 会話を継続する識別文字
        public static string Value { get; set; }
    }

    public class DocomoService
    {
        private const string Url = "https://api.apigw.smt.docomo.ne.jp/dialogue/v1/dialogue";
        private readonly string _token;

        public DocomoService(string token)
        {
            _token = token;
        }

        public async Task<DocomoDto> Post(string text)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(new
                {
                    utt = text,
                    context = DocomoContext.Value,
                    sex = Shuffle(new[] { "男", "女" }),
                    bloodtype = Shuffle(new[] { "A", "B", "O", "AB" }),
                    birthdateY = Shuffle(Enumerable.Range(1980, 35)),
                    birthdateM = Shuffle(Enumerable.Range(1, 12)),
                    birthdateD = Shuffle(Enumerable.Range(1, 31)),
                    age = Shuffle(Enumerable.Range(20, 40)),
                    constellations = Shuffle(new[] { "牡羊座", "牡牛座", "双子座", "蟹座", "獅子座", "乙女座", "天秤座", "蠍座", "射手座", "山羊座", "水瓶座", "魚座" }),
                    place = "東京",
                });

                var buildUrl = $"{Url}?APIKEY={_token}";
                var response = await client.PostAsync(buildUrl, new StringContent(json));
                var dto = await response.Content.ReadAsJsonAsync<DocomoDto>();

                // 会話継続のためにセット
                DocomoContext.Value = dto.Context;

                return dto;
            }
        }

        private static T Shuffle<T>(IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid()).First();
        }
    }
}
