using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class Account
    {
        private static int _accountCounter = 0;
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }
        public double InterestRate { get; private set; }
        public bool IsBlocked { get; private set; }
        public bool IsFrozen { get; private set; }
        public Client Owner { get; private set; }

        public Account(Client owner, decimal initialBalance, double interestRate)
        {
            AccountNumber = "ACC" + (_accountCounter++).ToString();
            Balance = initialBalance;
            InterestRate = interestRate;
            Owner = owner;
        }

        public void Deposit(decimal amount)
        {
            if (IsBlocked || IsFrozen)
            {
                Console.WriteLine($"❌ Операция невозможна. Счет {AccountNumber} заблокирован или заморожен.");
                return;
            }

            Balance += amount;
            Console.WriteLine($"✅ Пополнение: {amount}. Новый баланс {Balance}");
        }

        public void Withdraw(decimal amount)
        {
            if (IsBlocked || IsFrozen)
            {
                Console.WriteLine($"❌ Операция невозможна. Счет {AccountNumber} заблокирован или заморожен.");
                return;
            }

            if (Balance >= amount)
            {
                Balance -= amount;
                Console.WriteLine($"✅ Снятие: {amount}. Остаток: {Balance}");
            }
            else
            {
                Console.WriteLine("❌ Недостаточно средств.");
            }
        }

        public void Transfer(Account target, decimal amount)
        {
            if (IsBlocked || IsFrozen || target.IsBlocked || target.IsFrozen)
            {
                Console.WriteLine($"❌ Операция невозможна. Один из счетов заблокирован или заморожен.");
                return;
            }

            if (Balance >= amount)
            {
                Balance -= amount;
                target.Deposit(amount);
                Console.WriteLine($"✅ Перевод {amount} со счета {AccountNumber} на {target.AccountNumber}. Баланс: {Balance}");
            }
            else
            {
                Console.WriteLine("❌ Недостаточно средств для перевода.");
            }
        }

        public void AccumulateInterest()
        {
            if (IsBlocked || IsFrozen)
            {
                Console.WriteLine($"❌ Операция невозможна. Счет {AccountNumber} заблокирован или заморожен.");
                return;
            }

            decimal interest = Balance * (decimal)(InterestRate / 100);
            Balance += interest;
            Console.WriteLine($"✅ Начислены проценты: {interest}. Новый баланс: {Balance}");
        }

        public void Block()
        {
            IsBlocked = true;
            Console.WriteLine($"❌ Счет {AccountNumber} заблокирован.");
        }

        public void Unblock()
        {
            IsBlocked = false;
            Console.WriteLine($"✅ Счет {AccountNumber} разблокирован.");
        }

        public void Freeze()
        {
            IsFrozen = true;
            Console.WriteLine($"❄️ Счет {AccountNumber} заморожен.");
        }

        public void Unfreeze()
        {
            IsFrozen = false;
            Console.WriteLine($"🔥 Счет {AccountNumber} разморожен.");
        }
    }