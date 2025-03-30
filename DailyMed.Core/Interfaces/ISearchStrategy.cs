using DailyMed.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Interfaces
{
    public interface ISearchStrategy
    {
        /// <summary>
        /// Returns a filtered subset of ICD-10 codes based on the searchTerm.
        /// </summary>
        /// <param name="searchTerm">The string you are searching for.</param>
        /// <returns>Enumerable of matching codes.</returns>
        IEnumerable<ICD10Code> Search(string searchTerm);
    }
}
