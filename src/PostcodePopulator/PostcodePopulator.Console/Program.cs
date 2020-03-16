using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace RightmovePostcodeToLocationId.PostcodePopulator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .AddEnvironmentVariables()
                .Build();

            var addresses = File.ReadAllLines("postcodes-london.csv");
            // Wait for the rabbitmq container to initialize and start up - swap out with better implementation
            Thread.Sleep(new TimeSpan(0, 0, 0, 60));
            System.Console.WriteLine($"Host Details : {config.GetSection("Rabbitmq").GetValue<string>("Host")}");
            var factory = new ConnectionFactory
            {
                Uri = $"amqp://guest:guest@{config.GetSection("Rabbitmq").GetValue<string>("Host")}/"
            };

            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare("new-postcode", false, false);

                    foreach (var address in addresses.Skip(1))
                    {
                        var postcode = address.Split(",");
                        if (postcode[1].Equals("Yes"))
                            channel.BasicPublish("", "new-postcode", null, Encoding.UTF8.GetBytes(postcode.First()));
                    }
                }
            }
        }
    }
}
