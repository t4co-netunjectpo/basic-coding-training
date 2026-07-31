using System;
using DiTraining.Bad;

// ===== 実行コード（トップレベルステートメント） =====
var service = new NotificationService();
service.Notify("こんにちは");

// ===== クラス定義 =====

namespace DiTraining.Bad
{
    public class EmailSender
    {
        public void Send(string message) => Console.WriteLine($"[Email] {message}");
    }

    public class NotificationService
    {
        private readonly EmailSender _sender = new EmailSender();

        public void Notify(string message) => _sender.Send(message);
    }
}
