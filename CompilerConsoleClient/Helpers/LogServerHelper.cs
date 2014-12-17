using EntityModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;

namespace Client.Helpers
{
    public class LogServerHelper
    {
        HttpClient _client;
        string _logServerUrl;

        public LogServerHelper()
        {
            this._client = new HttpClient();
            this._logServerUrl = ConfigurationManager.AppSettings["LogServerUrl"];
        }

        public List<LogItem> Get()
        {
            HttpResponseMessage response = _client.GetAsync(this._logServerUrl).Result;
            var data = response.Content.ReadAsAsync<List<LogItem>>().Result;
            return data;
        }

        public bool Post(LogItem obj)
        {            
            HttpResponseMessage response = _client.PostAsync<LogItem>(_logServerUrl, obj, new XmlMediaTypeFormatter()).Result;
            var data = response.Content.ReadAsAsync<bool>().Result;
            return data;
        }

        public string Put(string Id, LogItem obj)
        {
            return "";
        }

        public string Delete(string Id)
        {
            return "";
        }
    }
}
