using EntityDAL;
using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebServer.Filters;

namespace WebServer.Controllers
{
    [HandleException]
    public class LogController : ApiController
    {
        // GET api/log
        public IEnumerable<LogItem> Get()
        {
            return new List<LogItem>();
        }

        // GET api/log/5
        public LogItem Get(int id)
        {
            return new LogItem();
        }

        
        // POST api/log
        public bool Post(LogItem value)
        {
            using (var db = new LogDBContext())
            {
                if (value != null)
                {
                    value.Source = GetClientIp().IPAddress + " " + GetClientIp().HostName;
                    value.Created = DateTime.Now;
                    db.LogItems.Add(value);                    
                    db.SaveChanges();
                    return true;
                }                
            }
            return false;
        }

        // PUT api/log/5
        public void Put(int id, [FromBody]LogItem value)
        {
        }

        // DELETE api/log/5
        public void Delete(int id)
        {
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