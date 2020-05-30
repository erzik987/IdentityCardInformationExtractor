using System;
using IdentityCardInformationExtractor.Interfaces;
using Tesseract;

namespace IdentityCardInformationExtractor
{
    internal class Tesseract4Process : IOcrProcess
    {
        public float MeanConfidence { get; set; }
        public string Text { get; set; }
        private string userName { get; set; }
        private string password { get; set; }


        public Tesseract4Process(string userName = null, string password = null)
        {
            this.userName = userName;
            this.password = password;
        }

        public string Process(string dataPath)
        {
            try
            {
                using (var engine = new TesseractEngine("tessdata", "ces", EngineMode.Default))
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