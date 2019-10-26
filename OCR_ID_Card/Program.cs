using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Tesseract;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using OCR_ID_Card.Models;

namespace OCR_ID_Card
{
    class Program
    {
        static void Main(string[] args)
        {
            //    "D:\\test\\download.jpeg"
            //    "D:\\test\\op-test.jpg"
            DataProcess data = new DataProcess("D:\\test\\download.jpeg");
            //data.Print();
            //data.getIdentityCard();
            IdentityCard identityCard = data.getIdentityCard();

            Console.WriteLine($"Ahoj, moje meno je {identityCard.Name} {identityCard.Surname}. Moja národnosť je {identityCard.Nationality}. Narodil som sa {identityCard.DateOfBirth}. Identifikačné číslo mojej karty je " +
                $"{identityCard.CardCode} a vyprší mi presne {identityCard.ExpirationDate}");
        }
    }
}
