using System;
using System.Collections.Generic;
using System.Text;
using OCR_ID_Card.Enums;


namespace OCR_ID_Card.Models
{
    class PersonalData
    {
        //Required
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
        public Nation Nationality { get; set; }
        public string IdentificationNumber { get; set; }

        //Optional
    }
}
