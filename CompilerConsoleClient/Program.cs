using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using System.Net;


namespace CompilerConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
            Console.ReadKey();
        }
        static async Task RunAsync()
        {

            using (var client = new HttpClient())
            {
                // TODO - Send HTTP requests
                // New code:
                client.BaseAddress = new Uri("http://localhost:4058/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var _formatters = new List<MediaTypeFormatter>() { new XmlMediaTypeFormatter() };

                // New code:
                HttpResponseMessage response = await client.GetAsync("api/Compiler");
                var result = response.Content.ReadAsStringAsync().Result;
                GetXML(result);
                response.EnsureSuccessStatusCode();
            }
        }

        private static void GetXML(string xmlContent)
        {
            XmlDocument objXMLDoc = new XmlDocument();
            objXMLDoc.LoadXml(xmlContent);
            var con = WebUtility.HtmlDecode(xmlContent);
            Console.WriteLine(con);
            //objXMLDoc.Save(Console.Out);
        }
    }
}
