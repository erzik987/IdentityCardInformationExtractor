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
using IdentityCardInformationExtractor.Enums;

namespace IdentityCardInformationExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            //    "D:\\test\\HQ_TEST_OP.jpeg"

            if (args.Length < 2)
            {
                Console.WriteLine("You have to pass atleast two parameters, check documentation for closer info");
                Console.ReadLine();
            }

            if (args.Length == 2)
            {
                var backSidePath = args[1];
                var cardType = args[0];

                process(cardType, backSidePath);
            }

            if (args.Length == 3)
            {
                var backSidePath = args[1];
                var cardType = args[0];
                var format = args[2];

                process(cardType, backSidePath, format);
            }

            if (args.Length == 4)
            {
                var frontSidePath = args[3];
                var backSidePath = args[1];
                var cardType = args[0];
                var format = args[2];

                if (cardType == "P")
                {
                    Console.WriteLine("The last parameter wasn't proceed because is not needed.");
                }

                process(cardType, backSidePath, format, frontSidePath);
                
            }
        }

        public static void process(string cardType, string backSidePath , string format = "JSON", string frontSidePath = null) 
        {
            DataProcess data;

            try
            {
                switch (cardType)
                {
                    case "IC":
                        data = new DataProcess(Ocr.Tesseract, CardType.IdentityCard, backSidePath, frontSidePath);
                        data.Print();
                        Console.WriteLine(chooseOutput(format, data));
                        break;

                    case "P":
                        data = new DataProcess(Ocr.Tesseract, CardType.Passport, backSidePath);
                        data.Print();
                        Console.WriteLine(chooseOutput(format, data));
                        break;

                    default:
                        Console.WriteLine("Card type was not recognized");
                        break;
                }
            }
            catch (PathToFileNotFoundException ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
                throw;
            }
            //Console.ReadKey();
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
                    identityCard = data.getIdentityCard();
                    output = identityCard.ToJson();
                    break;
            }

            return output;
        }
    }
}
