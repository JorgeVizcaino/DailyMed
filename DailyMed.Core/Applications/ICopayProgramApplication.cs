using DailyMed.Core.Models.CopayCard;
using DailyMed.Core.Models.Program;

namespace DailyMed.Application.Services
{
    public interface ICopayProgramApplication
    {
        Task<int> MockProgramApplication();
        Task<CopayProgram> GetCopayProgramByIdAsync(int programId);
    }
}