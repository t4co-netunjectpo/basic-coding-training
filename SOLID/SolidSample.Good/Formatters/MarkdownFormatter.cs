using System.Collections.Generic;
using System.Text;
using SolidSample.Good.Domain;

namespace SolidSample.Good.Formatters;

public class MarkdownFormatter : IReportFormatter
{
    public string FormatName => "Markdown";

    public string Format(IReadOnlyList<SalesRecord> records)
    {
        var sb = new StringBuilder();
        sb.AppendLine("| Product | Amount |");
        sb.AppendLine("|---------|--------|");
        foreach (var record in records)
        {
            sb.AppendLine($"| {record.Product} | {record.Amount} |");
        }
        return sb.ToString();
    }
}
