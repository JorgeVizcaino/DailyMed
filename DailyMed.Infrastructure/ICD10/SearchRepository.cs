using DailyMed.Core.Interfaces;
using DailyMed.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure.ICD10
{
    public class SearchRepository
    {
        private ISearchStrategy? _strategy;
        public void SetStrategy(ISearchStrategy strategy)
        {
            _strategy = strategy;
        }
        public IEnumerable<ICD10Code> ExecuteSearch(string searchTerm)
        {
            if (_strategy == null)
            {
                throw new InvalidOperationException("No search strategy set.");
            }

            return _strategy.Search(searchTerm);
        }
    }
}
