using System.Collections.Generic;
using System.Text;
using SolidSample.Good.Domain;

namespace SolidSample.Good.Formatters;

public class CsvFormatter : IReportFormatter
{
    public string FormatName => "CSV";

    public string Format(IReadOnlyList<SalesRecord> records)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Product,Amount");
        foreach (var record in records)
        {
            sb.AppendLine($"{record.Product},{record.Amount}");
        }
        return sb.ToString();
    }
}
