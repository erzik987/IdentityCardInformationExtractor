using IdentityCardInformationExtractor.Enums;
using IdentityCardInformationExtractor.Exceptions;
using IdentityCardInformationExtractor.Helpers;
using IdentityCardInformationExtractor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityCardInformationExtractor.PapersOnProcess
{
    class PassportProcess
    {
        public IdentityCard IDCard;
        public string Text { get; set; }

        public PassportProcess(IdentityCard IDCard, string Text)
        {
            this.IDCard = IDCard;
            this.Text = Text;
        }

        private void parseFirstLine(string line) {
            string nationality = "";

            var blocks = line.Split("<");
            for (int j = 0; j < blocks.Length; j++)
            {
                var block = blocks[j];
                if (block != "")
                {
                    if (j == 1)
                    {
                        IDCard.PersonalData.GivenNames = block.Substring(3);
                        nationality = block.Substring(0, 3);
                    }

                    if (j == 3)
                    {
                        IDCard.PersonalData.Surname = block;
                    }
                }
            }


            IDCard.CardData.CardType = Enums.CardType.Passport;
            IDCard.CardData.CardSubType = Enums.CardSubType.ResidencePermit;

            if (IdentityCardHelper.nations.ContainsKey(nationality.ToUpper()))
            {
                IDCard.PersonalData.Nationality = IdentityCardHelper.nations[nationality.ToUpper()];
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load nationality");
            }
        }

        private void parseSecondLine(string line) {
            string cardCode = "";
            string validationNumberForCardCode = "";
            string cardOrigin = "";
            string dateOfBirth = "";
            string validationNumberDateOfBirth = "";
            string gender = "";
            string dateOfExpiry = "";
            string validationNumberDateOfExpiry = "";

            var blocks = line.Split("<");
            for (int j = 0; j < blocks.Length; j++)
            {
                var block = blocks[j];
                if (block != "")
                {
                    if (j == 0)
                    {
                        cardCode = block;
                    }

                    if (j == 1)
                    {
                        validationNumberForCardCode = block.Substring(0, 1);
                        cardOrigin = block.Substring(1, 3);
                        dateOfBirth = block.Substring(4, 6);
                        validationNumberDateOfBirth = block.Substring(10, 1);
                        gender = block.Substring(11, 1);
                        dateOfExpiry = block.Substring(12,6);
                        validationNumberDateOfExpiry = block.Substring(18,1);
                        if (block.Length != 19)
                        {
                            IDCard.PersonalData.PersonalNumber = block.Substring(19,10);
                        }
                    }

                    //if (j == 3)
                    //{
                    //    IDCard.PersonalData.Surname = block;
                    //}
                }
            }

            if (validationNumberForCardCode != "" && cardCode != "")
            {
                if (IdentityCardHelper.validate(cardCode, Int32.Parse(validationNumberForCardCode)))
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
                throw new WrongDataFormatException("We couldn't load card code");
            }

            if (IdentityCardHelper.countries.ContainsKey(cardOrigin))
            {
                IDCard.CardData.CardOrigin = IdentityCardHelper.countries[cardOrigin];
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load card origin");
            }

            if (IdentityCardHelper.validate(dateOfBirth, Int32.Parse(validationNumberDateOfBirth)))
            {
                IDCard.PersonalData.DateOfBirth = IdentityCardHelper.parseDateTimeFormat(dateOfBirth);
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load date of birth");
            }

            if (IdentityCardHelper.validate(dateOfExpiry, Int32.Parse(validationNumberDateOfExpiry)))
            {
                IDCard.PersonalData.DateOfBirth = IdentityCardHelper.parseDateTimeFormat(dateOfExpiry);
            }
            else
            {
                throw new WrongDataFormatException("We couldn't load date of birth");
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

        public IdentityCard getIdentityCard() {
            var lineIndex = 0;
            var lines = Text.Split("\n");
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line.Contains("<"))
                {
                    lineIndex++;
                    if (line.Length != 44)
                    {

                        throw new WrongDataFormatException($"Number of chars on line {i + 1} is {line.Length} but it should be 44. It looks like some data missing :/");
                    }

                    switch (lineIndex)
                    {
                        case 1:
                            parseFirstLine(line);
                            break;
                        case 2:
                            parseSecondLine(line);
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
