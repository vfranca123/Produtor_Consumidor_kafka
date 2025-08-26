using Confluent.Kafka;
using Produtor.Model;
using System.Text.Json;

namespace Produtor.Services
{
    public class KafkaService
    {
        private readonly ProducerConfig _producerConfig;
        private readonly IProducer<Null,string> producer;
        public KafkaService(ProducerConfig producerConfig)
        {
            _producerConfig = producerConfig;
            producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
        }
        public async Task SendMensage(Produto produto)
        {
            string json = JsonSerializer.Serialize(produto);

            try
            {
                var resultado = await producer.ProduceAsync("produtos", new Message<Null, string> { Value = json });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
            }
        }
    }
}
