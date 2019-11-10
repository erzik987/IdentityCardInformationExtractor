using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OCR_ID_Card.Enums;

namespace OCR_ID_Card.Models
{
    class IdentityCard
    {
        public CardData CardData { get; set; }
        public PersonalData PersonalData { get; set; }
    }
}
