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
using RightmovePostcodeToLocationId.LocationIdProcessor.Core.Interfaces.Infrastructure;
using RightmovePostcodeToLocationId.LocationIdProcessor.Service.Models;
using RightmovePostcodeToLocationId.LocationIdProcessor.Service.Settings;

namespace RightmovePostcodeToLocationId.LocationIdProcessor.Service
{
    public class LocationIdDataRetrieverService : BackgroundService
    {
        private readonly ILogger<LocationIdDataRetrieverService> _logger;
        private readonly IRightmoveService _rightmoveService;
        private readonly IOptions<LocationIdProcessorSettings> _locationIdProcessorSettings;
        private IConnection _connection;
        private IModel _channel;

        public LocationIdDataRetrieverService(IOptions<LocationIdProcessorSettings> locationIdProcessorSettings, ILogger<LocationIdDataRetrieverService> logger, IRightmoveService rightmoveService)
        {
            if (locationIdProcessorSettings.Value == null)
                throw new ArgumentNullException(nameof(locationIdProcessorSettings),
                    "LocationIdProcessorSettings must be provided");
            _logger = logger;
            _rightmoveService = rightmoveService;
            _locationIdProcessorSettings = locationIdProcessorSettings;
            InitRabbitMq();
        }

        private void InitRabbitMq()
        {
            _logger.LogInformation("Waiting for RabbitMQ to spin up");
            Thread.Sleep(new TimeSpan(0, 0, 0, 30));
            var factory = new ConnectionFactory
            {
                Uri = $"amqp://guest:guest@{_locationIdProcessorSettings.Value.RabbitMq.Host}/"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("new-locationid-retrieved", false, false, false, null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Creating connection");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = JsonConvert.DeserializeObject<RecordToProcess>(Encoding.UTF8.GetString(body));
                _rightmoveService.GetDataAsync(message.Postcode, message.LocationId)
                    .ConfigureAwait(true);
                Console.WriteLine($"Processed postcode {message.Postcode} and locationId {message.LocationId}");
            };
            _channel.BasicConsume(queue: "new-locationid-retrieved",
                false,
                consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
