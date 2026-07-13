using System.IO;

namespace SolidSample.Good.Output;

public class FileWriter : IFileWriter
{
    public void Write(string filePath, string content)
    {
        File.WriteAllText(filePath, content);
    }
}
