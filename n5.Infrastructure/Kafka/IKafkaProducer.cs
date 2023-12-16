namespace n5.Infrastructure.Kafka;

public interface IKafkaProducer
{
    void Produce<T>(string topic, T message);
}