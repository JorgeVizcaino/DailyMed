using DailyMed.Core.Interfaces;
using DailyMed.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure.ICD10.SearchICD10
{
    public partial class SynonymSearchStrategy : ICD10Base
    {

        public override IEnumerable<ICD10Code> Search(string searchTerm)
        {
            return icd10Codes.Where(x => x.Synonym != null &&
                x.Synonym.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }
}
