namespace OOP_Lab_1.Core.Entities.Repositories;

public interface IBankRepository
{
    Task<bool> CreateBankTableAsync(string id);

    Task<List<(string BankId, string BankName)>> GetBanksAsync();

    Task<bool> AddBankAsync(Bank bank);

    Task<bool> RemoveBankAsync(string bankName);

    Task<bool> UpdateBankAsync(Bank bank);
}