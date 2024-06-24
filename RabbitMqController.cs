using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Text;

namespace PBT_205_A1
{
    /// <summary>
    /// Probs best we make a class for rabbit for each other task/app to referance rather than ea one having init code
    /// </summary>
    class RabbitMqController
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string exchangeName = "user.positions";
        private readonly string routingKey;
        private readonly string username;
        private readonly string queueName;

        public RabbitMqController(string username, string roomName,string routingKey)
        {
            this.username = username;
            this.queueName = $"Positions_Queue_{roomName}_{username}";
            this.routingKey = routingKey;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: true, arguments: null);
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);
        }

        public void PublishPosition(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: body);
        }

        public void SubscribeToPositions(Action<string> callback)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                callback(message);
            };
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void Close()
        {
            channel.Close();
            connection.Close();
        }
    }
}
