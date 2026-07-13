using System;

namespace SolidSample.Good.Output;

public class SmtpEmailSender : IEmailSender
{
    private readonly string _smtpServer;

    public SmtpEmailSender(string smtpServer = "smtp.mymailserver.com")
    {
        _smtpServer = smtpServer;
    }

    public void Send(string to, string subject, string body)
    {
        Console.Write($"Email sent to {to} via {_smtpServer}. Subject: {subject}. Body: {body}");
    }
}
