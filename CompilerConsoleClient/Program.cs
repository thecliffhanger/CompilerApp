using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:4058/");
            client.DefaultRequestHeaders.Accept.Clear();

            var _formatters = new List<MediaTypeFormatter>() { new XmlMediaTypeFormatter() };

            Console.WriteLine();
            BEGIN:
            Console.WriteLine("Connecting to the server...");
            client.DefaultRequestHeaders.Accept.Clear();

            RunAsync(client).Wait();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            
            bool exit = false;
            int input = 0;

            while (!exit)
            {
            MENU:
                Console.WriteLine("Please select from the following actions\n");
                Console.WriteLine("1.Compile and Test a project");
                Console.WriteLine("2.Disconnect \n");
                var inputText = Console.ReadLine();
                if(!Int32.TryParse(inputText, out input))
                    goto MENU;

                switch (input)
                {
                    case 1:
                        //GetCompileCommand
                        Console.WriteLine("\nEnter the filepath(for Code) or zipfilePath(for Projects) to upload for compilation/to run test package");
                        //GetCompileCommand(client).Wait();
                        Console.WriteLine("\nEnter the path of the solution(project) to compile and test using NAnt:\n");
                        var path = Console.ReadLine();
                        CompileOp(path);
                        break;
                    case 2:
                        Console.WriteLine("Disconnedting...");
                        exit = true;
                        break;
                }
            }
            Console.WriteLine("Disconnected! \nSelect any of the following:\n");
            Console.WriteLine("1.Connect to the server");
            Console.WriteLine("2.Quit");
            var fIn = Int32.Parse(Console.ReadLine());
            if(fIn == 1)
                goto BEGIN;
        }

        private static void CompileOp(string path)
        {
            try
            {
                var compiler = new Process(); // necessary to begin NAnt running as a process, right??
                compiler.StartInfo.WorkingDirectory = path;

                compiler.StartInfo.FileName = "cmd.exe"; // the tool to execute on the cmd line
                compiler.StartInfo.UseShellExecute = false; // forces the cmdline to be used, right?
                compiler.StartInfo.RedirectStandardOutput = true;

                compiler.StartInfo.Arguments = "/C NAnt";
                compiler.Start();

                Console.WriteLine(compiler.StandardOutput.ReadToEnd());

                compiler.WaitForExit();

                CompileOpToXML(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void CompileOpToXML(string path)
        {
            try
            {
                var compiler = new Process(); // necessary to begin NAnt running as a process, right??
                compiler.StartInfo.WorkingDirectory = path;

                compiler.StartInfo.FileName = "cmd.exe"; // the tool to execute on the cmd line
                compiler.StartInfo.UseShellExecute = false; // forces the cmdline to be used, right?
                compiler.StartInfo.RedirectStandardOutput = true;

                compiler.StartInfo.Arguments = "/C NAnt -logger:NAnt.Core.XmlLogger -logfile:C:\\Temp\\buildlog.xml";
                compiler.Start();

                //Console.WriteLine(compiler.StandardOutput.ReadToEnd());

                compiler.WaitForExit();

                //Write logic to read xml file at C:\Temp\builddlog.xml and log it into database whether successful or not
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static async Task RunAsync(HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync("api/Connect");
            var result = response.Content.ReadAsStringAsync().Result;
            if (bool.Parse(result))
                Console.WriteLine("Connected to the server \n");
            response.EnsureSuccessStatusCode();
        }

        private static async Task GetCompileCommand(HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync("api/Compiler/GetCompileCommand");
            var result = response.Content.ReadAsStringAsync().Result;
            GetXML(result);
            response.EnsureSuccessStatusCode();
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
