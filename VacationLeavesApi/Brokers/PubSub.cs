using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace VacationLeavesApi.Brokers;

public class PubSub:IPubSub
{
    public void SendMessage(string mssg)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "vacation",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                var body = Encoding.UTF8.GetBytes(mssg);
                channel.BasicPublish(exchange: string.Empty, routingKey: "vacation", basicProperties: null, body: body);
            }
        }
    }
    public string RecieveMessage()
    {
        string message = "";
        var factory = new ConnectionFactory { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "vacation",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, e) =>
                {
                    var body = e.Body.ToArray();
                    message = Encoding.UTF8.GetString(body);
                };
                channel.BasicConsume(
                    queue: "vacation",
                    autoAck: true,
                    consumer: consumer);
            }
        }
        return message;
    }
}
