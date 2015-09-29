using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ProtonAnalytics.JsonApiClient
{
    // Tries to meet jsonapi.org's requirements
    public class JsonHttpClient
    {
        private string baseUrl;

        public JsonHttpClient(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public async Task<JsonApiObject<T>> Get<T>(string relativeUrl)
        {            
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(string.Format("{0}/{1}", baseUrl, relativeUrl));

                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                var content = task.Result;

                if (response.IsSuccessStatusCode)
                {
                    var asObject = JsonConvert.DeserializeObject<T>(content);
                    return new JsonApiObject<T>(asObject);
                }
                else
                {
                    return new JsonApiObject<T>(new string[] { content });
                }
            }
        }
    }
}