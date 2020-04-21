using System;

namespace IdentityCardInformationExtractor.Exceptions
{
    [Serializable]
    internal class WrongDataFormatException : Exception
    {
        public WrongDataFormatException()
        {
        }

        public WrongDataFormatException(string errorMessge)
           : base(String.Format("Message: {0}", errorMessge))
        {
        }
    }
}