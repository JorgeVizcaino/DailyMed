using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Models
{
    public class ICD10Code
    {
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string FullCode { get; set; }
        public string LongDescription { get; set; }
        public string AltDescription { get; set; }
        public string Synonym { get; set; }
    }
}
