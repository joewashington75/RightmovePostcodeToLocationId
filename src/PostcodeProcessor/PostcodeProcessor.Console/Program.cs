using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PostcodeProcessor.Infrastructure.Repositories;
using RabbitMQ.Client;
using RightmovePostcodeToLocationId.PostcodeProcessor.Console.Factories;
using RightmovePostcodeToLocationId.PostcodeProcessor.Console.Helpers;
using RightmovePostcodeToLocationId.PostcodeProcessor.Core.Enums;

namespace RightmovePostcodeToLocationId.PostcodeProcessor.Console
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

            var driver = ChromeDriverFactory.GetChromeDriver();
            var postcodeMapperRepository = new PostcodeMapperRepository(config.GetValue<string>("SqlConnectionString"));

            // Wait for the rabbitmq container to initialize and start up - swap out with better implementation
            Thread.Sleep(new TimeSpan(0, 0, 0, 60));

            var factory = new ConnectionFactory
            {
                Uri = $"amqp://guest:guest@{config.GetSection("Rabbitmq").GetValue<string>("Host")}/"
            };

            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    try
                    {
                        while (true)
                        {
                            var result = channel.BasicGet("new-postcode", false);
                            if (result == null)
                            {

                            }
                            else
                            {
                                var postcode = Encoding.UTF8.GetString(result.Body);
                                var existingPostcode = postcodeMapperRepository.Get(postcode);

                                if (existingPostcode == null)
                                {
                                    postcodeMapperRepository.Create(postcode);
                                }
                                else
                                {
                                    if (existingPostcode.ProcessingStatus == ProcessingStatus.LocationIdRetrieved)
                                    {
                                        channel.BasicAck(result.DeliveryTag, false);
                                        continue;
                                    }
                                    postcodeMapperRepository.Update(postcode, ProcessingStatus.SearchForPostcode);
                                }

                                driver.NavigateToRightMove()
                                    .AddPostcodeToSearchLocation(postcode)
                                    .ClickBuy();

                                var (found, status) = driver.CheckPostcodeExists();

                                if (!found)
                                {
                                    System.Console.WriteLine($"Postcode {postcode} not found");
                                    postcodeMapperRepository.Update(postcode, status);
                                    continue;
                                }

                                postcodeMapperRepository.Update(postcode, ProcessingStatus.PostcodeFound);
                                System.Console.WriteLine($"Postcode {postcode} found - processing");

                                driver.ClickSubmit();

                                var url = driver.Url;
                                postcodeMapperRepository.Update(postcode, ProcessingStatus.RetrievingLocationId);

                                var postCodeSplit = url.Split("POSTCODE");
                                if (postCodeSplit.Any())
                                {
                                    var locationId = postCodeSplit[1].Split("&").First().Replace("%5", "");
                                    postcodeMapperRepository.Update(postcode, locationId);
                                    var postcodeLocationId = new
                                    {
                                        Postcode = postcode,
                                        LocationId = locationId
                                    };
                                    channel.BasicPublish("", "new-locationid-retrieved", null, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postcodeLocationId)));
                                }
                                else
                                {
                                    postcodeMapperRepository.Update(postcode, ProcessingStatus.PostcodeNotFound);
                                }

                                channel.BasicAck(result.DeliveryTag, false);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }
    }
}
