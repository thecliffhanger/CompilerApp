using EntityModel;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;

namespace WebServer.Models
{
    public class ClientModel
    {
        HttpClient _client;
        string _serverUrl;

        public ClientModel()
        {
            this._client = new HttpClient();
            this._serverUrl = ConfigurationManager.AppSettings["serverURL"];
        }

        public List<Client> GetClients()
        {
            HttpResponseMessage response = _client.GetAsync(this._serverUrl).Result;
            var data = response.Content.ReadAsAsync<List<Client>>().Result;
            return data;
        }

    }
}