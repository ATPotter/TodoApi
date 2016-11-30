using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;

using DataServer.DataAccess.Interfaces;
using DataServer.Handlers; 

using DataObjects.RPCRequests;

using Newtonsoft.Json;

namespace DataServer
{
    class Program
    {

        static void Main(string[] args)
        {

            IServiceProvider Services;

            var serviceCollection = new ServiceCollection() as IServiceCollection;
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();

            var Handlers = new Dictionary<System.Type, IHandler>();

            //RegisterHandlers(Handlers);
            Handlers.Add(typeof(GetAllDto), new GetAllHandler(Services.GetRequiredService<ITodoStorage>()));
            Handlers.Add(typeof(AddItemDto), new AddItemHandler(Services.GetRequiredService<ITodoStorage>()));



            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "rpc_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                    channel.BasicQos(0, 1, false);


                    var consumer = new QueueingBasicConsumer(channel);



                    //consumer.Received += (model, ea) =>
                    //{
                    //    var body = ea.Body;
                    //    var message = Encoding.UTF8.GetString(body);
                    //    Console.WriteLine(" [x] Received {0}", message);
                    //};
                    channel.BasicConsume(queue: "rpc_queue",
                                         noAck: false,
                                         consumer: consumer);

                    while (true)
                    {
                        Console.WriteLine("Awaiting work request");

                        var response = null as string;
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        var body = ea.Body;

                        // Create a reply object, fill in the Correlation ID
                        var props = ea.BasicProperties;
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = props.CorrelationId;

                        try
                        {


                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine("Received message: {0}", message);

                            // Can we deserialize it?
                            var deserializedMessage = JsonConvert.DeserializeObject<RpcRequest<object>>(message);

                            if(Handlers.ContainsKey(deserializedMessage.ObjectType))
                            {
                                response = Handlers[deserializedMessage.ObjectType].HandleRequest(message);
                            }
                            else
                            {
                                Console.WriteLine("Unrecognised message type: {0}", message);

                                // Allow things to continue
                                response = "";
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(" [.] " + e.Message);
                            response = "";
                        }
                        finally
                        {
                            // Encode the response and send it back to the server
                            var responseBytes = Encoding.UTF8.GetBytes(response);
                            channel.BasicPublish(exchange: "",
                                routingKey: props.ReplyTo,
                                basicProperties: replyProps,
                                body: responseBytes);

                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                    }
                }
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ITodoStorage, DataAccess.Implementations.Postgres.PgToDoStorage>();

        }
    }
}
