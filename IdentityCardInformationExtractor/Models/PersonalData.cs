using System;
using IdentityCardInformationExtractor.Enums;
using System.Drawing;


namespace IdentityCardInformationExtractor.Models
{
    public class PersonalData
    {
        //Required
        public string GivenNames { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
        public Nationality Nationality { get; set; }
        public string PersonalNumber { get; set; }

        //Optional
        public Image Photo { get; set; }
        public Image Signature { get; set; }
        public string  Address { get; set; }
        public string PlaceOfBirth { get; set; }
        public string SpecialRemarks { get; set; }
    }
}
