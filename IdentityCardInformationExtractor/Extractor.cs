using IdentityCardInformationExtractor.Exceptions;
using IdentityCardInformationExtractor.Interfaces;
using IdentityCardInformationExtractor.PapersOnProcess;

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
        public static string process(string cardType, string backSidePath, string format = "JSON", string frontSidePath = null)
        {
            try
            {
                var ocr = new Tesseract4Process();

                switch (cardType)
                {
                    case "IC":
                        var identificationCard = new IdentificationCardProcess(backSidePath, frontSidePath, ocr);
                        return chooseOutput(format, identificationCard);

                    case "P":
                        var passport = new PassportProcess(backSidePath,ocr);
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

        public static string print(string cardType, string backSidePath, bool printRaw)
        {
            try
            {
                var ocr = new Tesseract4Process();

                if (printRaw)
                {
                    return ocr.Process(backSidePath);
                }
                else
                {
                    switch (cardType)
                    {
                        case "IC":
                            return new IdentificationCardProcess(backSidePath, null, ocr).print();

                        case "P":
                            return new PassportProcess(backSidePath, ocr).print();

                        default:
                            return "Card type was not recognized";
                    }
                }
            }
            catch (PathToFileNotFoundException ex)
            {
                throw ex;
            }
        }
    }
}