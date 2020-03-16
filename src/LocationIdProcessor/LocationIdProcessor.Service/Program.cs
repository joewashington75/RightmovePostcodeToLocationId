using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RightmovePostcodeToLocationId.LocationIdProcessor.Service.Settings;

namespace RightmovePostcodeToLocationId.LocationIdProcessor.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            var defaultBuilder = Host.CreateDefaultBuilder(args);

            defaultBuilder
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    var os = hostBuilderContext.Configuration.GetValue<string>("OperatingSystem");
                    if (os.Equals("Windows"))
                    {
                        defaultBuilder
                            .UseWindowsService();
                    }
                    else
                    {
                        defaultBuilder
                            .UseSystemd();
                    }

                    var settings = hostBuilderContext.Configuration.GetSection("LocationIdProcessorSettings");
                    serviceCollection.Configure<LocationIdProcessorSettings>(settings);
                    serviceCollection.AddHostedService<LocationIdDataRetrieverService>();
                });

            return defaultBuilder;
        }
    }
}
