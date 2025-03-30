using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Models.CopayCard
{
    public class DrugIndication
    {
        public int Id { get; set; }
        public string DrugName { get; set; }  
        public string Indication { get; set; }
        public string Setid { get; set; } 

    }
}
