using IdentityCardInformationExtractor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityCardInformationExtractor.Interfaces
{
    interface ICardDataProcess
    {
        IdentityCard getIdentityCard();
    }
}
