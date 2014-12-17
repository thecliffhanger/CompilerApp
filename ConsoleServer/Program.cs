using System.Web.Configuration;
using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;

namespace OwinSelfhostServer
{
    public class Program
    {
        static void Main()
        {
            var baseAddress =new Uri(WebConfigurationManager.AppSettings["serverURL"]);

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress.AbsoluteUri))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/Connect").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                while (true)
                {
                    continue;
                }
            }

            Console.ReadLine();
        }
    }
}