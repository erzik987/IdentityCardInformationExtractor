namespace IdentityCardInformationExtractor.Interfaces
{
    internal interface IOcrProcess
    {
        string Process(string dataPath);
    }
}