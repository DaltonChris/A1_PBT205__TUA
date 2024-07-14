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
        private readonly string queueName;
        private readonly string queryQueueName;
        private readonly string queryResponseExchange = "user.query.response";
        private readonly string ordersExchangeName = "orders"; // Added for trading app
        private readonly string tradesExchangeName = "trades"; // Added for trading app

        public RabbitMqController(string username, string password)
        {
            this.queueName = $"Positions_Queue_{username}";
            var factory = new ConnectionFactory() { HostName = "localhost",
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: exchangeName, 
                                        type: ExchangeType.Topic);
            channel.QueueDeclare(queue: queueName, durable: false,
                                exclusive: false,
                                autoDelete: true,
                                arguments: null);
            channel.QueueBind(queue: queueName,
                            exchange: exchangeName,
                            routingKey: _RoutingKey);

            channel.ExchangeDeclare(exchange: ordersExchangeName, type: ExchangeType.Fanout); // Added for trading app
            channel.ExchangeDeclare(exchange: tradesExchangeName, type: ExchangeType.Fanout); // Added for trading app
            channel.ExchangeDeclare(exchange: queryResponseExchange, type: ExchangeType.Fanout); // Added for trading app
        }

        public void PublishPosition(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: exchangeName, routingKey: _RoutingKey, basicProperties: null, body: body);
        }

        public void PublishQueryResponse(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: queryResponseExchange, routingKey: "query", basicProperties: null, body: body);
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

        public void SubscribeToQuery(Action<string> callback)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                callback(message);
            };
            channel.BasicConsume(queue: queryQueueName, autoAck: true, consumer: consumer);
        }

        // Trading app
        public void PublishOrder(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: ordersExchangeName, routingKey: "", basicProperties: null, body: body);
        }

        public void SubscribeToOrders(Action<string> callback)
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

        public void PublishTrade(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: tradesExchangeName, routingKey: "", basicProperties: null, body: body);
        }

        public void SubscribeToTrades(Action<string> callback)
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
