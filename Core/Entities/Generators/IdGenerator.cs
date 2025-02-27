namespace OOP_Lab_1.Core.Generators;

public class IdGenerator
{
    public static string GenerateId(int length)
    {
        Guid guid = Guid.NewGuid();
        return guid.ToString("N").Substring(0, length);
    }
}