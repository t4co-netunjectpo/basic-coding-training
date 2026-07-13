using System;

namespace SolidSample.Good.Output;

public class ConsoleWriter : IConsoleWriter
{
    public void Write(string content)
    {
        Console.Write(content);
    }
}
