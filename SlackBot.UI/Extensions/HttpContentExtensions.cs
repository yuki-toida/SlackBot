using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SlackBot.UI.Extensions
{
    public static class HttpContentExtensions
    {
        /// <summary> 
        /// HttpResponseMessageのContentからJSONをオブジェクトにデシリアライズするメソッド 
        /// </summary> 
        /// <typeparam name="T">JSONをデシリアライズする型</typeparam> 
        /// <param name="content">HttpContent</param> 
        /// <returns>HttpContentから読み込んだJSONをデシリアライズした結果のオブジェクト</returns> 
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var binary = await content.ReadAsByteArrayAsync();
            var jsonText = Encoding.UTF8.GetString(binary, 0, binary.Length);
            return JsonConvert.DeserializeObject<T>(jsonText);

            //var text = await content.ReadAsStringAsync();
            //return JsonConvert.DeserializeObject<T>(text);
        }   
    }
}
