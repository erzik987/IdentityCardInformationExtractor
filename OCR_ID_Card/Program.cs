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
            //    "D:\\test\\op_front_page.jpg"

            IdentityCard identityCard = new IdentityCard();

            Console.WriteLine("WELCOME IN IDENTITY CARD INFORMATION EXTRACTOR");
            while (true) 
            {
                Console.WriteLine("Choose action:");
                Console.WriteLine("R) read data from PC");

                char foo = Console.ReadKey().KeyChar;
                Console.WriteLine();
               
                switch (foo)
                {
                    case 'R':
                    case 'r':
                        Console.WriteLine("Enter path of BACK side your identification card:");
                        var backSidePath = Console.ReadLine();
                        Console.WriteLine("Enter path of FRONT side your identification card (optional):");
                        var frontSidePath = Console.ReadLine();

                        DataProcess data;

                        if (frontSidePath == "")
                        {
                            data = new DataProcess(backSidePath);
                        }
                        else 
                        {
                            data = new DataProcess(backSidePath, frontSidePath);
                        }

                        try
                        {
                            identityCard = data.getIdentityCard();

                            Console.WriteLine($"Ahoj, moje meno je {identityCard.Name} {identityCard.Surname}. Moja národnosť je {identityCard.Nationality}. Narodil som sa {identityCard.DateOfBirth}. Identifikačné číslo mojej karty je " +
                            $"{identityCard.CardCode} a vyprší mi presne {identityCard.ExpirationDate}");
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex);
                            throw;
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine("There was problem while parsing data from identification card. You can check the data output:");
                            data.Print();
                        }

                        break;
                    default:
                        Console.WriteLine("On this key is not registered any action");
                        break;
                }

                Console.WriteLine("___________________________________________________");
            }
        }
    }
}
