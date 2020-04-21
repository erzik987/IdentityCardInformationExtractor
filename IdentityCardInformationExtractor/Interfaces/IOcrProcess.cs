namespace IdentityCardInformationExtractor.Interfaces
{
    internal interface IOcrProcess
    {
        string Process(string dataPath, string userName, string password);
    }
}