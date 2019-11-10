using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Tesseract;
using OCR_ID_Card.Exceptions;
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

            if (args.Length < 4)
            {
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

                            string backSidePath;
                            do
                            {
                                Console.WriteLine("Enter path of BACK side your identification card (required):");
                                backSidePath = Console.ReadLine();
                            }
                            while (backSidePath == "");

                            Console.WriteLine("Enter path of FRONT side your identification card (optional):");
                            var frontSidePath = Console.ReadLine();

                            DataProcess data;

                            try
                            {
                                if (frontSidePath == "")
                                {
                                    data = new DataProcess(backSidePath);
                                }
                                else
                                {
                                    data = new DataProcess(backSidePath, frontSidePath);
                                }
                            }
                            catch (PathToFileNotFoundException ex)
                            {

                                Console.WriteLine(ex);
                                break;
                            }

                            try
                            {
                                identityCard = data.getIdentityCard();

                                Console.WriteLine($"Ahoj, moje meno je {identityCard.PersonalData.Name} {identityCard.PersonalData.Surname}. Moja národnosť je {identityCard.PersonalData.Nationality}. " +
                                    $"Narodil som sa {identityCard.PersonalData.DateOfBirth}. Identifikačné číslo mojej karty je {identityCard.CardData.CardCode} a vyprší mi presne {identityCard.CardData.ExpirationDate}");
                            }
                            catch (ArgumentNullException ex)
                            {
                                Console.WriteLine(ex);
                                throw;
                            }
                            catch (WrongDataFormatException)
                            {
                                Console.WriteLine("There was problem while parsing data from identification card. You can check the data output:");
                                data.Print();
                            }
                            catch (Exception ex)
                            {
                                //Console.
                                Console.WriteLine(ex);
                                throw;
                            }

                            break;
                        default:
                            Console.WriteLine("On this key is not registered any action");
                            break;
                    }

                    Console.WriteLine("___________________________________________________");
                }
            }
            else
            {
                var frontSidePath = args[0];
                var backSidePath = args[1];
                var cardType = args[2];
                var format = args[3];

                DataProcess data;

                try
                {
                    data = new DataProcess(backSidePath, frontSidePath);
                }
                catch (PathToFileNotFoundException ex)
                {

                    Console.WriteLine(ex);
                    Console.ReadKey();
                    throw;
                }

                switch (cardType)
                {
                    case "OP":
                        Console.WriteLine(chooseOutput(format,data));
                        break;
                    default:
                        Console.WriteLine("Card type was not recognized");
                        break;
                }

                Console.ReadKey();
                
            }
            
        }

        public static string chooseOutput(string format,DataProcess data) 
        {

            string output = "";

            switch (format)
            {
                case "JSON":
                    output = data.getIdentityCardAsJson();
                    break;
                case "XML":
                    Console.WriteLine("todo output in xml format");
                    break;
                default:
                    Console.WriteLine("Format was not recognized");
                    return "";
            }

            return output;
        }
    }
}
