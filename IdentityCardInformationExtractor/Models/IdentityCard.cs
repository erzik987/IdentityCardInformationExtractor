using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using IdentityCardInformationExtractor.Enums;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace IdentityCardInformationExtractor.Models
{
    public class IdentityCard
    {
        public IdentityCard()
        {
            CardData = new CardData();
            PersonalData = new PersonalData();
        }
        public CardData CardData { get; set; }
        public PersonalData PersonalData { get; set; }

        private Object getIdentityCardAsObject() 
        {
            var obj = new IdentityCard
            {
                PersonalData = new PersonalData
                {
                    GivenNames = PersonalData.GivenNames,
                    Sex = PersonalData.Sex,
                    DateOfBirth = PersonalData.DateOfBirth,
                    PersonalNumber = PersonalData.PersonalNumber,
                    Nationality = PersonalData.Nationality,
                    Surname = PersonalData.Surname,
                    Address = PersonalData.Address,
                    PlaceOfBirth = PersonalData.PlaceOfBirth,
                    SpecialRemarks = PersonalData.SpecialRemarks
                },
                CardData = new CardData
                {
                    CardCode = CardData.CardCode,
                    CardType = CardData.CardType,
                    CardOrigin = CardData.CardOrigin,
                    DateOfExpiry = CardData.DateOfExpiry,
                    DateOfIssue = CardData.DateOfIssue,
                    CardSubType = CardData.CardSubType,
                    IssuedBy = CardData.IssuedBy
                }
            };

            return obj;
        }
        public string ToJson()
        {
            var obj = getIdentityCardAsObject();

            string json = JsonConvert.SerializeObject(obj);

            return json;
        }

        public string ToXml() 
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(IdentityCard));
            var subReq = getIdentityCardAsObject();
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, subReq);
                    xml = sww.ToString();
                }
            }

            return xml;
        }
    }
}
