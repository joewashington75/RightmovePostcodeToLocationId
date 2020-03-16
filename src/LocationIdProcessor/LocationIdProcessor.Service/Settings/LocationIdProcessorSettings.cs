namespace RightmovePostcodeToLocationId.LocationIdProcessor.Service.Settings
{
    public class LocationIdProcessorSettings
    {
        public RabbitMQSettings RabbitMq { get; set; }
        public string SqlConnectionString { get; set; }
    }
}
