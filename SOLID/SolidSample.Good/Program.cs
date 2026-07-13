using System;
using Microsoft.Extensions.DependencyInjection;
using SolidSample.Good.DataAccess;
using SolidSample.Good.Formatters;
using SolidSample.Good.Output;
using SolidSample.Good.Services;

Console.WriteLine("=== SOLID Sample (Good Pattern) Running ===");

// 1. Composition Root: DIコンテナの初期化と依存関係登録
var services = new ServiceCollection();

// DataAccess
services.AddSingleton<ISalesRepository, InMemorySalesRepository>();

// Formatters (複数登録)
services.AddSingleton<IReportFormatter, CsvFormatter>();
services.AddSingleton<IReportFormatter, JsonFormatter>();
services.AddSingleton<IReportFormatter, MarkdownFormatter>();

// Output
services.AddSingleton<IFileWriter, FileWriter>();
services.AddSingleton<IEmailSender, SmtpEmailSender>();
services.AddSingleton<IConsoleWriter, ConsoleWriter>();

// Services
services.AddSingleton<ReportService>();

// コンテナ構築
using var serviceProvider = services.BuildServiceProvider();

// 2. サービスの解決とデモ実行
var service = serviceProvider.GetRequiredService<ReportService>();

// デモ 1: CSV to Console
Console.WriteLine("\n--- Case 1: CSV to Console ---");
service.PrintToConsole("CSV");

// デモ 2: JSON to File
Console.WriteLine("\n--- Case 2: JSON to File ---");
var filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "good_report.json");
service.SaveToFile("JSON", filePath);
Console.WriteLine("Check report.txt for JSON output.");

// デモ 3: Markdown to Email
Console.WriteLine("\n--- Case 3: CSV to Email ---");
service.SendByEmail("CSV", "user@example.com");
Console.WriteLine();
