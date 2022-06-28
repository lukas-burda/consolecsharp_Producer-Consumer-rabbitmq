using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace MensageriaISSDeconto_C_
{
    class Mensagem
    {
        public int id { get; set; }
        public string body { get; set; }
        public DateTime createdAt { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            try
            {
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        var queue = "mensagens";
                        channel.QueueDeclare(
                            queue: queue,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null
                            );

                        string mensagem = JsonConvert.SerializeObject(
                            new Mensagem()
                            {
                                id = 1,
                                body = "mensagem",
                                createdAt = DateTime.Now
                            });

                        Console.WriteLine(mensagem);

                        bool repeat = false;
                        if (Console.ReadKey().Key.ToString() == "Enter")
                        {
                            repeat = true;
                        }
                        var cont = 1;
                        while (repeat)
                        {
                            byte[] bytes = Encoding.UTF8.GetBytes(mensagem);

                            channel.BasicPublish(
                                body: bytes,
                                routingKey: queue,
                                basicProperties: null,
                                exchange: ""
                                );

                            cont++;
                            Console.WriteLine("mensagem enviada"+$" {cont}");
                            
                            //if (Console.ReadKey().Key.ToString() == "Enter")
                            //{
                            //    repeat = true;
                            //}
                            //else
                            //{
                            //    repeat = false;
                            //}
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
