using System.Collections.Generic;
using System.Text.Json;
using SolidSample.Good.Domain;

namespace SolidSample.Good.Formatters;

public class JsonFormatter : IReportFormatter
{
    public string FormatName => "JSON";

    public string Format(IReadOnlyList<SalesRecord> records)
    {
        return JsonSerializer.Serialize(records);
    }
}
