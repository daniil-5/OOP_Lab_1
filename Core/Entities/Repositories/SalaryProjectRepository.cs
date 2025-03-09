using Microsoft.Data.Sqlite;

namespace OOP_Lab_1.Core.Entities.Repositories;

public class SalaryProjectRepository : ISalaryProjectRepository
{
    
    private readonly string _connectionString;

    public SalaryProjectRepository(string databasePath)
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

    public async Task<bool> SaveSalaryProjectAsync(SalaryProject salaryProject, string EnterpriseName)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            var existingProject = await GetSalaryProjectByEnterpriseIdAsync(salaryProject.EnterpriseId);
            if (existingProject != null)
            {
                return false;
            }

            // If no existing project, proceed with the insertion
            var sql = @"
        INSERT INTO SalaryProjects (IsProcessed, EnterpriseId, ApprovedDate, Period, ConnectedWorkers, EnterpriseName)
        VALUES (@IsProcessed, @EnterpriseId, @ApprovedDate, @Period, @ConnectedWorkers, @EnterpriseName);";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@IsProcessed", salaryProject.IsProcessed);
                command.Parameters.AddWithValue("@EnterpriseId", salaryProject.EnterpriseId);
                command.Parameters.AddWithValue("@ApprovedDate", salaryProject.ApprovedDate);
                command.Parameters.AddWithValue("@Period", salaryProject.Period);
                command.Parameters.AddWithValue("@ConnectedWorkers", 0);
                command.Parameters.AddWithValue("@EnterpriseName", EnterpriseName);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
    
    public async Task<SalaryProject> GetSalaryProjectByEnterpriseIdAsync(int enterpriseId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
        SELECT *
        FROM SalaryProjects
        WHERE EnterpriseId = @EnterpriseId;";
            try
            {
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EnterpriseId", enterpriseId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var salaryProject = new SalaryProject
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                IsProcessed = reader.GetBoolean(reader.GetOrdinal("IsProcessed")),
                                EnterpriseId = reader.GetInt32(reader.GetOrdinal("EnterpriseId")),
                                ApprovedDate = reader.GetDateTime(reader.GetOrdinal("ApprovedDate")),
                                Period = reader.GetInt32(reader.GetOrdinal("Period")),
                                EnterpriseName = reader.GetString(reader.GetOrdinal("EnterpriseName"))
                            };
                            return salaryProject;
                        }
                        return null;
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
    
    public async Task<bool> IncrementConnectedWorkersAsync(int salaryProjectId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
            UPDATE SalaryProjects
            SET ConnectedWorkers = ConnectedWorkers + 1
            WHERE Id = @SalaryProjectId;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@SalaryProjectId", salaryProjectId);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
    public async Task<bool> DecrementConnectedWorkersAsync(int salaryProjectId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
            UPDATE SalaryProjects
            SET ConnectedWorkers = ConnectedWorkers - 1
            WHERE Id = @SalaryProjectId;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@SalaryProjectId", salaryProjectId);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected >= 0;
            }
        }
    }
    
    public async Task<List<SalaryProject>> GetAllSalaryProjectsAsync()
    {
        var salaryProjects = new List<SalaryProject>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
            SELECT Id, IsProcessed, EnterpriseId, ApprovedDate, Period, ConnectedWorkers, EnterpriseName
            FROM SalaryProjects;";

            using (var command = new SqliteCommand(sql, connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var salaryProject = new SalaryProject
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        IsProcessed = reader.GetBoolean(reader.GetOrdinal("IsProcessed")),
                        EnterpriseId = reader.GetInt32(reader.GetOrdinal("EnterpriseId")),
                        ApprovedDate = reader.GetDateTime(reader.GetOrdinal("ApprovedDate")),
                        Period = reader.GetInt32(reader.GetOrdinal("Period")),
                        EnterpriseName = reader.GetString(reader.GetOrdinal("EnterpriseName"))
                    };
                    salaryProjects.Add(salaryProject);
                }
            }
        }

        return salaryProjects;
    }
    
    public async Task<bool> ProcessSalaryProjectAsync(int salaryProjectId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
            UPDATE SalaryProjects
            SET IsProcessed = 1
            WHERE Id = @SalaryProjectId;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@SalaryProjectId", salaryProjectId);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected >= 0;
            }
        }
    }
    
    public async Task<List<SalaryProject>> GetPendingSalaryProjectsAsync()
    {
        var salaryProjects = new List<SalaryProject>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
            SELECT Id, IsProcessed, EnterpriseId, ApprovedDate, Period, ConnectedWorkers, EnterpriseName
            FROM SalaryProjects WHERE IsProcessed = 0;";

            using (var command = new SqliteCommand(sql, connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var salaryProject = new SalaryProject
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        IsProcessed = reader.GetBoolean(reader.GetOrdinal("IsProcessed")),
                        EnterpriseId = reader.GetInt32(reader.GetOrdinal("EnterpriseId")),
                        ApprovedDate = reader.GetDateTime(reader.GetOrdinal("ApprovedDate")),
                        Period = reader.GetInt32(reader.GetOrdinal("Period")),
                        EnterpriseName = reader.GetString(reader.GetOrdinal("EnterpriseName"))
                    };
                    salaryProjects.Add(salaryProject);
                }
            }
        }

        return salaryProjects;
    }
    
    public async Task<bool> RejectSalaryProjectAsync(int salaryProjectId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
            DELETE FROM SalaryProjects
            WHERE Id = @SalaryProjectId;";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@SalaryProjectId", salaryProjectId);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected >= 0;
            }
        }
    }
}