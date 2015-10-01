using Newtonsoft.Json;
using ProtonAnalytics.JsonApi.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ProtonAnalytics.Web.Api
{
    public class JsonHttpClient
    {
        private string baseUrl;
        private HttpClient httpClient = new HttpClient();

        public JsonHttpClient()
        {
            this.baseUrl = ConfigurationManager.AppSettings["ApiUrl"];

            if (string.IsNullOrEmpty(this.baseUrl))
            {
                throw new InvalidOperationException("Please specify an app setting for ApiUrl in app.config");
            }
        }

        //public JsonApiObject<T> Get<T>(string relativeUrl)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        var response = client.GetAsync(string.Format("{0}/{1}", baseUrl, relativeUrl));
        //        response.Wait();

        //        var task = response.Result.Content.ReadAsStringAsync();
        //        task.Wait();

        //        if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
        //        {
        //            return new JsonApiObject<T>() { Errors = new string[] { string.Format("Error calling {0}: {1}", response.Result.RequestMessage.RequestUri, response.Result.ReasonPhrase) } };
        //        }
        //        else
        //        {
        //            // Deserializing to JsonApiObject doesn't work; all fields are null. Why?
        //            // Adding the "Type" property to the JSON didn't help, either.
        //            var raw = JsonConvert.DeserializeObject<JsonApiObject<T>>(task.Result);
        //            var data = raw.Data;
        //            var errors = raw.Errors as IEnumerable<string>;

        //            if (errors != null)
        //            {
        //                return new JsonApiObject<T>() { Errors = errors };
        //            }
        //            else
        //            {
        //                return new JsonApiObject<T>(data);
        //            }
        //        }
        //    }
        //}

        internal async Task<HttpResponseMessage> PostAsJsonAsync<T>(string relativeUrl, T value)
        {
            var toReturn = await this.httpClient.PostAsJsonAsync<T>(this.baseUrl + relativeUrl, value);
            return toReturn;
        }
    }
}