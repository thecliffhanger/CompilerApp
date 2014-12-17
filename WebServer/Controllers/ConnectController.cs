using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServer.Filters;

namespace WebServer.Controllers
{
    [HandleExceptionAttribute]
    public class ConnectController : ApiController
    {
        // GET api/<controller>
        [AllowAnonymous]
        [ActionName("Connect")]
        public bool GetConnect()
        {
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
        public void Delete(int id)
        {
        }
    }
}