using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using OOP_Lab_1.Core.Entities.Repositories;
using Serilog;

namespace OOP_Lab_1.Core.Services;

public class BanksService : IBanksService
{
    private readonly IBankRepository _bankRepository;
    private readonly ILogger _logger;

    public BanksService(IBankRepository bankRepository)
    {
        _bankRepository = bankRepository;
        _logger = Log.ForContext<BanksService>();
    }

    public async Task<bool> CreateBankTableAsync(string id)
    {
        _logger.Information("Creating bank table for Bank ID: {BankId}", id);

        try
        {
            bool result = await _bankRepository.CreateBankTableAsync(id);

            if (result)
            {
                _logger.Information("Bank table created successfully for Bank ID: {BankId}", id);
            }
            else
            {
                _logger.Warning("Failed to create bank table for Bank ID: {BankId}", id);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while creating bank table for Bank ID: {BankId}", id);
            throw;
        }
    }

    public async Task<List<(string BankId, string BankName)>> GetBanksAsync()
    {
        _logger.Information("Fetching list of banks.");

        try
        {
            var banks = await _bankRepository.GetBanksAsync();

            _logger.Information("Successfully fetched {BankCount} banks.", banks.Count);

            return banks;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while fetching banks.");
            throw;
        }
    }

    public async Task<bool> AddBankAsync(Bank bank)
    {
        _logger.Information("Adding new bank: {BankName} (BIC: {BIC})", bank.Name, bank.BIC);

        try
        {
            bool result = await _bankRepository.AddBankAsync(bank);

            if (result)
            {
                _logger.Information("Bank {BankName} (BIC: {BIC}) added successfully.", bank.Name, bank.BIC);
            }
            else
            {
                _logger.Warning("Failed to add bank {BankName} (BIC: {BIC}).", bank.Name, bank.BIC);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while adding bank {BankName} (BIC: {BIC}).", bank.Name, bank.BIC);
            throw;
        }
    }

    public async Task<bool> RemoveBankAsync(string bankName)
    {
        _logger.Information("Removing bank: {BankName}", bankName);

        if (bankName == null)
        {
            _logger.Error("Bank name is null.");
            throw new ArgumentNullException(nameof(bankName));
        }

        try
        {
            bool result = await _bankRepository.RemoveBankAsync(bankName);

            if (result)
            {
                _logger.Information("Bank {BankName} removed successfully.", bankName);
            }
            else
            {
                _logger.Warning("Failed to remove bank {BankName}.", bankName);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while removing bank {BankName}.", bankName);
            throw;
        }
    }

    public async Task<bool> UpdateBankAsync(Bank bank)
    {
        _logger.Information("Updating bank: {BankName} (BIC: {BIC})", bank.Name, bank.BIC);

        try
        {
            bool result = await _bankRepository.UpdateBankAsync(bank);

            if (result)
            {
                _logger.Information("Bank {BankName} (BIC: {BIC}) updated successfully.", bank.Name, bank.BIC);
            }
            else
            {
                _logger.Warning("Failed to update bank {BankName} (BIC: {BIC}).", bank.Name, bank.BIC);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while updating bank {BankName} (BIC: {BankId}).", bank.Name, bank.BIC);
            throw;
        }
    }
}