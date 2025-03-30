using DailyMed.Core.Models.DailyMed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Applications
{
    public interface IDailyMedApplication
    {
        Task<SplDailyMedData> SearchIndicationsAsync(string searchTerm);
    }
}
