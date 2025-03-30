using DailyMed.Core.Interfaces;
using DailyMed.Core.Models;
using DailyMed.Core.Models.CopayCard;
using DailyMed.Core.Models.Program;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure.Data
{
    public class CopayProgramRepository : ICopayProgramRepository
    {
        private readonly string _connectionString;

        public CopayProgramRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> CreateCopayProgramAsync(CopayProgram program)
        {
            var coverageJson = JsonConvert.SerializeObject(program.CoverageEligibilities);
            var requirementsJson = JsonConvert.SerializeObject(program.Requirements);
            var benefitsJson = JsonConvert.SerializeObject(program.Benefits);
            var formsJson = JsonConvert.SerializeObject(program.Forms);
            var detailsJson = JsonConvert.SerializeObject(program.Details);

            var sql = @"
        INSERT INTO [dbo].[ProgramData]
          (ProgramName,
           CoverageEligibilitiesJson,
           ProgramType,
           RequirementsJson,
           BenefitsJson,
           FormsJson,
           FundingEvergreen,
           FundingCurrentFundingLevel,
           DetailsJson)
        OUTPUT INSERTED.ProgramId
        VALUES
          (@ProgramName,
           @CoverageEligibilitiesJson,
           @ProgramType,
           @RequirementsJson,
           @BenefitsJson,
           @FormsJson,
           @FundingEvergreen,
           @FundingCurrentFundingLevel,
           @DetailsJson);";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ProgramName", program.ProgramName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CoverageEligibilitiesJson", (object)coverageJson ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ProgramType", program.ProgramType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@RequirementsJson", (object)requirementsJson ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@BenefitsJson", (object)benefitsJson ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FormsJson", (object)formsJson ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FundingEvergreen", program.Funding?.Evergreen ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@FundingCurrentFundingLevel", program.Funding?.CurrentFundingLevel ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DetailsJson", (object)detailsJson ?? DBNull.Value);

                await conn.OpenAsync();

                // The "OUTPUT INSERTED.ProgramId" in the SQL returns the newly assigned identity
                var insertedId = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(insertedId);
            }
        }

        // The method to retrieve a single record by programId
        public async Task<CopayProgram> GetCopayProgramByIdAsync(int programId)
        {
            const string sql = @"
            SELECT 
                ProgramId,
                ProgramName,
                CoverageEligibilitiesJson,
                ProgramType,
                RequirementsJson,
                BenefitsJson,
                FormsJson,
                FundingEvergreen,
                FundingCurrentFundingLevel,
                DetailsJson
            FROM [dbo].[ProgramData]
            WHERE ProgramId = @ProgramId;
        ";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ProgramId", programId);
                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        // Build the ProgramData object
                        var program = new CopayProgram
                        {
                            ProgramId = reader.GetInt32(0),
                            ProgramName = reader.GetString(1),
                            CoverageEligibilities = DeserializeStringArray(reader.GetString(2)),
                            ProgramType = reader.GetString(3),
                            Requirements = DeserializeList<ProgramRequirement>(reader.GetString(4)),
                            Benefits = DeserializeList<ProgramBenefit>(reader.GetString(5)),
                            Forms = DeserializeList<ProgramForm>(reader.GetString(6)),
                            Funding = new FundingData
                            {
                                Evergreen = (bool)(!reader.IsDBNull(7) ? reader.GetBoolean(7) : (bool?)null),
                                CurrentFundingLevel = !reader.IsDBNull(8) ? reader.GetString(8) : null
                            },
                            Details = DeserializeList<ProgramDetail>(reader.GetString(9))
                        };

                        return program;
                    }
                    else
                    {
                        // No record found
                        return null;
                    }
                }
            }
        }

        private string[] DeserializeStringArray(string json)
        {
            if (string.IsNullOrEmpty(json))
                return new string[0];

            return JsonConvert.DeserializeObject<string[]>(json);
        }

        private List<T> DeserializeList<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return new List<T>();

            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        
    }
}
