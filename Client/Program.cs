using System;

namespace Client
{
    class Program
    {
        private static RabbitMqManager _manager;
        static void Main(string[] args)
        {
            _manager = new RabbitMqManager();

            Console.WriteLine(@"Digite uma mensagem ou \quit para sair.");

            while (true)
            {
                string message = Console.ReadLine();

                if (message.Equals(@"\quit"))
                {
                    _manager.Dispose();

                    break;
                }
                else if (!string.IsNullOrWhiteSpace(message))
                {
                    _manager.Send(message);
                }
                else
                {
                    _manager.Dispose();

                    break;
                }
            }
        }
    }
}