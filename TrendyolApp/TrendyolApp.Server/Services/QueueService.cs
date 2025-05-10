using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;

namespace TrendyolApp.Server.Services
{
    public class QueueService:IQueueService
    {
        private readonly QueueClient _queueClient;
        private readonly Dictionary<string, int> _discountReadCounts = new();

        public QueueService(string connectionString, string queueName)
        {
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task<string?> GetDiscountCodeAsync()
        {
            QueueMessage[] messages = await _queueClient.ReceiveMessagesAsync(1, TimeSpan.FromSeconds(1));
            if (messages.Length == 0) return null;

            var message = messages[0];
            string messageText = message.MessageText;

            if (_discountReadCounts.ContainsKey(message.MessageId))
                _discountReadCounts[message.MessageId]++;
            else
                _discountReadCounts[message.MessageId] = 1;

            if (_discountReadCounts[message.MessageId] >= 10)
            {
                await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                _discountReadCounts.Remove(message.MessageId);
                return null;
            }

            await _queueClient.UpdateMessageAsync(message.MessageId, message.PopReceipt, messageText, TimeSpan.Zero);

            return messageText;
        }
    }
}
