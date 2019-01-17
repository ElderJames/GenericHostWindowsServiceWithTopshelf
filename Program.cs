using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Topshelf;

namespace GenericHostWindowsServiceWithTopshelf
{
    class Program
    {
        private static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IHostLifetime, TopshelfLifetime>();
                    services.AddHostedService<FileWriterService>();
                });

            HostFactory.Run(x =>
            {
                x.SetServiceName("GenericHostWindowsServiceWithTopshelf");
                x.SetDisplayName("Topshelf创建的Generic Host服务");
                x.SetDescription("运行Topshelf创建的Generic Host服务");

                x.Service<IHost>(s =>
                {
                    s.ConstructUsing(() => builder.Build());
                    s.WhenStarted(service =>
                    {
                        service.Start();
                    });
                    s.WhenStopped(service =>
                    {
                        service.StopAsync();
                    });
                });
            });
        }
    }
}
