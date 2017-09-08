using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteRabbitMq
{
    class Program
    {
        static string exchangeMusic = "music_exchange";
        static string exchangeGames = "game_exchange";
        static string queueMusic = "music_queue";
        static string queueGames = "game_queue";

        private static event Action<string> EventTest;

        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost"
            };

            EventTest += (msg) =>
            {
                Console.WriteLine(msg);
            };

            using (var conn = factory.CreateConnection())
            {
                using (var model = conn.CreateModel())
                {
                    model.ExchangeDeclare(exchangeMusic, ExchangeType.Topic, true, false, null);
                    model.ExchangeDeclare(exchangeGames, ExchangeType.Topic, true, false, null);

                    model.QueueDeclare(queueMusic, true, false, false, null);
                    model.QueueDeclare(queueGames, true, false, false, null);

                    model.QueueBind(queueMusic, exchangeMusic, "music", null);
                    model.QueueBind(queueGames, exchangeGames, "game", null);

                    EventTest?.Invoke("deu bom!");

                    //model.BasicPublish(exchangeMusic, "music", null, Encoding.UTF8.GetBytes("Matanza"));
                    //model.BasicPublish(exchangeGames, "game", null, Encoding.UTF8.GetBytes($"The Witcher III"));
                }
            }

            Console.ReadLine();
        }
    }
}