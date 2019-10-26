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

        public string TesseractProcess() 
        {
            return new TessProcess(dataPath).Process();
        }
    }
}