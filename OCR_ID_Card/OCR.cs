using IdentityCardInformationExtractor.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace IdentityCardInformationExtractor.OCR
{
    public class OCR
    {
        public OCR(){}

        public string TesseractProcess(string dataPath) 
        {
            return new TessProcess().Process(dataPath);
        }
    }
}