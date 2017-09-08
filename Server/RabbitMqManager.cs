using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Server
{
    public class RabbitMqManager : IDisposable
    {
        private static ConnectionFactory _connFactory;
        private static IConnection _conn;
        private static IModel _model;

        public RabbitMqManager()
        {
            if (_connFactory == null)
                _connFactory = new ConnectionFactory
                {
                    UserName = "guest",
                    Password = "guest",
                    HostName = "localhost"
                };

            if (_conn == null)
                _conn = _connFactory.CreateConnection();

            if (_model == null)
            {
                _model = _conn.CreateModel();
                _model.BasicQos(0, 1, false);
            }
        }

        public void Start()
        {
            BasicGetResult basicGetResult = _model.BasicGet("music_queue", false);

            EventingBasicConsumer consumer = new EventingBasicConsumer(_model);

            consumer.Received += (o, e) =>
            {
                Console.WriteLine($"A mensagem {Encoding.UTF8.GetString(e.Body)} foi recebida!");

                _model.BasicAck(e.DeliveryTag, false);
            };

            string tag = _model.BasicConsume("music_queue", false, consumer);
        }

        public void Dispose()
        {
            _model.Dispose();
            _conn.Dispose();
            _connFactory = null;
        }
    }
}