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
using System.Runtime.InteropServices;
using System.Configuration;


namespace CompilerConsoleClient
{
    class Program
    {
        static HttpClient client;
        static StringBuilder commandInputs;

        static Program()
        {
            commandInputs = new StringBuilder();
            client = new HttpClient();
        }

        static void Main(string[] args)            
        {
            client.BaseAddress = new Uri("http://localhost:9000/");
            client.DefaultRequestHeaders.Accept.Clear();

            var _formatters = new List<MediaTypeFormatter>() { new XmlMediaTypeFormatter() };

            Console.WriteLine();
        BEGIN:
            Console.WriteLine("Connecting to the server...");
            client.DefaultRequestHeaders.Accept.Clear();

            //var response = client.GetAsync(client.BaseAddress + "api/Connect").Result;


            RunAsync(client).Wait();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));


            int input = 0;

            while (!exitSystem)
            {

            MENU:

                Console.WriteLine("Please select from the following actions\n");
                Console.WriteLine("1.Compile a project");
                Console.WriteLine("2.Compile and Test a project");
                Console.WriteLine("3.Disconnect \n");
                commandInputs = new StringBuilder();
                var inputText = Console.ReadLine();

                if (!Int32.TryParse(inputText, out input))
                    goto MENU;

                switch (input)
                {
                    case 1:
                        Console.WriteLine("\nEnter the path of the solution(project) to compile using NAnt:\n");
                        var sourcePath = Console.ReadLine();
                        commandInputs.AppendLine("SourcePath: " + sourcePath);
                        ExecuteCommand(client, sourcePath, input);
                        break;
                    case 2:
                        Console.WriteLine("\nEnter the path of the solution(project) to compile and test using NAnt:\n");
                        sourcePath = Console.ReadLine();
                        commandInputs.AppendLine("SourcePath: " + sourcePath);
                        ExecuteCommand(client, sourcePath, input);
                        break;
                    case 3:
                        Console.WriteLine("Disconnecting...");
                        RunDisconnectAsync(client).Wait();
                        exitSystem = true;
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

        private static void ExecuteCommand(HttpClient client, string path, int input)
        {
            try
            {
                string commandXml = GetCommand(client, path, input).Result;
                var output = RunCommand(path, commandXml);
                Log("Info", input, output);
            }
            catch (Exception ex) { Log("Error", content: ex.StackTrace); }
        }

        private static string RunCommand(string path, string commandXml)
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


        private static async Task RunDisconnectAsync(HttpClient client)
        {
            HttpResponseMessage response = await client.DeleteAsync("api/Connect");
            var result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Disconnected from the server \n");
            response.EnsureSuccessStatusCode();
        }

        private static async Task RunAsync(HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync("api/Connect");
            var result = response.Content.ReadAsStringAsync().Result;
            if (bool.Parse(result))
                Console.WriteLine("Connected to the server \n");
            response.EnsureSuccessStatusCode();
        }

        private static async Task<string> GetCommand(HttpClient client, string path, int input)
        {
            HttpResponseMessage response = await client.GetAsync("api/Compiler/GetCompileCommand");
            var commandXml = response.Content.ReadAsStringAsync().Result;
            commandXml = GetCommandXML(path);
            string xmlPath = SetOnlyCompilation(commandXml, path, input);
            response.EnsureSuccessStatusCode();
            return xmlPath;
        }

        private static string GetCommandXML(string path)
        {
            var buildFile = Directory.GetFiles(path, "*.build", SearchOption.AllDirectories).Select(x => Path.GetFileNameWithoutExtension(x)).First();
            XmlDocument objXMLDoc = new XmlDocument();
            objXMLDoc.Load(Path.GetFullPath(path) + "\\" + buildFile + ".build");
            return objXMLDoc.InnerXml;
        }

        private static string SetOnlyCompilation(string xmlContent, string path, int input)
        {
            var buildFiles = Directory.GetFiles(path, "*.build", SearchOption.AllDirectories).Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
            foreach (var buildFile in buildFiles)
            {
                var buildFileLocation = Path.GetFullPath(path) + "\\" + buildFile + ".build";
                var objXMLDoc = XDocument.Parse(xmlContent);

                //set run.nunit.tests to false so that the tests would not run.
                var runtest = GetElementByAttibuteValue(objXMLDoc, "name", "run.nunit.tests");
                runtest.SetAttributeValue("value", (input == 2) ? true : false);
                commandInputs.AppendLine("run.nunit.tests: " + ((input == 2) ? true : false).ToString());

                objXMLDoc.Save(buildFileLocation); 
            }

            return Path.GetFullPath(path) + buildFiles.First();
        }

        private static string SaveCommandXML(string xmlContent, string path, int input)
        {
            var projFiles = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories).Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
            var testProjFiles = Directory.GetFiles(path, "*Tests.csproj", SearchOption.AllDirectories).Select(x => Path.GetFileNameWithoutExtension(x)).ToList();

            var objXMLDoc = XDocument.Parse(xmlContent);
            var solName = GetElementByAttibuteValue(objXMLDoc, "name", "solution.name");
            solName.SetAttributeValue("value", Path.GetFileName(path));
            commandInputs.AppendLine("solution.name: " + Path.GetFileName(path));

            var solSrcDir = GetElementByAttibuteValue(objXMLDoc, "name", "solution.src.dir");
            solSrcDir.SetAttributeValue("value", "");
            commandInputs.AppendLine("solution.src.dir: " + "");

            var solProjs = GetElementByAttibuteValue(objXMLDoc, "name", "solution.projects");
            solProjs.SetAttributeValue("value", string.Join(",", projFiles));
            commandInputs.AppendLine("solution.projects: " + string.Join(",", projFiles));

            var solTestProjs = GetElementByAttibuteValue(objXMLDoc, "name", "test.project.names");
            solTestProjs.SetAttributeValue("value", string.Join(",", testProjFiles));
            commandInputs.AppendLine("test.project.names: " + string.Join(",", testProjFiles));

            var solWS = GetElementByAttibuteValue(objXMLDoc, "name", "local.workspace");
            solWS.SetAttributeValue("value", Path.GetFullPath(path));
            commandInputs.AppendLine("local.workspace: " + Path.GetFullPath(path));

            var runtest = GetElementByAttibuteValue(objXMLDoc, "name", "run.nunit.tests");
            runtest.SetAttributeValue("value", (input == 2) ? true : false);
            commandInputs.AppendLine("run.nunit.tests: " + ((input == 2) ? true : false).ToString());

            var msbuild = GetElementByAttibuteValue(objXMLDoc, "name", "msbuild4.exe");
            msbuild.SetAttributeValue("value", ConfigurationManager.AppSettings["msbuild4.exe"]);

            var csc = GetElementByAttibuteValue(objXMLDoc, "name", "cspack.exe");
            csc.SetAttributeValue("value", ConfigurationManager.AppSettings["cspack.exe"]);

            var ps = GetElementByAttibuteValue(objXMLDoc, "name", "powershell.exe");
            ps.SetAttributeValue("value", ConfigurationManager.AppSettings["powershell.exe"]);

            var nunit = GetElementByAttibuteValue(objXMLDoc, "name", "nunit-console.exe");
            nunit.SetAttributeValue("value", ConfigurationManager.AppSettings["nunit-console.exe"]);

            var con = WebUtility.HtmlDecode(xmlContent);

            // Generating a NAnt build file from this application
            string xmlPath = Path.Combine(path, "Generated_NAnt_Build.xml");
            objXMLDoc.Save(xmlPath);

            Console.WriteLine("A NAnt Build file has been generated at:");
            Console.WriteLine(xmlPath);

            return xmlPath;
        }

        private static XElement GetElementByAttibuteValue(XDocument source, string attrName, string attrValue)
        {
            var xelements = source.Root.Elements("property");
            var xelement = xelements.Where(el => (string)el.Attribute(attrName) == attrValue).FirstOrDefault();
            return xelement;
        }

        private static void Log(string type, int input = 0, string content = "")
        {
            try
            {
                LogServerHelper logger = new LogServerHelper();                
                logger.Post(new LogItem()
                {
                    Id = Guid.NewGuid(),
                    LogType = type,
                    Command = GetCommandString(input),
                    Inputs = commandInputs.ToString(),
                    LogText = content
                });
            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("Client", ex.Message);
            }
            finally
            {
                commandInputs = new StringBuilder();
            }
        }


        private static string GetCommandString(int input)
        {
            if (input == 1)
                return "Compile";
            else if (input == 2)
                return "Compile & Test";
            else if (input == 3)
                return "Disconnect";
            return "";
        }

        static bool exitSystem = false;

        #region Trap application termination
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            Console.WriteLine("Exiting system due to external CTRL-C, or process kill, or shutdown");

            if (!exitSystem)
                RunDisconnectAsync(client).Wait();

            Console.WriteLine("Cleanup complete");

            //allow main to run off
            exitSystem = true;

            //shutdown right away so there are no lingering threads
            Environment.Exit(-1);

            return true;
        }
        #endregion

    }
}
