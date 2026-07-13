using System;
using System.Collections.Generic;
using System.Linq;
using SolidSample.Good.DataAccess;
using SolidSample.Good.Formatters;
using SolidSample.Good.Output;

namespace SolidSample.Good.Services;

public class ReportService
{
    private readonly ISalesRepository _repository;
    private readonly IEnumerable<IReportFormatter> _formatters;
    private readonly IFileWriter _fileWriter;
    private readonly IEmailSender _emailSender;
    private readonly IConsoleWriter _consoleWriter;

    public ReportService(
        ISalesRepository repository,
        IEnumerable<IReportFormatter> formatters,
        IFileWriter fileWriter,
        IEmailSender emailSender,
        IConsoleWriter consoleWriter)
    {
        _repository = repository;
        _formatters = formatters;
        _fileWriter = fileWriter;
        _emailSender = emailSender;
        _consoleWriter = consoleWriter;
    }

    private string BuildReport(string formatName)
    {
        var records = _repository.GetAll();
        var formatter = _formatters.FirstOrDefault(f => f.FormatName.Equals(formatName, StringComparison.OrdinalIgnoreCase));
        if (formatter == null)
        {
            throw new KeyNotFoundException($"Formatter for format '{formatName}' was not found.");
        }
        return formatter.Format(records);
    }

    public void PrintToConsole(string formatName)
    {
        var content = BuildReport(formatName);
        _consoleWriter.Write(content);
    }

    public void SaveToFile(string formatName, string filePath)
    {
        var content = BuildReport(formatName);
        _fileWriter.Write(filePath, content);
    }

    public void SendByEmail(string formatName, string to)
    {
        var content = BuildReport(formatName);
        _emailSender.Send(to, "Sales Report", content);
    }
}
