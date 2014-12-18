using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Web.Hosting;
using OwinSelfhostServer.Filters;
using EntityModel;

namespace OwinSelfhostServer
{
    [HandleException]
    public class ClientController : ApiController
    {
        protected ClientModel _clientModel = new ClientModel();

        // GET api/<controller>
        [AllowAnonymous]        
        public List<Client> Get()
        {
            return _clientModel.GetClients();
        }
    }
}
