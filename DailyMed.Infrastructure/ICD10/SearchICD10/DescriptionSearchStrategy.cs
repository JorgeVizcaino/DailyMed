using DailyMed.Core.Models;

namespace DailyMed.Infrastructure.ICD10.SearchICD10
{

    public class DescriptionSearchStrategy : ICD10Base
    {
        public override IEnumerable<ICD10Code> Search(string searchTerm)
        {
            return icd10Codes.Where(x => x.LongDescription != null &&
                x.LongDescription.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }

}
