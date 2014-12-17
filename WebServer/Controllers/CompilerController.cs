using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebServer.Controllers
{
    [Authorize]
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


        private XmlDocument GetXML(string xmlContent)
        {
            XmlDocument objXMLDoc = new XmlDocument();
            objXMLDoc.LoadXml(xmlContent);
            return objXMLDoc;
        }

        [AllowAnonymous]
        [ActionName("GetCompileCommand")]
        public HttpResponseMessage GetNantCompileProjectCommand()
        {
            try
            {
                string compileCmd = "<?xml version=\"1.0\"?>" +
                        "<project name=\"Getting Started with NAnt\" default=\"build\" basedir=\".\">" +
                        "<target name=\"build\" description=\"Build a simple project\">" +
                        "<csc target=\"exe\" output=\"Simple.exe\" debug=\"true\">" +
                        "<sources> " +
                         "<include name=\"simple.cs\" /> " +
                        "</sources>" +
                        "</csc>" +
                         "</target>" +
                        "</project>";

                //return compileCmd;


                return new HttpResponseMessage()
                {
                    Content = new StringContent(compileCmd, Encoding.UTF8, "application/xml")
                };
                //return GetXML(compileCmd).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
