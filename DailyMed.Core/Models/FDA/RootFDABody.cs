using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Models.FDA
{
    public class RootFDABody
    {
        public Meta meta { get; set; }
        public List<Result> results { get; set; }

    }
}
