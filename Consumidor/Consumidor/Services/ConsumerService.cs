
namespace Consumidor.Services
{
    public class ConsumerService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {

            });
        }
    }
}
