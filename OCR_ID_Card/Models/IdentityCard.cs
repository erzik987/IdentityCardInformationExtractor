using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OCR_ID_Card.Enums;

namespace OCR_ID_Card.Models
{
    class IdentityCard
    {
        //First line
        public CardType CardType { get; set; }
        public Nation CardOrigin { get; set; }
        public string CardCode { get; set; }
        public string IdentificationNumber { get; set; }

        //Second line
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Nation Nationality { get; set; }

        //Third line
        public  string Surname { get; set; }
        public string Name { get; set; }

        //Optional
        public Image Photo { get; set; }
        public Image FrontSide { get; set; }
        public Image BackSide { get; set; }
    }
}
