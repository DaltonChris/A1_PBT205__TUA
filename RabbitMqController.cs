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
        private readonly string _QueryResponseExchange = "user_querys";

        public RabbitMqController(string username, string password) // Contructor
        {
            this._QueueName = $"Positions_Queue_{username}";
            this._QueryQueueName = $"Query_Queue_{username}";
            var factory = new ConnectionFactory() { HostName = "localhost",
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
            _Channel.ExchangeDeclare(exchange: _QueryResponseExchange,
                            type: ExchangeType.Topic);
            _Channel.QueueDeclare(queue: _QueryQueueName, durable: false,
                                exclusive: false,
                                autoDelete: true,
                                arguments: null);
            _Channel.QueueBind(queue: _QueryQueueName,
                            exchange: _QueryResponseExchange,
                            routingKey: _RoutingKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void PublishPosition(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _Channel.BasicPublish(exchange: _ExchangeName, routingKey: _RoutingKey, basicProperties: null, body: body);
        }


        public void PublishQuery(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _Channel.BasicPublish(exchange: _QueryResponseExchange, routingKey: _RoutingKey, basicProperties: null, body: body);
        }

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

        public void Close()
        {
            _Channel.Close();
            _Connection.Close();
        }
    }
}
