﻿using Azure.Storage.Queues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerTask.Services
{
    public class QueueService : IQueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService(string connectionString, string queueName)
        {
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task<string> ReceiveMessageAsync()
        {
            var messageResponse = await _queueClient.ReceiveMessageAsync();
            if (messageResponse.Value != null)
            {
                string message = messageResponse.Value.Body.ToString();

                await _queueClient.DeleteMessageAsync(messageResponse.Value.MessageId, messageResponse.Value.PopReceipt);

                return message;
            }
            return "";
        }

        public async Task SendMessageAsync(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                await _queueClient.SendMessageAsync(message, TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(10));
            }
        }

        public async Task SendMessageWithCountAsync(string message, int count)
        {
            for (int i = 0; i < count; i++)
            {
                await SendMessageAsync(message);
            }
        }
    }
}
