using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Services;

public interface ISalaryProjectService
{
    Task<bool> SaveSalaryProjectAsync(SalaryProject salaryProject, string EnterpriseName);
    
    Task<SalaryProject> GetSalaryProjectByEnterpriseIdAsync(int enterpriseId);

    Task<bool> IncrementConnectedWorkersAsync(int salaryProjectId);

    Task<bool> DecrementConnectedWorkersAsync(int salaryProjectId);

    Task<List<SalaryProject>> GetAllSalaryProjectsAsync();

    Task<bool> ProcessSalaryProjectAsync(int salaryProjectId);

    Task<List<SalaryProject>> GetPendingSalaryProjectsAsync();

    Task<bool> RejectSalaryProjectAsync(int salaryProjectId);
}