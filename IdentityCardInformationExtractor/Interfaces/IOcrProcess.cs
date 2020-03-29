namespace IdentityCardInformationExtractor.Interfaces
{
    interface IOcrProcess
    {
        string Process(string dataPath,string userName,string password);
    }
}
