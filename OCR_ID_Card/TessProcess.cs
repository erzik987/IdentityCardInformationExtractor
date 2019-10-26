using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Tesseract;

namespace OCR_ID_Card
{
    class TessProcess
    {
        private string dataPath { get; set; }
        public float MeanConfidence { get; set; }
        public string Text { get; set; }

        public TessProcess(string dataPath) 
        {
            this.dataPath = dataPath;
        }

        public string Process() 
        {
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
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
