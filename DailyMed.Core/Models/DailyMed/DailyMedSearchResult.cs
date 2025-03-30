using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DailyMed.Core.Models.DailyMed
{
    public class DailyMedSearchResult
    {
        public List<SplDailyMedData> Data { get; set; }
    }
}
