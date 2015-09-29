using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProtonAnalytics.JsonApiClient
{
    // Tries to meet jsonapi.org's requirements
    public class JsonApiObject<T>
    {
        public T Data { get; private set; }
        public string[] Errors { get; private set; }

        public JsonApiObject(T data)
        {
            this.Data = data;
        }

        public JsonApiObject(string[] errors)
        {
            this.Errors = errors;
        }

        public override string ToString()
        {
            //var builder = new StringBuilder();
            //builder.Append('{');
            //if (this.Errors != null && this.Errors.Length >= 1)
            //{
            //    builder.Append("'errors': [");
            //    foreach (var error in this.Errors)
            //    {
            //        builder.Append(string.Format("'{0}'", error.Replace("'", "\\'")));
            //    }
            //    builder.Append("]");
            //}
            //else
            //{
            //    if (this.Data == null) {
            //        this.Data = default(T);
            //    }
            //    builder.Append(string.Format("'data': {{ {0} }}", JsonConvert.SerializeObject(this.Data)));
            //}
            //builder.Append('}');

            //return builder.ToString();

            return JsonConvert.SerializeObject(this);
        }
    }
}