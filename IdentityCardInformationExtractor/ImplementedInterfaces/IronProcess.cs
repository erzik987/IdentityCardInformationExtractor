using IdentityCardInformationExtractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using asprise_ocr_api;
using Google.Cloud.Vision.V1;

namespace IdentityCardInformationExtractor.ImplementedInterfaces
{
    class IronProcess : IProcess
    {
        public string Text { get; set; }

        public string Process(string dataPath, string userName = null, string password = null)
        {
            try
            {
                var client = ImageAnnotatorClient.Create();
                var image = Google.Cloud.Vision.V1.Image.FromFile("D:\\test\\OP_CR_zadnaStrana.jpg");
                var response = client.DetectText(image);




                AspriseOCR.SetUp();
                AspriseOCR ocr = new AspriseOCR();
                ocr.StartEngine("eng", AspriseOCR.SPEED_FASTEST);
                string file = dataPath;
                string s = ocr.Recognize("D:\\test\\OP_CR_zadnaStrana.jpg", -1, -1, -1, -1, -1, AspriseOCR.RECOGNIZE_TYPE_ALL, AspriseOCR.OUTPUT_FORMAT_PLAINTEXT);
                ocr.StopEngine();

                return s;
            }
            catch (Exception e)
            {
                return "Unexpected Error: " + e.Message;
            }
            
        }
    }
}
