using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProtonAnalytics.JsonApi.Api
{
    public class JsonApiObject<T>
    {
        private List<T> data = null;
        private IEnumerable<string> errors = null;

        public List<T> Data
        {
            get { return this.data; }
            set
            {
                if (value != null)
                {
                    if (this.Errors != null)
                    {
                        throw new InvalidOperationException("Can't set both data and errors (errors are already set)");
                    }
                    this.data = value;
                }
            }
        }

        public IEnumerable<string> Errors
        {
            get { return this.errors; }
            set
            {
                if (value != null)
                {
                    if (this.Data != null)
                    {
                        throw new InvalidOperationException("Can't set both data and errors (data is already set)");
                    }
                    this.errors = value;
                }
            }
        }

        public JsonApiObject() { }

        public JsonApiObject(T data)
        {
            this.Data = new List<T>();
            this.Data.Add(data);
        }

        public JsonApiObject(IEnumerable<T> data)
        {
            this.Data = data.ToList<T>();
        }

        public override string ToString()
        {
            // Deserialize, and include the type            
            var json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}