using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Web.Hosting;

namespace OwinSelfhostServer 
{
    public class ConnectController : ApiController
    {
        // GET api/values/5 

        [AllowAnonymous]
        [ActionName("Connect")]
        public bool GetConnect()
        {
            return true;
        }

        // POST api/values 
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
} 

