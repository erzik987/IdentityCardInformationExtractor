using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OCR_ID_Card.Models
{
    class IdentityCard
    {
        //First line
        public CardType CardType { get; set; }

        //public Dictionary<CardType, String> CardTypes = new Dictionary<CardType, String>
        //{
        //    { CardType.IdCard, "ID" },
        //    { CardType.Other, "" }
        //};

        public CardOrigin CardOrigin { get; set; }
        public string CardCode { get; set; }

        //Second line
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Nationality Nationality { get; set; }

        //Third line
        public  string Surname { get; set; }
        public string Name { get; set; }

        //Optional
        public Image? Photo { get; set; }
        public Image? FrontSide { get; set; }
        public Image? BackSide { get; set; }
    }



    public enum Gender
    {
        Man,
        Woman,
        Unknown
    }

    public enum CardType
    { 
        IdCard,
        Other
    }

    public enum CardOrigin
    { 
        SK,
        CZ
    }

    public enum Nationality
    { 
        SK,
        CZ
    }
}
