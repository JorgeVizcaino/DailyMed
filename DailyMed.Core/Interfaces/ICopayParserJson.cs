using DailyMed.Core.Models.CopayCard;

namespace DailyMed.Core.Interfaces
{
    public interface ICopayParserJson
    {
        Task<StandardCopayCardDto> DoConvertAsync();
    }
}