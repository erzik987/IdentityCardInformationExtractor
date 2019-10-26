using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR_ID_Card.Models
{
    public class ExtractedField
    {
        public string Title { get; set; }
        public List<ExtractedFieldValue> Values { get; set; }
        public ExtractedField()
        {
            Values = new List<ExtractedFieldValue>();
        }
    }

    public class ExtractedFieldValue
    {
        public string Value { get; set; }
        public float Similarity { get; set; }
    }
}