using CQELight.Buses.MSMQ.Common;
using System.Messaging;

namespace CQELight.Buses.MSMQ
{
    internal static class Helpers
    {

        public static MessageQueue GetMessageQueue(string queueName = "")
        {
            var queue = string.IsNullOrWhiteSpace(queueName) ? Consts.CONST_QUEUE_NAME : queueName;
            MessageQueue messageQueue;
            if (!MessageQueue.Exists(queue))
            {
                messageQueue = MessageQueue.Create(queue);
            }
            else
            {
                messageQueue = new MessageQueue(queue);
            }

            messageQueue.Formatter = new JsonMessageFormatter();
            return messageQueue;
        }

    }
}
