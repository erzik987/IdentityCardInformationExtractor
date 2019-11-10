using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OCR_ID_Card.Enums;

namespace OCR_ID_Card.Models
{
    class CardData
    {
        //Required
        public CardType CardType { get; set; }
        public IDCardSubType IdCardSubType { get; set; }
        public Nation CardOrigin { get; set; }
        public string CardCode { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Image Photo { get; set; }
        public Image FrontSide { get; set; }

        //Optional
        public Image BackSide { get; set; }
    }
}
