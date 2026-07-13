namespace SolidSample.Good.Output;

public interface IFileWriter
{
    void Write(string filePath, string content);
}
