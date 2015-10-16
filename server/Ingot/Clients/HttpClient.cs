using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Ingot.Clients
{
    /// <summary>
    /// A class that can make HTTP/JSON requests, and accept HTTP/JSON responses, including pure JSON responses.
    /// </summary>
    public class HttpClient
    {
        private CookieContainer cookieJar = new CookieContainer();
        private string baseUrl = "";

        public HttpClient(string baseUrl = "")
        {
            this.baseUrl = baseUrl;
        }

        public HttpWebResponse Request(string httpVerb, string relativeUrl, string body = "")
        {
            var request = (HttpWebRequest)WebRequest.Create(this.baseUrl + relativeUrl);
            // Cookie container is automatically set and accepts Set-Cookie headers; they will also be set on the response object.
            // c/o: http://stackoverflow.com/a/1055883/210780
            request.CookieContainer = cookieJar;

            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = httpVerb;

            if (!string.IsNullOrEmpty(body))
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(body);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }

            var response = (HttpWebResponse)request.GetResponse();
            return response;
        }

        public HttpWebResponse Request(string httpVerb, string relativeUrl, Dictionary<string, string> formData)
        {
            // Encode form data
            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString("");
            foreach (var kvp in formData)
            {
                outgoingQueryString.Add(kvp.Key, kvp.Value);
            }
            string postData = outgoingQueryString.ToString();
            return Request(httpVerb, relativeUrl, postData);
        }
    }

    public static class HttpWebResponseExtensions
    {
        public static string Content(this HttpWebResponse httpResponse)
        {
            string result;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }
    }
}