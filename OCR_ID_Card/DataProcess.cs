using OCR_ID_Card.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace OCR_ID_Card
{
    class DataProcess
    {

        public string dataPath { get; set; }
        public string Text { get; set; }
        //private IdentityCard 
        public DataProcess(string dataPath) 
        {
            this.dataPath = dataPath;
            Text = new OCR.OCR(dataPath).TesseractProcess();
        }

        public IdentityCard getIdentityCard() 
        {
            var IDCard = new IdentityCard();
            var lines = Text.Split("\n");
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line.Contains("<"))
                {
                    if (line.Length != 30) 
                    { 
                        throw new System.ArgumentException("It looks like some data missing :/", "original");
                    }

                    var blocks = line.Split("<");
                    for (int j = 0; j < blocks.Length; j++)
                    {
                        var block = blocks[j];
                        if (block != "")
                        {
                           
                            //IDCard.CardTypes = CardTypes[block.Substring(0, 10)] ;
                            //IDCard.CardOrigin = block.Substring(10, 5);
                            for (int k = 0; k < block.Length; k++)
                            {
                                var letter = block[k];
                                if (j == 0 && k > 5)
                                {
                                    IDCard.CardCode = IDCard.CardCode + letter;
                                }
                                Console.Write(letter);
                            }
                            Console.WriteLine();
                        }
                    }

                }
                
            }
            //foreach (var line in lines)
            //{
            //    if (line.Contains("<"))
            //    {
            //        var blocks = line.Split("<");
            //        foreach (var block in blocks)
            //        {
            //            if (block != "")
            //            {
            //                foreach (var letter in block)
            //                {
            //                    Console.Write(letter);
            //                }
            //                Console.WriteLine();
            //            }
            //        }
                    
            //    }
            //    
                
            //}
            return IDCard;
        }


    }
}
