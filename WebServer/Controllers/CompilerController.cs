using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Xml;
using WebServer.Filters;

namespace WebServer.Controllers
{
    [Authorize]
    [HandleExceptionAttribute]
    public class CompilerController : ApiController
    {
        // GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        //[Route("Compiler/{id}")]


        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }


        private string GetCommandXML()
        {
            XmlDocument objXMLDoc = new XmlDocument();
            string str = HostingEnvironment.MapPath("~/BuildConfigs/NAnt.build");
            objXMLDoc.Load("../../BuildConfigs/NAnt.build");
            return objXMLDoc.InnerXml;
        }

        [AllowAnonymous]
        [ActionName("GetCompileCommand")]
        public HttpResponseMessage GetNantCompileProjectCommand()
        {
            try
            {

                return new HttpResponseMessage()
                {
                    Content = new StringContent(GetCommandXML(), Encoding.UTF8, "application/xml")
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
