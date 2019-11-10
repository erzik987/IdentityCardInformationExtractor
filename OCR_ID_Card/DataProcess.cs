using OCR_ID_Card.Models;
using System;
using System.Collections.Generic;
using System.Text;
using OCR_ID_Card.Enums;
using System.Drawing;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using OCR_ID_Card.Exceptions;

namespace OCR_ID_Card
{
    class DataProcess
    {
        public string Text { get; set; }
        private IdentityCard IDCard { get; set; }
        private Dictionary<string, CardType> cardTypes { get; set; }
        private Dictionary<string,Nation> nations { get; set; }
        private Dictionary<string, Country> countries { get; set; }
        private Dictionary<string, IDCardSubType> IdCardSubTypes { get; set; }
        private Dictionary<string, Sex> genders { get; set; }

        public DataProcess(string backPageDataPath, string frontPageDataPath = null) 
        {
            IDCard = new IdentityCard();
            IDCard.CardData = new CardData();
            IDCard.PersonalData = new PersonalData();

            try
            {
                if (frontPageDataPath != null)
                {
                    processFrontPage(frontPageDataPath);
                }

                processBackPage(backPageDataPath);
            }
            catch (PathToFileNotFoundException ex)
            {
                throw ex;
            }

            cardTypes = new Dictionary<string, CardType>()
            {
                {"I",CardType.ObcanskyPrukaz},
                //{"R",IDCardSubType.PovoleniKPobytu},
            };

            nations = new Dictionary<string, Nation>()
            {
                {"SVK",Nation.SK},
                {"CZE",Nation.CZ},
            };

            countries = new Dictionary<string, Country>()
            {
                {"SVK",Country.SK},
                {"CZE",Country.CZ},
            };

            IdCardSubTypes = new Dictionary<string, IDCardSubType>()
            {
                {"D",IDCardSubType.ObcanskyPrukaz},
                {"R",IDCardSubType.PovoleniKPobytu},
            };

            genders = new Dictionary<string, Sex>()
            {
                {"M",Sex.Male},
                {"F",Sex.Female},
                {"<",Sex.NotApplicable}
            };
        }

        private void processFrontPage(string dataPath) 
        {
            if (dataPath != null)
            {
                Image frontPage;

                try
                {
                    frontPage = Image.FromFile(dataPath);

                    IDCard.CardData.FrontSide = frontPage;
                }
                catch (Exception)
                {
                    throw new PathToFileNotFoundException(dataPath);
                }
            }
            else
            {
                throw new System.ArgumentNullException("Data path wasnt defined", "original");
            }
        }

        private void processBackPage(string dataPath)
        {
            if (dataPath != null) 
            {
                Text = new OCR.OCR(dataPath).TesseractProcess();
                Image backPage;
                try
                {
                    backPage = Image.FromFile(dataPath);
                }
                catch (Exception)
                {
                    throw new PathToFileNotFoundException(dataPath);
                }

                IDCard.CardData.BackSide = backPage;
            }
            else
            {
                throw new System.ArgumentNullException("Data path wasnt defined", "original");
            }
        }

        private int parseLetterToIntValue(char inputValue) 
        {
            var map = new Dictionary<char, int>() 
            {
               {'0',0 },
               {'1',1 },
               {'2',2 },
               {'3',3 },
               {'4',4 },
               {'5',5 },
               {'6',6 },
               {'7',7 },
               {'8',8 },
               {'9',9 },
               {'A',10 },
               {'B',11 },
               {'C',12 },
               {'D',13 },
               {'E',14 },
               {'F',15 },
               {'G',16 },
               {'H',17 },
               {'I',18 },
               {'J',19 },
               {'K',20 },
               {'L',21 },
               {'M',22 },
               {'N',23 },
               {'O',24 },
               {'P',25 },
               {'Q',26 },
               {'R',27 },
               {'S',28 },
               {'T',29 },
               {'U',30 },
               {'V',31 },
               {'W',32 },
               {'X',33 },
               {'Y',34 },
               {'Z',35 },
            };

            return map[inputValue]; 

        }

        private Boolean validate(string stringOnValidate,int? validationValue) 
        {
            var count = 0;
            var index = 7;
            for (int i = 0; i < stringOnValidate.Length; i++)
            {
                var letter = stringOnValidate[i];
                switch (index)
                {
                    case 7: 
                        count = count + parseLetterToIntValue(letter) * 7;
                        index = 3;
                        break;
                    case 3:
                        count = count + parseLetterToIntValue(letter) * 3;
                        index = 1;
                        break;
                    case 1:
                        count = count + parseLetterToIntValue(letter) * 1;
                        index = 7;
                        break;
                    default:
                        break;
                }                
            }

            //(count % 10 == validationValue) ? return true : return false;
            if (count % 10 == validationValue)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        private DateTime parseDateTimeFormat(string stringDate) 
        {
            if (stringDate.Length > 6) 
            {
                throw new System.ArgumentNullException("Wrong format on parsing date", "original");
            }

            string yearAsString;
            var deacadeOfBirth = Convert.ToInt32(stringDate[0]);
            if (deacadeOfBirth > 3)
            {
                yearAsString = "19" + stringDate.Substring(0, 2);
            }
            else 
            {
                yearAsString = "20" + stringDate.Substring(0, 2);
            }

            int year = Convert.ToInt32(yearAsString);
            int month = Convert.ToInt32(stringDate.Substring(2, 2));
            int day = Convert.ToInt32(stringDate.Substring(4, 2));

            return new DateTime(year,month,day);
        }

        private void parseFirstLine(string line) 
        {
            Boolean cardCodeExist = true;
            Boolean identificationNumberExist = true;

            var nation = "";
            var cardSubType = "";
            var cardType = "";
            var cardCode = "";
            var identificationNumber = "";

            int? validationNumber = null;
            var blocks = line.Split("<");
            for (int j = 0; j < blocks.Length; j++)
            {
                var block = blocks[j];
                if (block != "")
                {
                    if (j == 0) 
                    {
                        nation = block.Substring(2, 3);
                        cardSubType = block.Substring(1,1);
                        cardType = block.Substring(0, 1);
                        cardCode = block.Substring(5);
                    }

                    if (j == 1) 
                    {
                        identificationNumber = block.Substring(1);
                        validationNumber = block[0] - '0';
                    }
                }
            }

            if (cardTypes.ContainsKey(cardType))
            {
                IDCard.CardData.CardType = cardTypes[cardType];
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load card type");
            }

            if (IdCardSubTypes.ContainsKey(cardSubType))
            {
                IDCard.CardData.IdCardSubType = IdCardSubTypes[cardSubType];
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load card sub type");
            }

            if (nations.ContainsKey(nation))
            {
                IDCard.CardData.CardOrigin = nations[nation];
            }
            else 
            {
                throw new WrongDataFormatException("We couldn't load card origin");
            }

            if (cardCodeExist)
            {
                if (validationNumber != null)
                {
                    if (validate(cardCode, validationNumber))
                    {
                        IDCard.CardData.CardCode = cardCode;
                    }
                    else
                    {
                        throw new WrongDataFormatException("The data was not load properly");
                    }
                }
                else 
                {
                    IDCard.CardData.CardCode = cardCode;
                }
            }
            else 
            {
                throw new WrongDataFormatException("We couldn't load card code");
            }

            if (identificationNumberExist) 
            { 
                IDCard.PersonalData.IdentificationNumber = identificationNumber;
            }
            
            
        }

        private void parseSecondLine(string line) 
        {
            string dateOFBirth = "";
            string dateOfExpiry = "";
            string nationality = "";
            string gender = "";

            int? validationValueForDateOfBirth = null;
            int? validaitonValueForDateOfExpiry = null;
            int? generalValidationValue = line[line.Length-1];

            var blocks = line.Split("<");
            for (int j = 0; j < blocks.Length; j++)
            {
                var block = blocks[j];
                if (block != "")
                {
                    if (j == 0)
                    {
                        dateOFBirth = block.Substring(0, 6);
                        dateOfExpiry = block.Substring(8, 6);
                        gender = block.Substring(7,1);
                        nationality = block.Substring(15, 3);
                        validationValueForDateOfBirth = block[6] - '0';
                        validaitonValueForDateOfExpiry = block[14] - '0';

                    }
                }
            }

            if (validate(dateOfExpiry, validaitonValueForDateOfExpiry))
            {
                IDCard.CardData.ExpirationDate = parseDateTimeFormat(dateOfExpiry);
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load date of expiration");
            }

            if (validate(dateOFBirth, validationValueForDateOfBirth))
            {
                IDCard.PersonalData.DateOfBirth = parseDateTimeFormat(dateOFBirth);
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load date of birth");
            }

            if (nations.ContainsKey(nationality))
            {
                IDCard.PersonalData.Nationality = nations[nationality];
            }
            else 
            {
                throw new WrongDataFormatException("We couldn't load nationality");
            }

            if (genders.ContainsKey(gender))
            {
                IDCard.PersonalData.Sex = genders[gender];
            }
            else 
            {
                throw new WrongDataFormatException("We couldn't load gender");
            }
        }

        private void parseThirdLine(string line) 
        {
            var blocks = line.Split("<");
            for (int j = 0; j < blocks.Length; j++)
            {
                var block = blocks[j];
                if (block != "")
                {
                    if (j == 0)
                    {
                        IDCard.PersonalData.Surname = block;
                    }

                    if (j == 2)
                    {
                        IDCard.PersonalData.Name = block;
                    }
                }
            }
        }

        public IdentityCard getIdentityCard() 
        {
            var lineIndex = 0;
            var lines = Text.Split("\n");
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line.Contains("<"))
                {
                    lineIndex++;
                    if (line.Length != 30) 
                    { 
                        throw new WrongDataFormatException("It looks like some data missing :/");
                    }

                    switch (lineIndex)
                    {
                        case 1:
                            parseFirstLine(line);
                            break;
                        case 2:
                            parseSecondLine(line);
                            break;
                        case 3:
                            parseThirdLine(line);
                            break;
                        default:
                            throw new WrongDataFormatException("Error occured while trying to parse identity card data");
                    }
                }
            }
            return IDCard;
        }

        public dynamic getIdentityCardAsJson() 
        {
            var data = getIdentityCard();

            var obj = new IdentityCard
            {
                PersonalData = new PersonalData 
                {
                    Name = data.PersonalData.Name,
                    Sex = data.PersonalData.Sex,
                    DateOfBirth = data.PersonalData.DateOfBirth,
                    IdentificationNumber = data.PersonalData.IdentificationNumber,
                    Nationality = data.PersonalData.Nationality,
                    Surname = data.PersonalData.Surname
                },
                CardData = new CardData
                {
                    CardCode = data.CardData.CardCode,
                    CardType = data.CardData.CardType,
                    CardOrigin = data.CardData.CardOrigin,
                    ExpirationDate = data.CardData.ExpirationDate,
                    //Photo = data.CardData.Photo,
                    //BackSide = data.CardData.BackSide,
                    //FrontSide = data.CardData.FrontSide
                }
            };

            string json = JsonConvert.SerializeObject(obj);

            return json;
        }

        public dynamic getIdentityCardAsXml()
        {
            //var data = getIdentityCard();

            XmlSerializer xsSubmit = new XmlSerializer(typeof(IdentityCard));
            var subReq = getIdentityCard();
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, subReq);
                    xml = sww.ToString(); // Your XML
                }
            }

            return xml;
        }

        public void Print() 
        {
            Console.WriteLine(Text);
        }


    }
}
