using IdentityCardInformationExtractor.Models;

namespace IdentityCardInformationExtractor.Interfaces
{
    internal interface ICardDataProcess
    {
        IdentityCard getIdentityCard();

        string ProcessCard(IOcrProcess ocr, string backPageDataPath, string frontPageDataPath = null);
    }
}