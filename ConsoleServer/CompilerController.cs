using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Web.Hosting;

namespace OwinSelfhostServer 
{ 
    public class CompilerController : ApiController 
    { 
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
            var path = "../../BuildConfigs/NAnt.build";
            objXMLDoc.Load(path);
            return objXMLDoc.InnerXml;
        }

        [AllowAnonymous]
        [HttpGet]
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

