using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityCardInformationExtractor.Exceptions
{
    class PathToFileNotFoundException : Exception
    {
        public PathToFileNotFoundException()
        {

        }

        public PathToFileNotFoundException(string filePath)
           : base(String.Format("Path {0} wasnt found", filePath))
        {

        }
    }
}
