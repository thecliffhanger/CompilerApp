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
using System.IO;
using System.Xml.Linq;
using Client.Helpers;
using EntityModel;


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
                if (!Int32.TryParse(inputText, out input))
                    goto MENU;

                switch (input)
                {
                    case 1:
                        Console.WriteLine("\nEnter the path of the solution(project) to compile and test using NAnt:\n");
                        var sourcePath = Console.ReadLine();
                        ExecuteCompileCommand(client, sourcePath);
                        break;
                    case 2:
                        Console.WriteLine("Disconnecting...");
                        exit = true;
                        break;
                }
            }
            Console.WriteLine("Disconnected! \nSelect any of the following:\n");
            Console.WriteLine("1.Connect to the server");
            Console.WriteLine("2.Quit");
            var fIn = Int32.Parse(Console.ReadLine());
            if (fIn == 1)
                goto BEGIN;
        }

        private static void ExecuteCompileCommand(HttpClient client, string path)
        {
            try
            {
                GetCompileCommand(client, path).Wait();
                var output = RunCompileCommand(path);
                Log("Info", output);
            }
            catch (Exception ex) { Log("Error", ex.StackTrace); }
        }

        private static string RunCompileCommand(string path)
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

                string output = compiler.StandardOutput.ReadToEnd();
                Console.WriteLine(output);

                compiler.WaitForExit();

                CompileOpToXML(path);

                return output;
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

        private static async Task GetCompileCommand(HttpClient client, string path)
        {
            HttpResponseMessage response = await client.GetAsync("api/Compiler/GetCompileCommand");
            var result = response.Content.ReadAsStringAsync().Result;
            SaveCompileCommandXML(result, path);
            response.EnsureSuccessStatusCode();
        }

        private static void SaveCompileCommandXML(string xmlContent, string path)
        {
            var projFiles = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories).Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
            var testProjFiles = Directory.GetFiles(path, "*Tests.csproj", SearchOption.AllDirectories).Select(x => Path.GetFileNameWithoutExtension(x)).ToList();

            var objXMLDoc = XDocument.Parse(xmlContent);
            var solName = GetElementByAttibuteValue(objXMLDoc, "name", "solution.name");
            solName.SetAttributeValue("value", Path.GetFileName(path));

            var solSrcDir = GetElementByAttibuteValue(objXMLDoc, "name", "solution.src.dir");
            solSrcDir.SetAttributeValue("value", "");

            var solProjs = GetElementByAttibuteValue(objXMLDoc, "name", "solution.projects");
            solProjs.SetAttributeValue("value", string.Join(",", projFiles));

            var solTestProjs = GetElementByAttibuteValue(objXMLDoc, "name", "test.project.names");
            solTestProjs.SetAttributeValue("value", string.Join(",", testProjFiles));

            var con = WebUtility.HtmlDecode(xmlContent);
            objXMLDoc.Save(Path.Combine(path, "Nant.build"));
        }

        private static XElement GetElementByAttibuteValue(XDocument source, string attrName, string attrValue)
        {
            var xelements = source.Root.Elements("property");
            var xelement = xelements.Where(el => (string)el.Attribute(attrName) == attrValue).FirstOrDefault();
            return xelement;
        }

        private static void Log(string type, string content)
        {
            try
            {
                LogServerHelper logger = new LogServerHelper();
                logger.Post(new LogItem() { Id = Guid.NewGuid(), LogType = type, Source = "Client", LogText = content });
            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("Client", ex.Message);
            }
        }
    }
}
