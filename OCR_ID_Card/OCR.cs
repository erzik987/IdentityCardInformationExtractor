using OCR_ID_Card.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace OCR_ID_Card.OCR
{
    public class OCR
    {
        private string dataPath { get; set; }

        public OCR(string dataPath)
        {
            this.dataPath = dataPath;
        }

        public List<ExtractedField> ExtractFields(string[] fields, string text, double tolerance)
        {
            return Extractor.ExtractFields(text, fields, tolerance);
        }


        public void Print() 
        {
            Extractor.PrintDataFromImg(dataPath);
        }

        public string TesseractProcess() 
        {
            return new TessProcess(dataPath).Process();
        }

        public string GetText(Bitmap bitmap)
        {
            return "asd";
            //using (var engine = new TesseractEngine(dataPath, "ces", EngineMode.Default))
            //{
            //    using (var img = PixConverter.ToPix(bitmap))
            //    {
            //        engine.SetVariable("preserve_interword_spaces", true);
            //        using (var page = engine.Process(img))
            //        {
            //            return page.GetText();
            //        }
            //    }
            //}
        }
    }
}