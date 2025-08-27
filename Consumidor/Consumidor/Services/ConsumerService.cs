using Confluent.Kafka;
using Consumidor.Model;

namespace Consumidor.Services
{
    public class ConsumerService
    {
        private readonly ConsumerConfig _config;
        private readonly IConsumer<Ignore, string> _consumer;
        public ConsumerService(ConsumerConfig config)
        {
            _config = config;
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
        }
        public Task ExecuteAsync(CancellationToken stopingToken,string nome)
        {
            return Task.Run(() =>
            {
                _consumer.Subscribe(nome);
                try
                {
                    while (!stopingToken.IsCancellationRequested)
                    {
                        var cr = _consumer.Consume(stopingToken);
                        Console.WriteLine(cr.Message.Value);
                    }
                }catch(OperationCanceledException)
                {
                    _consumer.Close();
                }
            },stopingToken);
        }
    }
}
