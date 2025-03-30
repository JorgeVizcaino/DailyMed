using DailyMed.Core.Interfaces;
using Microsoft.Data.SqlClient;
using DailyMed.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DailyMed.Infrastructure.User
{

    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // CREATE
        public async Task<int> CreateUserAsync(LoginUser user)
        {
            var sql = @"
            INSERT INTO Users (Username, PasswordHash, Role)
            OUTPUT INSERTED.Id
            VALUES (@Username, @PasswordHash, @Role);
        ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Role", user.Role ?? (object)DBNull.Value);

            await conn.OpenAsync();
            var insertedId = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(insertedId);
        }

        // READ by Username
        public async Task<LoginUser> GetByUsernameAsync(string username)
        {
            var sql = @"
            SELECT Id, Username, PasswordHash, Role
            FROM Users
            WHERE Username = @Username
        ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", username);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new LoginUser
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Role = reader.IsDBNull(reader.GetOrdinal("Role"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("Role"))
                };
            }
            return null;
        }

        // READ by Id
        public async Task<LoginUser> GetByIdAsync(int id)
        {
            var sql = @"
            SELECT Id, Username, PasswordHash, Role
            FROM Users
            WHERE Id = @Id
        ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new LoginUser
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Role = reader.IsDBNull(reader.GetOrdinal("Role"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("Role"))
                };
            }
            return null;
        }
    }
}
