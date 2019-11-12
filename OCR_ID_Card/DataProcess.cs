using IdentityCardInformationExtractor.Models;
using System;
using System.Collections.Generic;
using System.Text;
using IdentityCardInformationExtractor.Enums;
using System.Drawing;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using IdentityCardInformationExtractor.Exceptions;

namespace IdentityCardInformationExtractor
{
    class DataProcess
    {
        public string Text { get; set; }
        private IdentityCard IDCard { get; set; }
        //private Dictionary<string, CardType> cardTypes { get; set; }
        private Dictionary<string,Nationality> nations { get; set; }
        private Dictionary<string, Country> countries { get; set; }
        //private Dictionary<string, CardSubType> IdCardSubTypes { get; set; }
        private Dictionary<string, Sex> genders { get; set; }

        public DataProcess(string backPageDataPath, string frontPageDataPath = null) 
        {
            IDCard = new IdentityCard();

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

            //cardTypes = new Dictionary<string, CardType>()
            //{
            //    {"I",CardType.ObcanskyPrukaz},
            //    //{"R",IDCardSubType.PovoleniKPobytu},
            //};

            nations = new Dictionary<string, Nationality>()
            {
                {"SVK",Nationality.Slovakia},
                {"CZE",Nationality.CzechRepublic},
            };

            countries = new Dictionary<string, Country>()
            {
                {"SVK",Country.SK},
                {"CZE",Country.CZ},
            };

            //IdCardSubTypes = new Dictionary<string, CardSubType>()
            //{
            //    {"D",CardSubType.ObcanskyPrukaz},
            //    {"R",CardSubType.PovoleniKPobytu},
            //};

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
                Text = new OCR.OCR().TesseractProcess(dataPath);
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
            if (deacadeOfBirth > 51)
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

            var country = "";
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
                        country = block.Substring(2, 3);
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

            //var cardTypeExist = Enum.IsDefined(typeof(CardType), cardType[0]);
            if (true)
            {
                IDCard.CardData.CardType = (CardType)cardType[0];
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load card type");
            }            

            //var cardSubTypeExist = Enum.IsDefined(typeof(CardSubType), cardSubType[0]);
            if (true)
            {
                IDCard.CardData.CardSubType = (CardSubType)cardSubType[0];
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load card sub type");
            }

            if (countries.ContainsKey(country))
            {
                IDCard.CardData.CardOrigin = countries[country];
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
                IDCard.PersonalData.PersonalNumber = identificationNumber;
            }
            
            
        }

        private void parseSecondLine(string line) 
        {
            string dateOFBirth = "";
            string dateOfExpiry = "";
            string country = "";
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
                        country = block.Substring(15, 3);
                        validationValueForDateOfBirth = block[6] - '0';
                        validaitonValueForDateOfExpiry = block[14] - '0';

                    }
                }
            }

            if (validate(dateOfExpiry, validaitonValueForDateOfExpiry))
            {
                IDCard.CardData.DateOfExpiry = parseDateTimeFormat(dateOfExpiry);
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

            if (nations.ContainsKey(country))
            {
                IDCard.PersonalData.Nationality = nations[country];
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
                        IDCard.PersonalData.GivenNames = block;
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

        public void Print() 
        {
            Console.WriteLine(Text);
        }


    }
}
