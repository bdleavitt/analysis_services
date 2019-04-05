using System;
using System.Threading.Tasks;
using Microsoft.AnalysisServices.AdomdClient;

namespace Service_Principal_Connect_to_AzAS_via_ADOMD
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

            var connection = new AdomdConnection(ConnectionString);
            connection.Open();

            System.Console.WriteLine($"Connected to {connection.Database}");

            Console.WriteLine("Cubes hosted in Database: ");
            foreach (CubeDef cube in connection.Cubes)
            {
                Console.WriteLine($"{cube.Name}");
                Console.WriteLine($"\t Named Sets");
                foreach (NamedSet ns in cube.NamedSets)
                {
                    Console.WriteLine($"\t\t{ns.Name}");
                }
                Console.WriteLine($"\t Named Sets");
                foreach (Measure ms in cube.Measures)
                {
                    Console.WriteLine($"\t\t{ms.Name} || {ms.UniqueName}");
                }
            }
        }
    }
}
