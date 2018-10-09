using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace LotterySystem.MessageBus
{
    public class KafkaProducer
    {
        private readonly Producer<Null, string> _producer;

        public KafkaProducer(Dictionary<string, object> config)
        {
            _producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8));
        }

        public KafkaProducer(): this(new Dictionary<string, object> {
            { "bootstrap.servers", "localhost:9092" }
        }) {}

        public async Task<Message<Null, string>> SendAsync(string topic, string message, int partition = -1)
        {
            Message<Null, string> responseMessage;

            if (partition < 0)
            {
                responseMessage = await _producer.ProduceAsync(topic, null, message);
            }
            else
            {
                responseMessage = await _producer.ProduceAsync(topic, null, message, partition);
            }

            if (responseMessage.Error.HasError)
                throw new KafkaException(responseMessage.Error);

            return responseMessage;
        }
    }
}
