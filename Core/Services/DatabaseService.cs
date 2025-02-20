using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace OOP_Lab_1.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string databasePath)
        {
            _connectionString = $"Data Source={databasePath}";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
            }
        }

        public async Task<bool> AddUserAsync(User user)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = "INSERT INTO Users (FullName, PassportNumber, IdentificationNumber, Phone, Email, Password, Role) " +
                          "VALUES (@FullName, @PassportNumber, @IdentificationNumber, @Phone, @Email, @Password, @Role)";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FullName", user.FullName);
                    command.Parameters.AddWithValue("@PassportNumber", user.PassportNumber);
                    command.Parameters.AddWithValue("@IdentificationNumber", user.IdentificationNumber);
                    command.Parameters.AddWithValue("@Phone", user.Phone);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Role", user.Role);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count > 0;
                }
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = new List<User>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = "SELECT * FROM Users";

                using (var command = new SqliteCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            PassportNumber = reader.GetString(2),
                            IdentificationNumber = reader.GetString(3),
                            Phone = reader.GetString(4),
                            Email = reader.GetString(5),
                            Password = reader.GetString(6),
                            Role = reader.GetInt32(7)
                        });
                    }
                }
            }

            return users;
        }
    }
}