using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Interfaces
{
    public interface IPromptStrategy
    {
        /// <summary>
        /// Builds the final prompt string given some input text or context.
        /// </summary>
        /// <param name="input">The text we want to transform or act upon.</param>
        /// <returns>The final prompt string to be sent to LLM.</returns>
        string BuildPrompt(string input);
    }
}
