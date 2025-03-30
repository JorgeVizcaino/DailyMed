using DailyMed.Core.Applications;
using DailyMed.Core.Interfaces;
using DailyMed.Core.Models.CopayCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Application.Services
{
    public class DrugIndicationsApplication : IDrugIndicationsApplication
    {
        private readonly IDrugIndicationsRepository _drugIndicationsRepository;

        public DrugIndicationsApplication(IDrugIndicationsRepository drugIndicationsRepository)
        {
            _drugIndicationsRepository = drugIndicationsRepository;
        }

        public async Task<int> CreateDrugIndicationsAsync(DrugIndication drugIndication)
        {
            return await _drugIndicationsRepository.CreateDrugIndicationsAsync(drugIndication);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _drugIndicationsRepository.DeleteAsync(id);
        }

        public async Task<List<DrugIndication>> GetAllAsync()
        {
            return await _drugIndicationsRepository.GetAllAsync();
        }

        public async Task<DrugIndication> GetByIdAsync(int id)
        {
            return await _drugIndicationsRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(DrugIndication entity)
        {
            return await _drugIndicationsRepository.UpdateAsync(entity);
        }
    }
}
