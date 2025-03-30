using DailyMed.Core.Models;

namespace DailyMed.Infrastructure.ICD10.SearchICD10
{

    public class CodeSearchStrategy : ICD10Base
    {
        public override IEnumerable<ICD10Code> Search(string searchTerm)
        {
            return icd10Codes.Where(x => x.FullCode != null &&
                x.FullCode.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }

}
