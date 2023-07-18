// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.AspNetCore.Mvc;

namespace api_consumer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageConsumerController : ControllerBase
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageConsumerController()
        {
            // Configure RabbitMQ connection
            _connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "user",
                Password = "password",
                VirtualHost = "/"
            };

            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare the queue
            _channel.QueueDeclare("Buying", durable: true, exclusive: false, autoDelete: false);

            // Set up the event handler for received messages
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Handle the received message here (e.g., process, store, etc.)
                // You can also call methods to update your database, trigger other actions, etc.
                System.Console.WriteLine($"Received message: {message}");
            };

            // Start consuming messages from the queue
            _channel.BasicConsume("Buying", true, consumer);
        }

        // You can add other API endpoints if needed for your application
        // For example, you might want to expose additional functionality related to message handling.

        // Remember to dispose of the resources when the application shuts down.
        // For simplicity, you can add a Dispose method and call it when the application stops.
        // For production use, consider implementing proper lifecycle management.
        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
        [HttpGet]
        public void GetAsync()
        {
            throw new NotImplementedException();
        }
    }
}