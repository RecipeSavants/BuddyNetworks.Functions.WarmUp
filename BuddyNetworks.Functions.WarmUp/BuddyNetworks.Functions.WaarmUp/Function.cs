using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;

namespace BuddyNetworks.Functions.WarmUp
{
    public class Function
    {
        private readonly SqlConnection connection;

        public Function()
        {
            connection = new SqlConnection(Environment.GetEnvironmentVariable("sqlconnection"));
        }

        [FunctionName("Sites-WarmUp")]
        public async Task RunAsync([TimerTrigger("0 */2 * * * *")]TimerInfo myTimer, ILogger log)
        {
            connection.Open();
            var client = new HttpClient();
            var list = connection.Query<string>("select url from urls");
            foreach (var item in list)
            {
                await client.GetAsync(item);
            }
            connection.Close();
        }
    }
}

