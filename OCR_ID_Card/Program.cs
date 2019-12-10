using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Tesseract;
using IdentityCardInformationExtractor.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdentityCardInformationExtractor.Models;

namespace IdentityCardInformationExtractor
{
    class Program
    {
        static void Main(string[] args)
        {

            //    "D:\\test\\HQ_TEST_OP.jpeg"


            if (args.Length == 1)
            {
                if (args[0] == "-i" || args[0] == "--interactive")
                {
                    IdentityCard identityCard = new IdentityCard();

                    Console.WriteLine("WELCOME IN IDENTITY CARD INFORMATION EXTRACTOR");
                    while (true)
                    {
                        Console.WriteLine("Choose action:");
                        Console.WriteLine("R) read data from PC and print from model");
                        Console.WriteLine("P) read data from PC print raw version on console");

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

                                    Console.WriteLine($"Ahoj, moje meno je {identityCard.PersonalData.GivenNames} {identityCard.PersonalData.Surname}. Moja národnosť je {identityCard.PersonalData.Nationality}. " +
                                        $"Narodil som sa {identityCard.PersonalData.DateOfBirth}. Identifikačné číslo mojej karty je {identityCard.CardData.CardCode} a vyprší mi presne {identityCard.CardData.DateOfExpiry}");
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

                            case 'P':
                            case 'p':
                                string imgData;
                                do
                                {
                                    Console.WriteLine("Enter picture on data process (required):");
                                    imgData = Console.ReadLine();
                                }
                                while (imgData == "");

                                try
                                {
                                    data = new DataProcess(imgData);
                                    data.Print();
                                }
                                catch (PathToFileNotFoundException ex)
                                {

                                    Console.WriteLine(ex);
                                    break;
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
            
            if(args.Length == 4)
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
            IdentityCard identityCard = new IdentityCard();
            string output = "";

            switch (format)
            {
                case "JSON":
                    identityCard = data.getIdentityCard();
                    output = identityCard.ToJson();
                    break;
                case "XML":
                    identityCard = data.getIdentityCard();
                    output = identityCard.ToXml();
                    break;
                default:
                    Console.WriteLine("Format was not recognized");
                    return "";
            }

            return output;
        }
    }
}
