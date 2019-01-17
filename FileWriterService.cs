using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenericHostWindowsServiceWithTopshelf
{
    internal class FileWriterService : IHostedService, IDisposable
    {
        private static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"test.txt");

        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return Task.FromCanceled(cancellationToken);

            _timer = new Timer(
                (e) => WriteTimeToFile(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        public void WriteTimeToFile()
        {
            if (!File.Exists(path))
            {
                using (var sw = File.CreateText(path))
                {
                    sw.WriteLine(DateTime.Now);
                }
            }
            else
            {
                using (var sw = File.AppendText(path))
                {
                    sw.WriteLine(DateTime.Now);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}