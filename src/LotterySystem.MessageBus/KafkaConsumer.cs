using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace LotterySystem.MessageBus
{
    public class KafkaConsumer
    {
        private readonly Consumer<Null, string> _consumer;

        public KafkaConsumer(Dictionary<string, object> config)
        {
            _consumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8));
        }

        public KafkaConsumer(): this(new Dictionary<string, object> {
            {"group.id", $"{Guid.NewGuid()}"},
            {"bootstrap.servers", "localhost:9092"},
            {"enable.auto.commit", "false"},
            {"default.topic.config", new Dictionary<string, object>
                {
                    {"auto.offset.reset", "smallest"}
                }
            }
        }) {}

        public void SubscribeOn<T>(string[] topics, Action<T> action) where T: class 
        {
            _consumer.Subscribe(topics);

            _consumer.OnMessage += (_, msg) => {
                action(msg.Value as T);
                _consumer.CommitAsync();
            };

            while (true)
            {
                _consumer.Poll(100);
            }
        }
    }
}
