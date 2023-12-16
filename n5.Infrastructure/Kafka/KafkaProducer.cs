using Confluent.Kafka;
using Newtonsoft.Json;

namespace n5.Infrastructure.Kafka;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(string bootstrapServers)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
            // Configuraciones adicionales según tus necesidades
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public void Produce<T>(string topic, T message)
    {
        // Serializar el mensaje a formato JSON o el que prefieras
        var serializedMessage = JsonConvert.SerializeObject(message);

        // Enviar el mensaje al tema
        _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedMessage });
    }
}