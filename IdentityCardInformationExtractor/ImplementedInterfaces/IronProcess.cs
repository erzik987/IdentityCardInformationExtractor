using IdentityCardInformationExtractor.Interfaces;
using IronOcr;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityCardInformationExtractor.ImplementedInterfaces
{
    class IronProcess : IProcess
    {
        public string Text { get; set; }

        public string Process(string dataPath, string userName = null, string password = null)
        {
            try
            {
                //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                //var Ocr = new IronOcr.AdvancedOcr()
                //{
                //    CleanBackgroundNoise = true,
                //    EnhanceContrast = true,
                //    EnhanceResolution = true,
                //    Language = IronOcr.Languages.MultiLanguage.OcrLanguagePack, //IronOcr.Languages.English.OcrLanguagePack,
                //    Strategy = IronOcr.AdvancedOcr.OcrStrategy.Advanced,
                //    ColorSpace = AdvancedOcr.OcrColorSpace.Color,
                //    DetectWhiteTextOnDarkBackgrounds = true,
                //    InputImageType = AdvancedOcr.InputTypes.AutoDetect,
                //    RotateAndStraighten = true,
                //    ReadBarCodes = true,
                //    ColorDepth = 4
                //};

                //var testDocument = @"C:\path\to\scan.pdf";
                // var Results = Ocr.Read(testDocument);

                AutoOcr OCR = new AutoOcr() { ReadBarCodes = false };
                var Results = OCR.Read(dataPath);

                return Results.Text;
            }
            catch (Exception e)
            {
                return "Unexpected Error: " + e.Message;
            }
            
        }
    }
}
