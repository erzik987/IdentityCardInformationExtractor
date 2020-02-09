using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using IdentityCardInformationExtractor.Enums;

namespace IdentityCardInformationExtractor.Models
{
    public class CardData
    {
        //Required
        public CardType CardType { get; set; }
        public CardSubType CardSubType { get; set; }
        public Country CardOrigin { get; set; }
        public string CardCode { get; set; }
        public DateTime DateOfExpiry { get; set; }
        public Image BackSide { get; set; }

        //Optional
        public Image FrontSide { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public string IssuedBy { get; set; }
    }
}
