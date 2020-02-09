using System;
using Tesseract;
using IdentityCardInformationExtractor.Interfaces;

namespace IdentityCardInformationExtractor
{
    class TessProcess : IProcess
    {
        public float MeanConfidence { get; set; }
        public string Text { get; set; }

        public TessProcess() {}

        public string Process(string dataPath,string userName = null,string password = null) 
        {
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "ces", EngineMode.Default))
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
