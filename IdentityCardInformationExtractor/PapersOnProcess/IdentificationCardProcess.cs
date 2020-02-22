using System;
using IdentityCardInformationExtractor.Enums;
using IdentityCardInformationExtractor.Exceptions;
using IdentityCardInformationExtractor.Models;
using IdentityCardInformationExtractor.Helpers;

namespace IdentityCardInformationExtractor.PapersOnProcess
{
    class IdentificationCardProcess
    {

        public IdentityCard IDCard;
        public string Text { get; set; }

        public IdentificationCardProcess(IdentityCard IDCard, string Text) {
            this.IDCard = IDCard;
            this.Text = Text;
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
                        cardSubType = block.Substring(1, 1);
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

            if (IdentityCardHelper.countries.ContainsKey(country))
            {
                IDCard.CardData.CardOrigin = IdentityCardHelper.countries[country];
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load card origin");
            }

            if (cardCodeExist)
            {
                if (validationNumber != null)
                {
                    if (IdentityCardHelper.validate(cardCode, validationNumber))
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
            int? generalValidationValue = line[line.Length - 1];

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
                        gender = block.Substring(7, 1);
                        country = block.Substring(15, 3);
                        validationValueForDateOfBirth = block[6] - '0';
                        validaitonValueForDateOfExpiry = block[14] - '0';

                    }
                }
            }

            if (IdentityCardHelper.validate(dateOfExpiry, validaitonValueForDateOfExpiry))
            {
                IDCard.CardData.DateOfExpiry = IdentityCardHelper.parseDateTimeFormat(dateOfExpiry);
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load date of expiration");
            }

            if (IdentityCardHelper.validate(dateOFBirth, validationValueForDateOfBirth))
            {
                IDCard.PersonalData.DateOfBirth = IdentityCardHelper.parseDateTimeFormat(dateOFBirth);
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load date of birth");
            }

            if (IdentityCardHelper.nations.ContainsKey(country))
            {
                IDCard.PersonalData.Nationality = IdentityCardHelper.nations[country];
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load nationality");
            }

            if (IdentityCardHelper.genders.ContainsKey(gender))
            {
                IDCard.PersonalData.Sex = IdentityCardHelper.genders[gender];
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
    }
}
