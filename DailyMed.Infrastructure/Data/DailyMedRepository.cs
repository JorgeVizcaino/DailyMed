using DailyMed.Core.Interfaces;
using DailyMed.Core.Models;
using DailyMed.Core.Models.CopayCard;
using DailyMed.Core.Models.DailyMed;
using DailyMed.Core.Models.FDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure.Data
{
    public class DailyMedRepository : IDailyMedRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IIndicationsParser _indicationsParser;
        private readonly IDrugIndicationsRepository _drugIndicationsRepository;

        public DailyMedRepository(IHttpClientFactory httpClientFactory, 
            IIndicationsParser indicationsParser,
            IDrugIndicationsRepository drugIndicationsRepository)
        {
            _httpClientFactory = httpClientFactory;
            _indicationsParser = indicationsParser;
            _drugIndicationsRepository = drugIndicationsRepository;
        }
        public async Task<SplDailyMedData> SearchIndicationsAsync(string searchTerm)
        {
            // 1. Create the clients
            var httpClientDailyMed = _httpClientFactory.CreateClient("DailyMed");
            var httpClientFda = _httpClientFactory.CreateClient("fdaApi");

            // 2. Query DailyMed for the SPL setid
            var searchUrl = $"spls.json?drug_name={searchTerm}&pagesize=1";
            var searchResult = await httpClientDailyMed.GetFromJsonAsync<DailyMedSearchResult>(searchUrl);

            if (searchResult?.Data == null || searchResult.Data.Count == 0)
            {
                throw new InvalidOperationException($"No SPL found for '{searchTerm}'.");
            }

            // 3. Extract the first setid
            var setid = searchResult.Data[0].SetId;
            if (string.IsNullOrWhiteSpace(setid))
            {
                throw new InvalidOperationException($"No valid setid found for '{searchTerm}'.");
            }

            // 4. Retrieve the full label from the FDA endpoint
            var labelUrl = $"label.json?search=set_id:{setid}";
            var fdaResult = await httpClientFda.GetFromJsonAsync<RootFDABody>(labelUrl);

            if (fdaResult == null || fdaResult.results == null || fdaResult.results.Count == 0)
            {
                throw new InvalidOperationException($"No FDA label data found for setid '{setid}'.");
            }

            // Example: the first result's "indications_and_usage" 
            var fdaItem = fdaResult.results[0];
            var rawIndications = fdaItem.indications_and_usage?.FirstOrDefault() ?? string.Empty;

            // 5. Parse out the relevant indications from that raw text
            var indications = _indicationsParser.ExtractIndications(rawIndications);

            await InsertDrugIndicationsAsync(searchTerm, setid, indications);

            // 6. Construct the final domain object
            var result = new SplDailyMedData
            {
                SetId = setid,
                Indications = indications
            };

            return result;
        }

        private async Task InsertDrugIndicationsAsync(string searchTerm,string setid,string indications)
        {
            var maxLength = 499;

            if (maxLength >= indications.Length)
            {
                maxLength = indications.Length - 2;
            }

            var drugObj = new DrugIndication()
            {
                DrugName = searchTerm,
                Setid = setid,
                Indication = indications.Substring(1, maxLength)

            };
            await _drugIndicationsRepository.CreateDrugIndicationsAsync(drugObj);

        }
    }
}
