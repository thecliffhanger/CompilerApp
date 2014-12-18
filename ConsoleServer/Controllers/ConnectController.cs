using EntityModel;
using Microsoft.Owin;
using OwinSelfhostServer;
using OwinSelfhostServer.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;

namespace OwinSelfhostServer
{
    [HandleException]
    public class ConnectController : ApiController
    {        
        protected ClientModel _clientModel = new ClientModel();

        // GET api/<controller>
        [AllowAnonymous]
        [ActionName("Connect")]
        public bool GetConnect()
        {
            var _callerIP = GetClientIp();
            var guid = _clientModel.Connect(_callerIP);
            return true;
        }
        

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [AllowAnonymous]
        [ActionName("Disconnect")]
        public bool Delete()
        {
            var client = GetClientIp();
            var result = _clientModel.Disconnect(client);
            return true;
        }


        private Client GetClientIp(HttpRequestMessage request = null)
        {
            Client c = new Client();
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                c.IPAddress = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                c.HostName = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostName;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)this.Request.Properties[RemoteEndpointMessageProperty.Name];
                c.IPAddress = prop.Address;
                c.HostName = prop.Address;
            }
            if (request.Properties.ContainsKey("MS_OwinContext"))
            {
                c.IPAddress =((OwinContext)request.Properties["MS_OwinContext"]).Request.RemoteIpAddress;
                c.HostName = ((OwinContext)request.Properties["MS_OwinContext"]).Request.RemoteIpAddress;
            }
            else if (HttpContext.Current != null)
            {
                c.IPAddress = HttpContext.Current.Request.UserHostAddress;
                c.HostName = HttpContext.Current.Request.UserHostName;
            }
            else
            {
                return null;
            }

            return c;
        }
    }
}