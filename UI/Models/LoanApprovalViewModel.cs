using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace OOP_Lab_1.UI.Models
{
    public class LoanApprovalViewModel : BaseViewModel
    {
        private readonly ILoanService _loanService;
        private string _tableName;

        public ObservableCollection<Loan> Loans { get; set; }
        
        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        public LoanApprovalViewModel(ILoanService loanService)
        {
            _loanService = loanService;
            Loans = new ObservableCollection<Loan>();

            ApproveCommand = new Command<Loan>(async (loan) => await ApproveLoan(loan));
            RejectCommand = new Command<Loan>(async (loan) => await RejectLoan(loan));
        }

        // Apply query to get the Bank ID
        public void ApplyBankId(string bankId)
        {
            _tableName = $"{bankId}_Loans";
            LoadLoans();
        }

        // Load Pending Loans from the Service
        private async void LoadLoans()
        {
            var pendingLoans = await _loanService.GetPendingLoansAsync(_tableName);
            Loans.Clear();
            foreach (var loan in pendingLoans)
            {
                Loans.Add(loan);
            }
        }

        // Approve Loan
        private async Task ApproveLoan(Loan loan)
        {
            Console.WriteLine(_tableName);
            await _loanService.UpdateLoanApprovalStatusAsync(_tableName, loan.LoanId, true);
            Loans.Remove(loan); // Remove from the list after approval
        }

        // Reject Loan
        private async Task RejectLoan(Loan loan)
        {
            Console.WriteLine("Rejecting loan...");
            // await _loanService.UpdateLoanApprovalStatusAsync(_tableName, loan.LoanId, false);
            // await _loanService.CancelLoanAsync(_tableName, loan.LoanId);
            Loans.Remove(loan); // Remove from the list after rejection
        }
    }
}
