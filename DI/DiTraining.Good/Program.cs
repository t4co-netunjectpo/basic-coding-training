using System;
using DiTraining.Good;

// ===== 実行コード（トップレベルステートメント） =====
var emailService = new NotificationService(new EmailSender());
emailService.Notify("こんにちは（メール）");

var smsService = new NotificationService(new SmsSender());
smsService.Notify("こんにちは（SMS）");

// ===== インターフェースおよびクラス定義 =====

namespace DiTraining.Good
{
    public interface IMessageSender
    {
        void Send(string message);
    }

    public class EmailSender : IMessageSender
    {
        public void Send(string message) => Console.WriteLine($"[Email] {message}");
    }

    public class SmsSender : IMessageSender
    {
        public void Send(string message) => Console.WriteLine($"[SMS] {message}");
    }

    public class NotificationService
    {
        private readonly IMessageSender _sender;

        public NotificationService(IMessageSender sender) => _sender = sender;

        public void Notify(string message) => _sender.Send(message);
    }
}
