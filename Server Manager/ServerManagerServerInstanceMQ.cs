﻿using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Manager
{
    public class ServerManagerServerInstanceMQ
    {
        IConnection connection;
        IModel channel;
        EventingBasicConsumer consumer;
        string queueName;

        public ServerManagerServerInstanceMQ(string hostIPv4 = "localhost", string queueName = "server_manager_server_instance_queue")
        {
            this.queueName = queueName;

            ConnectionFactory factory = new ConnectionFactory() { HostName = hostIPv4, UserName = "gruppe5", Password = "123" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queueName, false, false, false, null);
            channel.BasicConsume(queueName, false, consumer);

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                string msg = Encoding.UTF8.GetString(body);
                JObject json = JObject.Parse(msg);
                switch (json["function"].ToString())
                {
                    case "newQueue":
                        LoadBalancer.SingletonInstance.AddInfoToNewServerInstance(json["queueName"].ToString(), json["ip"].ToString());
                        break;
                }
            };

            channel.BasicConsume(consumer, queueName, true);
        }

        public void Call(string message)
        {
            byte[] msg = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("ServerManagerServerInstance", queueName, null, msg);
        }
    }
}
