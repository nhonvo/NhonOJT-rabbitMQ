// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Hello, World!");

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


var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    System.Console.WriteLine($"A message has been received - {message}");
};

channel.BasicConsume("Buying2", true, consumer);

Console.ReadKey();