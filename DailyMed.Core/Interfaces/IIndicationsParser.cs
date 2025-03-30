using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Interfaces
{
    public interface IIndicationsParser
    {
        /// <summary>
        /// returns a cleaned list of indication strings.
        /// </summary>
        string ExtractIndications(string rawText);
    }
}
