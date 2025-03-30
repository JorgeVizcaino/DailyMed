using DailyMed.Core.Models;
using DailyMed.Core.Models.DailyMed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Interfaces
{
    public interface IDailyMedRepository
    {
        Task<SplDailyMedData> SearchIndicationsAsync(string searchTerm);
       
    }
}
