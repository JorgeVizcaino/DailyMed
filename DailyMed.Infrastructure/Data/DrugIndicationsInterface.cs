using DailyMed.Core.Interfaces;
using DailyMed.Core.Models;
using DailyMed.Core.Models.CopayCard;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure.Data
{
    public class DrugIndicationsRepository : IDrugIndicationsRepository
    {
        private readonly string _connectionString;

        public DrugIndicationsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Inserts a new DrugIndication, returning the newly generated Id.
        /// </summary>
        public async Task<int> CreateDrugIndicationsAsync(DrugIndication drugIndication)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = @"INSERT INTO DrugIndications (DrugName, Indication, Setid)
                  VALUES (@DrugName, @Indication, @Setid);
                  SELECT SCOPE_IDENTITY();";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@DrugName", drugIndication.DrugName);
            cmd.Parameters.AddWithValue("@Indication", drugIndication.Indication);
            cmd.Parameters.AddWithValue("@Setid", drugIndication.Setid);

            var newId = Convert.ToString(await cmd.ExecuteScalarAsync());


            return Convert.ToInt32(newId);
        }

        /// <summary>
        /// Retrieves a DrugIndication by primary key Id.
        /// Returns null if not found.
        /// </summary>
        public async Task<DrugIndication> GetByIdAsync(int id)
        {
            var sql = @"
            SELECT Id, DrugName, Indication, Setid
            FROM DrugIndications
            WHERE Id = @Id
        ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                // Build the entity
                return new DrugIndication
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    DrugName = reader.GetString(reader.GetOrdinal("DrugName")),
                    Indication = reader.GetString(reader.GetOrdinal("Indication")),
                    Setid = reader.IsDBNull(reader.GetOrdinal("Setid"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("Setid"))
                };
            }

            return null; // not found
        }

        /// <summary>
        /// Retrieves all DrugIndications in the table.
        /// </summary>
        public async Task<List<DrugIndication>> GetAllAsync()
        {
            var sql = @"
            SELECT Id, DrugName, Indication, Setid
            FROM DrugIndications
        ";

            var results = new List<DrugIndication>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var entity = new DrugIndication
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    DrugName = reader.GetString(reader.GetOrdinal("DrugName")),
                    Indication = reader.GetString(reader.GetOrdinal("Indication")),
                    Setid = reader.IsDBNull(reader.GetOrdinal("Setid"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("Setid"))
                };
                results.Add(entity);
            }

            return results;
        }

        /// <summary>
        /// Updates an existing DrugIndication record by Id.
        /// Returns true if a row was updated, false if no matching row.
        /// </summary>
        public async Task<bool> UpdateAsync(DrugIndication entity)
        {
            var sql = @"
            UPDATE DrugIndications
            SET
                DrugName = @DrugName,
                Indication = @Indication,
                Setid = @Setid
            WHERE Id = @Id
        ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@DrugName", entity.DrugName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Indication", entity.Indication ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Setid", entity.Setid ?? (object)DBNull.Value);

            await conn.OpenAsync();

            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a DrugIndication by Id.
        /// Returns true if a row was deleted, false if no row found.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var sql = @"
            DELETE FROM DrugIndications
            WHERE Id = @Id
        ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", id);

            await conn.OpenAsync();

            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return (rowsAffected > 0);
        }
    }
}
