using System;
using System.Collections.Generic;
using IdentityCardInformationExtractor.Enums;
using System.Drawing;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using IdentityCardInformationExtractor.Exceptions;

namespace IdentityCardInformationExtractor.PapersOnProcess.BaseFunctions
{
    public class BaseFunctionsForProcess
    {

        protected Dictionary<string, Nationality> nations { get; set; }
        protected Dictionary<string, Country> countries { get; set; }
        protected Dictionary<string, Sex> genders { get; set; }

        public BaseFunctionsForProcess() {

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

            genders = new Dictionary<string, Sex>()
            {
                {"M",Sex.Male},
                {"F",Sex.Female},
                {"<",Sex.NotApplicable}
            };
        }

        protected int parseLetterToIntValue(char inputValue)
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

        protected Boolean validate(string stringOnValidate, int? validationValue)
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

        protected DateTime parseDateTimeFormat(string stringDate)
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

            return new DateTime(year, month, day);
        }
    }
}
