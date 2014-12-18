using System.Web.Configuration;
using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;
using System.Configuration;

namespace OwinSelfhostServer
{
    public class Program
    {
        static void Main()
        {
            var baseAddress =new Uri(ConfigurationManager.AppSettings["serverURL"]);

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress.AbsoluteUri))
            {
                // Create HttpCient and make a request to api/values 
                //HttpClient client = new HttpClient();
                //var response = client.GetAsync(baseAddress + "api/Connect").Result;
                //Console.WriteLine(response);
                //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
               
                Console.WriteLine();
                Console.WriteLine("Server is now hosted sucessfully");
                Console.WriteLine();
                Console.WriteLine("Waiting for Client Connections....");
                
                while (true)
                {
                    continue;
                }
            }

            Console.ReadLine();
        }
    }
}