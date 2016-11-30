using System;
using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using DataObjects.RPCRequests;
using Newtonsoft.Json;

namespace TodoApi.Models
{
    public class RpcClient
    {
        private IConnection connection;
        private IModel channel;
        private string replyQueueName;
        private QueueingBasicConsumer consumer;

        public RpcClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queue: replyQueueName,
                                 noAck: true,
                                 consumer: consumer);
        }

        public Toutput Call<Tinput, Toutput>(RpcRequest<Tinput> request)
        {
            // Serialize the request
            var message = JsonConvert.SerializeObject(request);

            var corrId = Guid.NewGuid().ToString();
            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;
            props.CorrelationId = corrId;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                                 routingKey: "rpc_queue",
                                 basicProperties: props,
                                 body: messageBytes);

            while (true)
            {
                var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                if (ea.BasicProperties.CorrelationId == corrId)
                {
                    return JsonConvert.DeserializeObject<Toutput>(Encoding.UTF8.GetString(ea.Body));
                }
            }
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
