using Microsoft.Data.Sqlite;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Entities.Repositories;
using OOP_Lab_1.Core.Interfaces;
using Serilog;

namespace OOP_Lab_1.Core.Services;

public class SalaryProjectService : ISalaryProjectService
{
    private readonly ISalaryProjectRepository _salaryRepository;

    public SalaryProjectService(ISalaryProjectRepository salaryRepository)
    {
        _salaryRepository = salaryRepository ?? throw new ArgumentNullException(nameof(salaryRepository));
    }
    
    public async Task<bool> SaveSalaryProjectAsync(SalaryProject salaryProject, string EnterpriseName)
    {
        Log.Information("Saving salary project for Enterprise: {EnterpriseName}", EnterpriseName);
        var result = await _salaryRepository.SaveSalaryProjectAsync(salaryProject, EnterpriseName);
        Log.Information("Salary project saved: {Result}", result);
        return result;
    }

    public async Task<SalaryProject> GetSalaryProjectByEnterpriseIdAsync(int enterpriseId)
    {
        if (enterpriseId <= 0) throw new ArgumentException("Enterprise id must be greater than 0");
        Log.Information("Fetching salary project for Enterprise ID: {EnterpriseId}", enterpriseId);
        var result = await _salaryRepository.GetSalaryProjectByEnterpriseIdAsync(enterpriseId);
        Log.Information("Fetched salary project: {SalaryProject}", result);
        return result;
    }

    public async Task<bool> IncrementConnectedWorkersAsync(int salaryProjectId)
    {
        if (salaryProjectId <= 0) throw new ArgumentException("Salary project id must be greater than 0");
        
        Log.Information("Incrementing connected workers for Salary Project ID: {SalaryProjectId}", salaryProjectId);
        var result = await _salaryRepository.IncrementConnectedWorkersAsync(salaryProjectId);
        Log.Information("Incremented connected workers: {Result}", result);
        return result;
    }
    
    public async Task<bool> DecrementConnectedWorkersAsync(int salaryProjectId)
    {
        if (salaryProjectId <= 0) throw new ArgumentException("Salary project id must be greater than 0");
        
        Log.Information("Decrementing connected workers for Salary Project ID: {SalaryProjectId}", salaryProjectId);
        var result = await _salaryRepository.DecrementConnectedWorkersAsync(salaryProjectId);
        Log.Information("Deccremented connected workers: {Result}", result);
        return result;
    }

    public async Task<List<SalaryProject>> GetAllSalaryProjectsAsync()
    {
        Log.Information("Fetching all salary projects");
        var result = await _salaryRepository.GetAllSalaryProjectsAsync();
        Log.Information("Fetched {Count} salary projects", result.Count);
        return result;
    }

    public async Task<bool> ProcessSalaryProjectAsync(int salaryProjectId)
    {
        if (salaryProjectId <= 0) throw new ArgumentException("Salary project id must be greater than 0");
        
        Log.Information("Processing salary project with ID: {SalaryProjectId}", salaryProjectId);
        var result = await _salaryRepository.ProcessSalaryProjectAsync(salaryProjectId);
        Log.Information("Salary project processed: {Result}", result);
        return result;
    }

    public async Task<List<SalaryProject>> GetPendingSalaryProjectsAsync()
    {
        Log.Information("Fetching pending salary projects");
        var result = await _salaryRepository.GetPendingSalaryProjectsAsync();
        Log.Information("Fetched {Count} pending salary projects", result.Count);
        return result;
    }

    public async Task<bool> RejectSalaryProjectAsync(int salaryProjectId)
    {
        if (salaryProjectId <= 0) throw new ArgumentException("Salary project id must be greater than 0");
        
        Log.Warning("Rejecting salary project with ID: {SalaryProjectId}", salaryProjectId);
        var result = await _salaryRepository.RejectSalaryProjectAsync(salaryProjectId);
        Log.Warning("Salary project rejection result: {Result}", result);
        return result;
    }
}