using CsvHelper;
using CsvHelper.Configuration;
using DailyMed.Core.Interfaces;
using DailyMed.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure.ICD10.SearchICD10
{
    public abstract class ICD10Base : ISearchStrategy
    {
        internal readonly List<ICD10Code> icd10Codes;
        private string csvFilePath = "StaticFiles\\icd10.csv";

        public ICD10Base()
        {
            if (icd10Codes != null)
                return;

            icd10Codes = new List<ICD10Code>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ",",
            };

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, config))
            {
                // Register the mapping so columns go to the right property
                csv.Context.RegisterClassMap<ICD10CodeMap>();

                // Fetch records
                icd10Codes = csv.GetRecords<ICD10Code>().ToList();
            }

        }

        public abstract IEnumerable<ICD10Code> Search(string searchTerm);

    }
}
