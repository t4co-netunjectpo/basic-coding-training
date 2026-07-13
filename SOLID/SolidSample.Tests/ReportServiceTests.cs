using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using SolidSample.Good.DataAccess;
using SolidSample.Good.Domain;
using SolidSample.Good.Formatters;
using SolidSample.Good.Output;
using SolidSample.Good.Services;
using Xunit;

namespace SolidSample.Tests;

public class ReportServiceTests
{
    private readonly Mock<ISalesRepository> _repositoryMock;
    private readonly Mock<IReportFormatter> _formatterMock;
    private readonly Mock<IFileWriter> _fileWriterMock;
    private readonly Mock<IEmailSender> _emailSenderMock;
    private readonly Mock<IConsoleWriter> _consoleWriterMock;
    private readonly List<SalesRecord> _dummyRecords;
    private readonly ReportService _service;

    public ReportServiceTests()
    {
        _repositoryMock = new Mock<ISalesRepository>();
        _formatterMock = new Mock<IReportFormatter>();
        _fileWriterMock = new Mock<IFileWriter>();
        _emailSenderMock = new Mock<IEmailSender>();
        _consoleWriterMock = new Mock<IConsoleWriter>();

        _dummyRecords = new List<SalesRecord> { new SalesRecord("TestProduct", 500m) };
        _repositoryMock.Setup(r => r.GetAll()).Returns(_dummyRecords);
        _formatterMock.Setup(f => f.FormatName).Returns("TEST");
        _formatterMock.Setup(f => f.Format(_dummyRecords)).Returns("FormattedData");

        var formatters = new List<IReportFormatter> { _formatterMock.Object };

        _service = new ReportService(
            _repositoryMock.Object,
            formatters,
            _fileWriterMock.Object,
            _emailSenderMock.Object,
            _consoleWriterMock.Object
        );
    }

    [Fact]
    public void PrintToConsole_ShouldFormatAndWriteToConsole()
    {
        // Act
        _service.PrintToConsole("TEST");

        // Assert
        _consoleWriterMock.Verify(c => c.Write("FormattedData"), Times.Once);
    }

    [Fact]
    public void SaveToFile_ShouldFormatAndWriteToFile()
    {
        // Act
        _service.SaveToFile("TEST", "test.txt");

        // Assert
        _fileWriterMock.Verify(f => f.Write("test.txt", "FormattedData"), Times.Once);
    }

    [Fact]
    public void SendByEmail_ShouldFormatAndSendEmail()
    {
        // Act
        _service.SendByEmail("TEST", "recipient@example.com");

        // Assert
        _emailSenderMock.Verify(e => e.Send("recipient@example.com", "Sales Report", "FormattedData"), Times.Once);
    }

    [Fact]
    public void PrintToConsole_ShouldThrowKeyNotFoundException_WhenFormatterNotFound()
    {
        // Act
        var act = () => _service.PrintToConsole("NON_EXISTENT");

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage("*NON_EXISTENT*");
    }
}
