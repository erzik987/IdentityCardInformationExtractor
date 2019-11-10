using System;
using System.Collections.Generic;
using System.Text;

namespace OCR_ID_Card.Exceptions
{
    [Serializable]
    class WrongDataFormatException : Exception
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
