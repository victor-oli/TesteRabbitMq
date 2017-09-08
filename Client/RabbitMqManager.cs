using RabbitMQ.Client;
using System;
using System.Text;

namespace Client
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
                _model = _conn.CreateModel();
        }

        public void Send(string message)
        {
            _model.BasicPublish("music_exchange", "music", null, Encoding.UTF8.GetBytes(message));

            Console.WriteLine($"A mensagem: \"{message}\" foi publicada.");
        }

        public void Dispose()
        {
            _model.Dispose();
            _conn.Dispose();
            _connFactory = null;
        }
    }
}