namespace SolidSample.Good.Output;

public interface IEmailSender
{
    void Send(string to, string subject, string body);
}
