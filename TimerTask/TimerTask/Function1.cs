using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TimerTask.Services;

namespace TimerTask
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly IQueueService _queueService;

        public Function1(ILoggerFactory loggerFactory, IQueueService queueService)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _queueService = queueService;
        }

        [Function("SendMessageTimer")]
        public async Task SendMessageTimer(
            [TimerTrigger("*/2 * * * * *")] TimerInfo myTimer,
            ILogger log)
        {
            string product = "Product: " + Guid.NewGuid().ToString();
            await _queueService.SendMessageAsync(product);
            log.LogInformation($"[SEND] {product} gonderildi - {DateTime.Now}");
        }

        [Function("ReadAndDeleteMessagesTimer")]
        public async Task ReadAndDeleteMessagesTimer(
            [TimerTrigger("*/6 * * * * *")] TimerInfo myTimer,
            ILogger log)
        {
            while (true)
            {
                var message = await _queueService.ReceiveMessageAsync();
                if (string.IsNullOrEmpty(message)) break;

                log.LogInformation($"[READ] Oxunan mesaj: {message}");
            }

            log.LogInformation($"[READ] mesajlar bitdi ve hamisi silindi - {DateTime.Now}");
        }
    }
}
