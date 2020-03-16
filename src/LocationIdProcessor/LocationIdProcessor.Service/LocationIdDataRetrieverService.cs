using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RightmovePostcodeToLocationId.LocationIdProcessor.Service.Models;
using RightmovePostcodeToLocationId.LocationIdProcessor.Service.Settings;

namespace RightmovePostcodeToLocationId.LocationIdProcessor.Service
{
    public class LocationIdDataRetrieverService : BackgroundService
    {
        private readonly ILogger<LocationIdDataRetrieverService> _logger;
        private readonly IOptions<LocationIdProcessorSettings> _locationIdProcessorSettings;

        public LocationIdDataRetrieverService(IOptions<LocationIdProcessorSettings> locationIdProcessorSettings, ILogger<LocationIdDataRetrieverService> logger)
        {
            if (locationIdProcessorSettings.Value == null)
                throw new ArgumentNullException(nameof(locationIdProcessorSettings),
                    "LocationIdProcessorSettings must be provided");
            _logger = logger;
            _locationIdProcessorSettings = locationIdProcessorSettings;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Waiting for RabbitMQ to spin up");
            Thread.Sleep(new TimeSpan(0, 0, 0, 30));
            
            var factory = new ConnectionFactory
            {
                Uri = $"amqp://guest:guest@{_locationIdProcessorSettings.Value.RabbitMq.Host}/"
            };
            _logger.LogInformation("Creating connection");
            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare(queue: "new-locationid-retrieved",
                        false,
                        false,
                        false,
                        null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = JsonConvert.DeserializeObject<RecordToProcess>(Encoding.UTF8.GetString(body));

                        _logger.LogInformation($"Processing postcode {message.Postcode} and locationId {message.LocationId}");
                        
                        Console.WriteLine(" [x] Received {0}", message);
                    };
                    channel.BasicConsume(queue: "new-locationid-retrieved",
                        noAck: false,
                        consumer: consumer);
                }
            }

            return Task.CompletedTask;
        }
    }
}
