using System;
using SolidSample.Bad;

Console.WriteLine("=== SOLID Sample (Bad Pattern) Running ===");

var manager = new ReportManager();

// 正常系ケース 1: CSV to Console
Console.WriteLine("\n--- Case 1: CSV to Console ---");
manager.GenerateAndDeliverReport("CSV", "Console", null);

// 正常系ケース 2: JSON to File
Console.WriteLine("\n--- Case 2: JSON to File ---");
manager.GenerateAndDeliverReport("JSON", "File", null);
Console.WriteLine("Check report.txt for JSON output.");

// 正常系ケース 3: CSV to Email
Console.WriteLine("\n--- Case 3: CSV to Email ---");
manager.GenerateAndDeliverReport("CSV", "Email", "user@example.com");

// 異常系・LSP違反ケース (PDF to Console)
// 以下のコードは例外を発生させるため、動作確認後にコメントアウトします。
// Console.WriteLine("\n--- Case 4: PDF to Console (LSP Violation Test) ---");
// try
// {
//     manager.GenerateAndDeliverReport("PDF", "Console", null);
// }
// catch (NotSupportedException ex)
// {
//     Console.WriteLine($"Caught expected LSP violation exception: {ex.Message}");
// }
