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
using IdentityCardInformationExtractor.PapersOnProcess;

namespace IdentityCardInformationExtractor
{
    class DataProcess
    {
        public string Text { get; set; }
        private IdentityCard IDCard { get; set; }
        private IdentificationCardProcess identificationCardProcess { get; set; }
        public PassportProcess passportProcess { get; set; }

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

            identificationCardProcess = new IdentificationCardProcess(IDCard,Text);
            passportProcess = new PassportProcess();
        }

        public IdentityCard getIdentityCard() 
        {
            return identificationCardProcess.getIdentityCard();
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

        public void Print()
        {
            Console.WriteLine();
            Console.WriteLine(Text);
            Console.WriteLine();
        }




    }
}
