﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityCardInformationExtractor.Interfaces
{
    interface IOcrProcess
    {
        string Process(string dataPath,string userName,string password);
    }
}