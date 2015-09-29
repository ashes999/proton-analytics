using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProtonAnalytics.JsonApi.Transport
{
    public class JsonApiObject
    {
        public dynamic Data { get; private set; }
        public string[] Errors { get; private set; }

        public JsonApiObject() { }

        public JsonApiObject(dynamic data)
        {
            this.Data = data;
        }

        public JsonApiObject(string[] errors)
        {
            this.Errors = errors;
        }

        public override string ToString()
        {
            // Deserialize, and include the type            
            var json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}