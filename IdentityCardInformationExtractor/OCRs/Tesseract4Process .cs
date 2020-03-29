using System;
using Tesseract;
using IdentityCardInformationExtractor.Interfaces;
using System.Text.RegularExpressions;

namespace IdentityCardInformationExtractor
{
    class Tesseract4Process : IOcrProcess
    {
        public float MeanConfidence { get; set; }
        public string Text { get; set; }

        public Tesseract4Process() {}

        public string Process(string dataPath,string userName = null,string password = null) 
        {
            try
            {
                string regexPattern = @"(bin\\(Debug|Release)\\netcoreapp\d.\d)";
                string pathToTrainedData = Regex.Replace(System.AppContext.BaseDirectory,regexPattern, "OCRs\\tessdata");
                using (var engine = new TesseractEngine(pathToTrainedData, "ces", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(dataPath))
                    {
                        using (var page = engine.Process(img))
                        {
                            Text = page.GetText();
                            MeanConfidence = page.GetMeanConfidence();
                            return page.GetText();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return "Unexpected Error: " + e.Message;
            }
        }
    }
}
