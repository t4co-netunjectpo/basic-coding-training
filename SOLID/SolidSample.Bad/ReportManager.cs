using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SolidSample.Bad;

public class ReportManager
{
    private readonly string _connectionString = "Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;";
    private readonly string _smtpServer = "smtp.mymailserver.com";

    public void GenerateAndDeliverReport(string format, string outputTarget, string? emailTo)
    {
        // 1. データ取得処理
        var data = GetSalesData();

        // 2. フォーマット整形処理 (SRP / OCP / LSP 違反の再現)
        string reportText = "";
        if (format.Equals("CSV", StringComparison.OrdinalIgnoreCase))
        {
            var sb = new StringBuilder();
            sb.AppendLine("Product,Amount");
            foreach (var item in data)
            {
                sb.AppendLine($"{item.Product},{item.Amount}");
            }
            reportText = sb.ToString();
        }
        else if (format.Equals("JSON", StringComparison.OrdinalIgnoreCase))
        {
            reportText = JsonSerializer.Serialize(data, new JsonSerializerOptions { IncludeFields = true });
            var filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "bad_report.json");
            System.IO.File.WriteAllText(filePath, reportText);
        }
        else if (format.Equals("PDF", StringComparison.OrdinalIgnoreCase))
        {
            var sb = new StringBuilder();
            sb.AppendLine("PDF Report");
            foreach (var item in data)
            {
                sb.AppendLine($"{item.Product}: {item.Amount}");
            }
            reportText = sb.ToString();
        }
        else
        {
            throw new ArgumentException("Unsupported format");
        }

        // 3. LSP違反の組み込み (PDF to Consoleの場合に例外スロー)
        if (format.Equals("PDF", StringComparison.OrdinalIgnoreCase) && outputTarget.Equals("Console", StringComparison.OrdinalIgnoreCase))
        {
            throw new NotSupportedException("PDF output to Console is not supported.");
        }

        // 4. 出力先分岐処理 (DIP / SRP 違反)
        if (outputTarget.Equals("Console", StringComparison.OrdinalIgnoreCase))
        {
            Console.Write(reportText);
        }
        else if (outputTarget.Equals("File", StringComparison.OrdinalIgnoreCase))
        {
            // 直接ローカルファイルに書き出す (SRP違反: 外部リソース操作)
            var filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "report.txt");
            // カレントディレクトリに書き出す場合
            filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "report.txt");
            System.IO.File.WriteAllText(filePath, reportText);
        }
        else if (outputTarget.Equals("Email", StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrEmpty(emailTo))
            {
                throw new ArgumentException("Email recipient is required for Email output.");
            }
            // 直接SMTP送信を想定した処理 (SRP / DIP 違反: 接続文字列やsmtpServerを直接参照)
            Console.Write($"Email sent to {emailTo} via {_smtpServer}. Subject: Sales Report. Body: {reportText}");
            // DIP違反をより再現するために_connectionStringも何かに使っておく（警告抑制も兼ねて）
            var connStr = _connectionString;
        }
        else
        {
            throw new ArgumentException("Unsupported output target");
        }
    }

    private List<(string Product, decimal Amount)> GetSalesData()
    {
        return new List<(string Product, decimal Amount)>
        {
            ("Apple", 100m),
            ("Banana", 200m),
            ("Cherry", 300m)
        };
    }
}
