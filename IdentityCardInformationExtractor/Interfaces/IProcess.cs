using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityCardInformationExtractor.Interfaces
{
    interface IProcess
    {
        string Text { get; set; }        
        string Process(string dataPath,string userName,string password);
    }
}
