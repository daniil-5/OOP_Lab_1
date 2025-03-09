using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;
using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Entities.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;
  
    public UserRepository(string databasePath)
    {
        _connectionString = $"Data Source={databasePath}";
        InitializeAccountsDatabase();
    }
    
    private void InitializeAccountsDatabase()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
        }
    }

    // Method to register a new user
    public async Task<bool> RegisterUserAsync(User user, string bankId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
                INSERT INTO Users (FullName, PassportNumber, IdentificationNumber, Phone, Email, Password, Role, BankId, Status)
                VALUES (@FullName, @PassportNumber, @IdentificationNumber, @Phone, @Email, @Password, @Role, @BankId, @Status);";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@PassportNumber", user.PassportNumber);
                command.Parameters.AddWithValue("@IdentificationNumber", user.IdentificationNumber);
                command.Parameters.AddWithValue("@Phone", user.Phone);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", HashPassword(user.Password));
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@BankId", bankId);
                command.Parameters.AddWithValue("@Status", "Pending");

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0; // Return true if registration was successful
            }
        }
    }

    public async Task<User> LoginAsync(string email, string password, string bankId)
{
    try
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
                SELECT * FROM Users 
                WHERE Email = @Email AND BankId = @BankId AND Status = @Status;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@BankId", bankId);
                command.Parameters.AddWithValue("@Status", "Approved");

                using (var reader = await command.ExecuteReaderAsync())
                {
                    // Check if the reader has any rows
                    if (await reader.ReadAsync())
                    {
                        int idOrdinal = reader.GetOrdinal("Id");
                        int fullNameOrdinal = reader.GetOrdinal("FullName");
                        int passportNumberOrdinal = reader.GetOrdinal("PassportNumber");
                        int identificationNumberOrdinal = reader.GetOrdinal("IdentificationNumber");
                        int phoneOrdinal = reader.GetOrdinal("Phone");
                        int emailOrdinal = reader.GetOrdinal("Email");
                        int passwordOrdinal = reader.GetOrdinal("Password");
                        int roleOrdinal = reader.GetOrdinal("Role");

                        var hashedPassword = reader.GetString(passwordOrdinal); // Get the hashed password
                        
                        if (VerifyPassword(password, hashedPassword))
                        {
                            return new User
                            {
                                Id = reader.GetInt32(idOrdinal),
                                FullName = reader.GetString(fullNameOrdinal),
                                PassportNumber = reader.GetString(passportNumberOrdinal),
                                IdentificationNumber = reader.GetString(identificationNumberOrdinal),
                                Phone = reader.GetString(phoneOrdinal),
                                Email = reader.GetString(emailOrdinal),
                                Password = hashedPassword,
                                Role = reader.GetInt32(roleOrdinal)
                            };
                        }
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Log the exception (replace with proper logging in production)
        Console.WriteLine($"An error occurred during login: {ex.Message}");
        throw; // Re-throw the exception to propagate it up the call stack
    }
    
    return null;
}

    // Method to get all users of a specific bank (by BankId)
    public async Task<List<User>> GetUsersByBankAsync(string bankId)
    {
        var users = new List<User>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
                SELECT * FROM Users 
                WHERE BankId = @BankId;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@BankId", bankId);

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
        }

        return users; // Return the list of users
    }

    // Method to update user details
    public async Task<bool> UpdateUserAsync(User user, string bankId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
                UPDATE Users 
                SET FullName = @FullName, PassportNumber = @PassportNumber, IdentificationNumber = @IdentificationNumber, 
                    Phone = @Phone, Email = @Email, Password = @Password, Role = @Role, BankId = @BankId 
                WHERE Id = @Id;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@PassportNumber", user.PassportNumber);
                command.Parameters.AddWithValue("@IdentificationNumber", user.IdentificationNumber);
                command.Parameters.AddWithValue("@Phone", user.Phone);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", HashPassword(user.Password)); // Hash the password
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@BankId", bankId);
                command.Parameters.AddWithValue("@Id", user.Id);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0; // Return true if update was successful
            }
        }
    }

    // Method to delete a user
    public async Task<bool> DeleteUserAsync(int userId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
                DELETE FROM Users 
                WHERE Id = @Id;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", userId);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0; // Return true if deletion was successful
            }
        }
    }
    
    public async Task<User> UserExistsByEmailAsync(string email, string bankId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
        SELECT * 
        FROM Users 
        WHERE Email = @Email AND BankId = @BankId;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@BankId", bankId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var user = new User
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            PassportNumber = reader.GetString(2),
                            IdentificationNumber = reader.GetString(3),
                            Phone = reader.GetString(4),
                            Email = reader.GetString(5),
                            Password = reader.GetString(6),
                            Role = reader.GetInt32(7)
                        };
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
    
    public async Task<bool> ApproveUserStatusAsync(int userId, string bankId)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"
                UPDATE Users
                SET Status = @Status
                WHERE Id = @UserId AND BankId = @BankId;";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Status", "Approved");
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@BankId", bankId);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception (replace with proper logging in production)
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }
    
    public async Task<List<User>> GetPendingUsersAsync(string bankId)
{
    var pendingUsers = new List<User>();

    try
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
                SELECT * FROM Users
                WHERE Status = @Status AND BankId = @BankId;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Status", "Pending");
                command.Parameters.AddWithValue("@BankId", bankId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var user = new User
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            PassportNumber = reader.GetString(2),
                            IdentificationNumber = reader.GetString(3),
                            Phone = reader.GetString(4),
                            Email = reader.GetString(5),
                            Password = reader.GetString(6),
                            Role = reader.GetInt32(7)
                        };

                        pendingUsers.Add(user);
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Log the exception (replace with proper logging in production)
        Console.WriteLine($"An error occurred: {ex.Message}");
    }

    return pendingUsers;
}
    // Helper method to hash passwords
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    // Helper method to verify passwords
    private bool VerifyPassword(string inputPassword, string hashedPassword)
    {
        return HashPassword(inputPassword) == hashedPassword;
    }
    
    public async Task<bool> AddExternalSpecialistAsync(int userId, int enterpriseId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            // SQL query to insert a new External Specialist
            var sql = @"
            INSERT INTO ExternalSpecialists (UserId, EnterpriseId)
            VALUES (@UserId, @EnterpriseId);";

            using (var command = new SqliteCommand(sql, connection))
            {
                // Add parameters
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@EnterpriseId", enterpriseId);

                // Execute the query
                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0; // Return true if the record was added successfully
            }
        }
    }
    
    public async Task<int> GetEnterpriseIdByUserIdAsync(int userId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            // SQL query to retrieve the EnterpriseId for a given UserId
            var sql = @"
            SELECT EnterpriseId 
            FROM ExternalSpecialists 
            WHERE UserId = @UserId;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                
                var result = await command.ExecuteScalarAsync();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }
        }

        return 0;
    }
    
    public async Task<List<Enterprise>> GetEnterprisesAsync(string bankId)
    {
        Console.WriteLine("Entering GetEnterprises");
        Console.WriteLine(bankId);
        var enterprises = new List<Enterprise>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            var sql = "SELECT * FROM Enterprises WHERE BIC = @BankId;";
            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@BankId", bankId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var enterprise = new Enterprise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            LegalName = reader.GetString(reader.GetOrdinal("LegalName")),
                            Type = reader.GetString(reader.GetOrdinal("Type")),
                            UNP = reader.GetString(reader.GetOrdinal("UNP")),
                            BIC = reader.GetString(reader.GetOrdinal("BIC")),
                            LegalAddress = reader.GetString(reader.GetOrdinal("LegalAddress"))
                        };

                        enterprises.Add(enterprise);
                    }
                }
            }
        }

        return enterprises;
    }
    
    public async Task<Enterprise> GetEnterpriseByIdAsync(int enterpriseId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
            SELECT Id, LegalName, Type, UNP, BIC, LegalAddress
            FROM Enterprises
            WHERE Id = @EnterpriseId;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@EnterpriseId", enterpriseId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Enterprise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            LegalName = reader.GetString(reader.GetOrdinal("LegalName")),
                            Type = reader.GetString(reader.GetOrdinal("Type")),
                            UNP = reader.GetString(reader.GetOrdinal("UNP")),
                            BIC = reader.GetString(reader.GetOrdinal("BIC")),
                            LegalAddress = reader.GetString(reader.GetOrdinal("LegalAddress"))
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
    
    public async Task<int> GetWorkerIdFromUserAsync(User user)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
            SELECT Id
            FROM Workers
            WHERE Full_Name = @FullName AND Email = @Email;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@Email", user.Email);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return reader.GetInt32(reader.GetOrdinal("Id"));
                    }
                    
                    return 0;
                }
            }
        }
    }
    
    public async Task<int> IsConnectedToSalaryAsync(int workerId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
            SELECT ConnectedToSalary
            FROM Workers
            WHERE Id = @WorkerId;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@WorkerId", workerId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return reader.GetInt32(reader.GetOrdinal("ConnectedToSalary"));
                    }
                    return -1;
                }
            }
        }
    }
    
    public async Task<int> ConnectToSalaryAsync(int workerId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
            UPDATE Workers 
            SET ConnectedToSalary = @ConnectedToSalary 
            WHERE Id = @WorkerId;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@WorkerId", workerId);
                command.Parameters.AddWithValue("@ConnectedToSalary", 1);
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return reader.GetInt32(reader.GetOrdinal("ConnectedToSalary"));
                    }
                    return -1;
                }
            }
        }
    }
    
    public async Task<int> DisconnectFromSalaryAsync(int workerId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
            UPDATE Workers 
            SET ConnectedToSalary = @ConnectedToSalary 
            WHERE Id = @WorkerId;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@WorkerId", workerId);
                command.Parameters.AddWithValue("@ConnectedToSalary", 0);
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return reader.GetInt32(reader.GetOrdinal("ConnectedToSalary"));
                    }
                    return -1;
                }
            }
        }
    }
    
    public async Task<List<Worker>> GetConnectedToSalaryWorkersAsync()
    {
        var connectedWorkers = new List<Worker>();

        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"
                SELECT * FROM Workers
                WHERE ConnectedToSalary = @ConnectedToSalary;";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ConnectedToSalary", 1);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var worker = new Worker
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Email = reader.GetString(2),
                                Position = reader.GetString(3),
                                Salary = reader.GetDouble(4)
                            };
                            connectedWorkers.Add(worker);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return connectedWorkers;
    }
    
    public async Task<int> GetEnterpriseIdByWorkerIdAsync(int workerId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            try
            {
                await connection.OpenAsync();

                var sql = @"
            SELECT EnterpriseId
            FROM WorkerEnterprise
            WHERE WorkerId = @WorkerId;";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@WorkerId", workerId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return reader.GetInt32(reader.GetOrdinal("EnterpriseId"));
                        }
                    
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}