using DailyMed.Core.Models;
using DailyMed.Infrastructure.ICD10;
using DailyMed.Infrastructure.ICD10.SearchICD10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DailyMed.Infrastructure.ICD10.SearchICD10.SynonymSearchStrategy;

namespace DailyMed.Infratructure.Test
{
    public class ICD10Searchtest
    {

        [Fact]
        public void SynonymSearchStrategy_FindsAllCholera()
        {
            // Arrange
            var strategy = new SynonymSearchStrategy();
            var searchTerm = "Cholera";

            // Act
            var results = strategy.Search(searchTerm).ToList();

            // Assert
            // Expect 3 matches (A000, A001, A009) as their Synonym == "Cholera"
            Assert.Equal(3, results.Count);
            Assert.Contains(results, r => r.FullCode == "A000");
            Assert.Contains(results, r => r.FullCode == "A001");
            Assert.Contains(results, r => r.FullCode == "A009");
        }

        [Fact]
        public void SynonymSearchStrategy_NoResultsForUnknownSynonym()
        {
            // Arrange
            var strategy = new SynonymSearchStrategy();
            var searchTerm = "UnknownTerm";

            // Act
            var results = strategy.Search(searchTerm).ToList();

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void DescriptionSearchStrategy_FindsBiovar()
        {
            // Arrange
            var strategy = new DescriptionSearchStrategy();
            var searchTerm = "biovar";

            // Act
            var results = strategy.Search(searchTerm).ToList();

            // Assert
            // Expect 2 matches: A000 and A001 (both mention "biovar" in LongDescription).
            Assert.Equal(2, results.Count);
            Assert.Contains(results, r => r.FullCode == "A000");
            Assert.Contains(results, r => r.FullCode == "A001");
        }

        [Fact]
        public void CodeSearchStrategy_FindsB010()
        {
            // Arrange
            var strategy = new CodeSearchStrategy();
            var searchTerm = "B010";

            // Act
            var results = strategy.Search(searchTerm).ToList();

            // Assert
            // Expect 1 match: B010
            Assert.Single(results);
            Assert.Equal("B010", results.First().FullCode);
        }

        [Fact]
        public void CodeSearchStrategy_NoResultsForNonExistingCode()
        {
            // Arrange
            var strategy = new CodeSearchStrategy();
            var searchTerm = "Z999";

            // Act
            var results = strategy.Search(searchTerm).ToList();

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void SearchContext_DelegatesToCurrentStrategy()
        {
            // Arrange
            var searchContext = new SearchRepository();

            // 1) Test synonym strategy
            searchContext.SetStrategy(new SynonymSearchStrategy());
            var choleraResults = searchContext.ExecuteSearch("Cholera").ToList();
            Assert.Equal(3, choleraResults.Count);

            // 2) Switch to code strategy
            searchContext.SetStrategy(new CodeSearchStrategy());
            var codeResults = searchContext.ExecuteSearch("B010").ToList();
            Assert.Single(codeResults);
            Assert.Equal("B010", codeResults.First().FullCode);
        }
    }

}
