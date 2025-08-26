using Confluent.Kafka;
using System.Net.NetworkInformation;

namespace Produtor.Config
{
    public class KafkaConfig
    {
        public static ProducerConfig GetProducerConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
            };
        }
    }
}
