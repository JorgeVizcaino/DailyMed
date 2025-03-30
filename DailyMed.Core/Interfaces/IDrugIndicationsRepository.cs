using DailyMed.Core.Models;
using DailyMed.Core.Models.CopayCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Interfaces
{
    public interface IDrugIndicationsRepository
    {
        Task<int> CreateDrugIndicationsAsync(DrugIndication drugIndication);
        Task<bool> DeleteAsync(int id);
        Task<List<DrugIndication>> GetAllAsync();
        Task<DrugIndication> GetByIdAsync(int id);
        Task<bool> UpdateAsync(DrugIndication entity);
    }
}
