using Confluent.Kafka;
namespace Consumidor.config
{
    public class ConsumerConfiguration
    {
        public static ConsumerConfig getConsumerCofig()
        {
            return new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "grupo1",
                AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest //Por onde vai começar a consumir as mensagens, se não houver offset salvo, ou seja, se for a primeira vez que o consumidor está consumindo o tópico.
            };
        }
    }
}