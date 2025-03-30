using CsvHelper.Configuration;
using DailyMed.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure.ICD10
{
    public sealed class ICD10CodeMap : ClassMap<ICD10Code>
    {
        public ICD10CodeMap()
        {
            Map(m => m.Category).Index(0);
            Map(m => m.Subcategory).Index(1);
            Map(m => m.FullCode).Index(2);
            Map(m => m.LongDescription).Index(3);
            Map(m => m.AltDescription).Index(4);
            Map(m => m.Synonym).Index(5);
        }
    }
}
