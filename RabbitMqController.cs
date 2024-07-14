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
        private readonly IConnection _Connection;
        private readonly IModel _Channel;
        private readonly string _ExchangeName = "user_positions";
        private readonly string _RoutingKey = "position_room";
        private readonly string _QueueName;
        private readonly string _QueryQueueName;
        private readonly string _QueryExchange = "user_querys";

        private readonly string queueName;
        private readonly string ordersExchangeName = "orders"; // Added for trading app
        private readonly string tradesExchangeName = "trades"; // Added for trading app

        public RabbitMqController(string username, string password) // Contructor
        {
            this._QueueName = $"Positions_Queue_{username}";
            this._QueryQueueName = $"Query_Queue_{username}";
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
            };
            _Connection = factory.CreateConnection();
            _Channel = _Connection.CreateModel();

            // Position exchange / Queue
            _Channel.ExchangeDeclare(exchange: _ExchangeName,
                                        type: ExchangeType.Topic);
            _Channel.QueueDeclare(queue: _QueueName, durable: false,
                                exclusive: false,
                                autoDelete: true,
                                arguments: null);
            _Channel.QueueBind(queue: _QueueName,
                            exchange: _ExchangeName,
                            routingKey: _RoutingKey);

            //Query topic/queue
            _Channel.ExchangeDeclare(exchange: _QueryExchange,
                            type: ExchangeType.Topic);
            _Channel.QueueDeclare(queue: _QueryQueueName, durable: false,
                                exclusive: false,
                                autoDelete: true,
                                arguments: null);
            _Channel.QueueBind(queue: _QueryQueueName,
                            exchange: _QueryExchange,
                            routingKey: _RoutingKey);

            channel.ExchangeDeclare(exchange: ordersExchangeName, type: ExchangeType.Fanout); // Added for trading app
            channel.ExchangeDeclare(exchange: tradesExchangeName, type: ExchangeType.Fanout); // Added for trading app
            channel.ExchangeDeclare(exchange: queryResponseExchange, type: ExchangeType.Fanout); // Added for trading app

        }

        /// <summary>
        /// Method to publish a new position msg (also grid size update)
        /// </summary>
        /// <param name="message"> the position </param>
        public void PublishPosition(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _Channel.BasicPublish(exchange: _ExchangeName, routingKey: _RoutingKey, basicProperties: null, body: body);
        }

        /// <summary>
        /// Method to publish to the query exchange
        /// </summary>
        /// <param name="message"> the query </param>
        public void PublishQuery(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _Channel.BasicPublish(exchange: _QueryExchange, routingKey: _RoutingKey, basicProperties: null, body: body);
        }

        /// <summary>
        /// Method to subscribe to the postion queue/ exchange
        /// </summary>
        /// <param name="callback"></param>
        public void SubscribeToPositions(Action<string> callback)
        {
            var consumer = new EventingBasicConsumer(_Channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                callback(message);
            };
            _Channel.BasicConsume(queue: _QueueName, autoAck: true, consumer: consumer);
        }

        /// <summary>
        /// Method to subscribe to the query queue/ exchange
        /// </summary>
        /// <param name="callback"></param>
        public void SubscribeToQuery(Action<string> callback)
        {
            var consumer = new EventingBasicConsumer(_Channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                callback(message);
            };
            _Channel.BasicConsume(queue: _QueryQueueName, autoAck: true, consumer: consumer);
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

        /// <summary>
        /// Closes connection.
        /// </summary>
        public void Close()
        {
            _Channel.Close();
            _Connection.Close();
        }
    }
}
