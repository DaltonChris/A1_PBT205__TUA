using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Text;

namespace PBT_205_A1
{
    /// <summary>
    /// Probs best we make a class for rabbit for each other task/app to referance rather than ea one having init code
    /// </summary>
    public class RabbitMqController
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string exchangeName = "user_positions";
        private readonly string _RoutingKey = "position_room";
        private readonly string username;
        private readonly string queueName;

        public RabbitMqController(string username, string roomName)
        {
            this.username = username;
            this.queueName = $"Positions_Queue_{username}";
            var factory = new ConnectionFactory() { HostName = "localhost",                
                                                    UserName = username
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: true, arguments: null);
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: _RoutingKey);
        }

        public void PublishPosition(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: exchangeName, routingKey: _RoutingKey, basicProperties: null, body: body);
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

        public void SubscribeToQueryResponse(string responseQueueName, Action<string> callback)
        {
            channel.QueueDeclare(queue: responseQueueName, durable: false, exclusive: false, autoDelete: true, arguments: null);
            channel.QueueBind(queue: responseQueueName, exchange: exchangeName, routingKey: "query-response");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                callback(message);
            };
            channel.BasicConsume(queue: responseQueueName, autoAck: true, consumer: consumer);
        }

        public void Close()
        {
            channel.Close();
            connection.Close();
        }
    }
}
