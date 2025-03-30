using DailyMed.Core.Models.CopayCard;

namespace DailyMed.Core.Applications
{
    public interface IDrugIndicationsApplication
    {
        Task<int> CreateDrugIndicationsAsync(DrugIndication drugIndication);
        Task<bool> DeleteAsync(int id);
        Task<List<DrugIndication>> GetAllAsync();
        Task<DrugIndication> GetByIdAsync(int id);
        Task<bool> UpdateAsync(DrugIndication entity);
    }
}