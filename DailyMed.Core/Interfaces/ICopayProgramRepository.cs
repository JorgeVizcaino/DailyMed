using DailyMed.Core.Models.Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Interfaces
{
    public interface ICopayProgramRepository
    {
        Task<int> CreateCopayProgramAsync(CopayProgram program);
        Task<CopayProgram> GetCopayProgramByIdAsync(int programId);
    }
}
