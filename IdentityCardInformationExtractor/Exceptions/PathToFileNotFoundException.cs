using System;

namespace IdentityCardInformationExtractor.Exceptions
{
    internal class PathToFileNotFoundException : Exception
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