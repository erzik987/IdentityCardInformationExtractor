using OCR_ID_Card.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace OCR_ID_Card
{
    class DataProcess
    {

        public string dataPath { get; set; }
        public string Text { get; set; }
        private IdentityCard IDCard { get; set; }

        public DataProcess(string dataPath) 
        {
            this.dataPath = dataPath;
            Text = new OCR.OCR(dataPath).TesseractProcess();
            IDCard = new IdentityCard();
        }

        private int parseLetterToIntValue(char inputValue) 
        {
            var map = new Dictionary<char, int>() 
            {
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

        private Boolean validate(string stringOnValidate,int validationValue) 
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

        private void parseFirstLine(string line) 
        {
            var cardCode = "";
            var identificationNumber = "";
            var blocks = line.Split("<");
            for (int j = 0; j < blocks.Length; j++)
            {
                var block = blocks[j];
                if (block != "")
                {
                    for (int k = 0; k < block.Length; k++)
                    {
                        var letter = block[k];
                        if (j == 0 && k > 4)
                        {
                            cardCode = cardCode + letter;
                        }

                        if (j == 1 && k > 0) 
                        {
                            identificationNumber = identificationNumber + letter;
                        }
                    }
                }
            }


            IDCard.CardCode = cardCode;
            IDCard.IdentificationNumber = identificationNumber;
        }

        private void parseSecondLine(string line) 
        { 
            
        }

        private void parseThirdLine(string line) 
        { 
        
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
                        throw new System.ArgumentException("It looks like some data missing :/", "original");
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
                            throw new System.ArgumentException("Nastala chyba pri spracovaní dát, skontrolujte či newm čo"); // ToDo preložiť a dokončiť
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
