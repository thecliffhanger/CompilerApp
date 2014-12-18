using EntityModel;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;

namespace OwinSelfhostServer
{
    public class ClientModel
    {
        internal static List<Client> clients = new List<Client>();

        public List<Client> GetClients()
        {
            return clients;
        }

        public Client GetClient(Client c)
        {
            var client = clients.Where(x => x.IPAddress == c.IPAddress).FirstOrDefault();
            return client ?? GetTestClient();
        }

        public Guid Connect(Client c)
        {
            var client = GetClient(c);
            if ((client == null || client.HostName == "TestHost") && (!string.IsNullOrEmpty(c.IPAddress)))
            {
                client = new Client() { Id = Guid.NewGuid(), HostName = "", IPAddress = c.IPAddress };
                clients.Add(client);
            }
            return client.Id;
        }

        public bool Disconnect(Client c)
        {
            var client = GetClient(c);
            if (client != null)
            {
                clients.Remove(client);
            }

            return true;
        }

        public void GetUserData(out string callerIp)
        {
            callerIp = string.Empty;
            try
            {
                //userName = HttpContext.Current.User.Identity.Name;
                callerIp = HttpContext.Current.Request.UserHostAddress;
            }
            catch { }
        }

        private Client GetTestClient()
        {
            var client = clients.Where(x => x.HostName == "TestHost").FirstOrDefault();
            if (client == null)
            {
                client = new Client() { HostName = "TestHost", Id = Guid.NewGuid(), IPAddress = "TestIP" };
                clients.Add(client);
            }
            return client;
        }

    }
}