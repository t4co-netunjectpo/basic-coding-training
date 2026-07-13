using System.Collections.Generic;
using SolidSample.Good.Domain;

namespace SolidSample.Good.Formatters;

public interface IReportFormatter
{
    string FormatName { get; }
    string Format(IReadOnlyList<SalesRecord> records);
}
