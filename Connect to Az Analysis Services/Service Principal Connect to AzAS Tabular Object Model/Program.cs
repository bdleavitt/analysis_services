using System;
using System.Threading.Tasks;
using Microsoft.AnalysisServices.Tabular;

namespace TOMQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadFromAzureAS().Wait();
            Console.ReadKey();
        }
        private static async Task ReadFromAzureAS()
        {
            var ssasUrl = "<your AzAS url>"; //get this from your Azure AS connectionString, i.e. aspaaseastus2.asazure.windows.net
            var clientId = "<your service principal client guid>"; // client id for your service principal (note: add as admin to each each model needed to access)
            var clientSecret = "<you service principal secret key>";
            var domain = "<yourcompanyazureaddomain>"; // i.e. yourcompany.onmicrosoft.com, you can find this here: https://portal.azure.com/#blade/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/Overview
            var asServerName = "<yourservername>"; // ie. "yourasserver" in aspaaseastus2.asazure.windows.net/yourasserver
            var initialCatalog = "<initial catalog>"; //the model name you want to connect to.

            // Get an active directory token
            var token = await ADALTokenHelper.GetAppOnlyAccessToken(domain, $"https://{ssasUrl}", clientId, clientSecret);

            /* Connection String  */
            string ConnectionString = $"Provider=MSOLAP;Data Source=asazure://{ssasUrl}/{asServerName};Initial Catalog={initialCatalog};User ID=;Password={token};";

            // 
            // The using syntax ensures the correct use of the 
            // Microsoft.AnalysisServices.Tabular.Server object. 
            // 
            using (Server server = new Server())
            {
                server.Connect(ConnectionString);

                Console.WriteLine("Connection established successfully.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Server name:\t\t{0}", server.Name);
                Console.WriteLine("Server product name:\t{0}", server.ProductName);
                Console.WriteLine("Server product level:\t{0}", server.ProductLevel);
                Console.WriteLine("Server version:\t\t{0}", server.Version);
                Console.ResetColor();
                Console.WriteLine();

                Console.WriteLine("Databases on server");
                foreach (Database db in server.Databases)
                {
                    Console.WriteLine($"\t{db.Name} | {db.EstimatedSize} | {db.ModelType}");
                    Console.WriteLine("\t Tables in DB");
                    foreach (Table t in db.Model.Tables)
                    {
                        Console.WriteLine($"\t\t {t.Name}");
                        Console.WriteLine($"\t\t Columns");
                        foreach (Column c in t.Columns) {
                            Console.WriteLine($"\t\t\t {c.Name} {c.DataType} {c.DataCategory} {c.FormatString}");
                        }
                        Console.WriteLine($"\t\t Measures");
                        foreach (Measure m in t.Measures)
                        {
                            Console.WriteLine($"\t\t\t {m.Name} {m.DataType} {m.FormatString}");
                        }
                    }
                }
            }
            Console.WriteLine("Press Enter to close this console window.");
            Console.ReadLine();
        }
    }
}