using IdentityCardInformationExtractor.Models;

namespace IdentityCardInformationExtractor.Interfaces
{
    internal interface ICardDataProcess
    {
        IdentityCard getIdentityCard();
    }
}