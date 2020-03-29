using IdentityCardInformationExtractor.Exceptions;
using IdentityCardInformationExtractor.PapersOnProcess;
using IdentityCardInformationExtractor.Interfaces;
using System;

namespace IdentityCardInformationExtractor
{
    public class Extractor
    {
        ///<sumary>
        /// Process the given image with aditional information
        ///</sumary>
        ///<param name="cardType"> 
        /// Card type (required) - (IC|P). Defines which type of card will be processed.
        ///</param>
        ///<param name="backSidePath">
        /// Path to image with machine readable field (required)
        ///</param>
        ///<param name="format">
        ///Format (optional, default is JSON) - (JSON|XML)
        ///</param>
        ///<param name="frontSidePath">
        /// Path to other side of ID card (optional)
        ///</param>
        public static string process(string cardType, string backSidePath , string format = "JSON", string frontSidePath = null) 
        {
            try
            {
                switch (cardType)
                {
                    case "IC":
                        var identificationCard = new IdentificationCardProcess(backSidePath,frontSidePath);
                        return chooseOutput(format, identificationCard);

                    case "P":
                        var passport = new PassportProcess(backSidePath);
                        return chooseOutput(format, passport);

                    default:
                        return "Card type was not recognized";
                }
            }
            catch (PathToFileNotFoundException ex)
            {
                throw ex;
            }
        }

        private static string chooseOutput(string format, ICardDataProcess cardDataProcess) 
        {
            switch (format)
            {
                case "JSON":
                    return cardDataProcess.getIdentityCard().ToJson();
                case "XML":
                    return cardDataProcess.getIdentityCard().ToXml();
                default:
                    return cardDataProcess.getIdentityCard().ToJson();
            }
        }
    }
}
