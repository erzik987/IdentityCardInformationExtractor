using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Tesseract;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCR_ID_Card
{
    class Program
    {
        //#region Methods
        static void Main(string[] args)
        {
            //OCR.OCR img = new OCR.OCR("D:\\test\\download.jpeg");

            //Console.WriteLine(img.TesseractProcess());

            DataProcess data = new DataProcess("D:\\test\\download.jpeg");
            data.getIdentityCard();
        }
    }
}
