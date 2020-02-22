using IdentityCardInformationExtractor.Models;
using System;
using System.Drawing;
using IdentityCardInformationExtractor.Exceptions;
using IdentityCardInformationExtractor.PapersOnProcess;
using IdentityCardInformationExtractor.Enums;

namespace IdentityCardInformationExtractor
{
    class DataProcess
    {
        public string Text { get; set; }
        private IdentityCard IDCard { get; set; }
        private IdentificationCardProcess identificationCardProcess { get; set; }
        public PassportProcess passportProcess { get; set; }
        private Ocr usedOcr { get; set; }
        private CardType cardType { get; set; }

        public DataProcess(Ocr usedOcr, CardType cardType, string backPageDataPath, string frontPageDataPath = null) 
        {
            this.usedOcr = usedOcr;
            this.cardType = cardType;
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

            identificationCardProcess = new IdentificationCardProcess(IDCard,Text);
            passportProcess = new PassportProcess(IDCard, Text);
        }

        public IdentityCard getIdentityCard() 
        {
            var identityCard = new IdentityCard();
            switch (cardType)
            {
                case CardType.IdentityCard:
                    identityCard = identificationCardProcess.getIdentityCard();
                    break;

                case CardType.Passport:
                    identityCard = passportProcess.getIdentityCard();
                    break;

                default: throw new ArgumentOutOfRangeException();
            }

            return identityCard;
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
                
                switch (usedOcr)
                {
                    case Ocr.Tesseract:
                        Text = new TessProcess().Process(dataPath);
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }

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

        public void Print()
        {
            Console.WriteLine();
            Console.WriteLine(Text);
            Console.WriteLine();
        }




    }
}
