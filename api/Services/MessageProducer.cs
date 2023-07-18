using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace api.Services
{
    public class MessageProducer : IMessageProducer
    {
        public void SendingMessage<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "password",
                VirtualHost = "/"
            };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declare the queue consistently with the 'exclusive' property
            channel.QueueDeclare("Buying2", durable: true, exclusive: false);


            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish("", "Buying2", body: body);
        }
    }
}