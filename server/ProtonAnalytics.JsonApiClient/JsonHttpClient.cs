using Newtonsoft.Json;
using ProtonAnalytics.JsonApi.Transport;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        public JsonHttpClient(string baseUrl = null)
        {
            if (string.IsNullOrEmpty(baseUrl)) {
                baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
            }

            this.baseUrl = baseUrl;
        }

        public JsonApiObject Get(string relativeUrl)
        {            
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(string.Format("{0}/{1}", baseUrl, relativeUrl));
                response.Wait();

                var task = response.Result.Content.ReadAsStringAsync();
                task.Wait();

                if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new JsonApiObject(new string[] { string.Format("Error calling {0}: {1}", response.Result.RequestMessage.RequestUri, response.Result.ReasonPhrase) });
                }
                else
                {
                    // Deserializing to JsonApiObject doesn't work; all fields are null. Why?
                    // Adding the "Type" property to the JSON didn't help, either.
                    var raw = JsonConvert.DeserializeObject<dynamic>(task.Result);
                    var data = raw.Data;
                    var errors = raw.Errors as IEnumerable<string>;

                    if (errors != null)
                    {
                        return new JsonApiObject(errors);
                    }
                    else
                    {
                        return new JsonApiObject(data);
                    }
                }
            }
        }
    }
}