using System;
using Microsoft.Extensions.DependencyInjection;
using DiTraining.DiContainer;

// ===== 実行コード（トップレベルステートメント） =====

// 1. サービスを登録
var services = new ServiceCollection();
services.AddTransient<IMessageSender, EmailSender>();
services.AddTransient<NotificationService>();

// 2. コンテナを構築
var provider = services.BuildServiceProvider();

// 3. サービスを取得
var service = provider.GetRequiredService<NotificationService>();
service.Notify("こんにちは（DIコンテナ経由）");

// ===== インターフェースおよびクラス定義 =====

namespace DiTraining.DiContainer
{
    public interface IMessageSender
    {
        void Send(string message);
    }

    public class EmailSender : IMessageSender
    {
        public void Send(string message) => Console.WriteLine($"[Email] {message}");
    }

    public class NotificationService
    {
        private readonly IMessageSender _sender;

        public NotificationService(IMessageSender sender) => _sender = sender;

        public void Notify(string message) => _sender.Send(message);
    }
}
