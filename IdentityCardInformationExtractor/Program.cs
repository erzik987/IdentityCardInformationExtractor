using System;

namespace IdentityCardInformationExtractor
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("you have to pass atleast two parameters, check documentation for closer info");
                Console.ReadLine();
            }

            if (args.Length == 2)
            {
                var backsidepath = args[1];
                var cardtype = args[0];
                Extractor.Process(cardtype, backsidepath);
            }

            if (args.Length == 3)
            {
                var backsidepath = args[1];
                var cardtype = args[0];
                var format = args[2];

                Extractor.Process(cardtype, backsidepath, format);
            }

            if (args.Length == 4)
            {
                var frontsidepath = args[3];
                var backsidepath = args[1];
                var cardtype = args[0];
                var format = args[2];

                if (cardtype == "p")
                {
                    Console.WriteLine("the last parameter wasn't proceed because is not needed.");
                }

                Extractor.Process(cardtype, backsidepath, format, frontsidepath);
            }
        }
    }
}