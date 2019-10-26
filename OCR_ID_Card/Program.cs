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
        static void Main(string[] args)
        {
            DataProcess data = new DataProcess("D:\\test\\op-test.jpg");
            data.Print();
            data.getIdentityCard();
        }
    }
}
