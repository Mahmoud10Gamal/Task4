using System.Security.Principal;

namespace Bank_Account
{
    public class Account
    {
        public string Name { get; set; }
        public double Balance { get; set; }

        public Account(string Name = "Unnamed Account", double Balance = 0.0)
        {
            this.Name = Name;
            this.Balance = Balance;
        }

        public virtual bool Deposit(double amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                return true;
            }
            return false;
        }

        public virtual bool Withdraw(double amount)
        {
            if (Balance - amount >= 0)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Balance: {Balance}";
        }

        public static double operator +(Account acc1, Account acc2)
        {
            return acc1.Balance + acc2.Balance;
        }
    }
    public class SavingsAccount : Account
    {
        public double InterestRate { get; set; }

        public SavingsAccount(string Name = "Unnamed Savings Account", double Balance = 0.0, double InterestRate = 3.0) : base(Name, Balance)
        {
            this.InterestRate = InterestRate;
        }

        public override bool Deposit(double amount)
        {
            if (base.Deposit(amount))
            {
                Balance += Balance * (InterestRate / 100);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return base.ToString() + $", Interest Rate: {InterestRate}";
        }
    }
    public class CheckingAccount : Account
    {
        public const double WithdrawalFee = 1.50;

        public CheckingAccount(string Name = "Unnamed Checking Account", double Balance = 0.0) : base(Name, Balance)
        {
        }

        public override bool Withdraw(double amount)
        {
            amount += WithdrawalFee;
            return base.Withdraw(amount);
        }

        public override string ToString()
        {
            return base.ToString() + $", Withdrawal Fee: {WithdrawalFee}";
        }
    }

    public class TrustAccount : SavingsAccount
    {
        public int WithdrawalCount { get; set; }
        public const int MaxWithdrawals = 3;

        public TrustAccount(string Name = "Unnamed Trust Account", double Balance = 0.0, double InterestRate = 3.0) : base(Name, Balance, InterestRate)
        {
        }

        public override bool Deposit(double amount)
        {
            if (amount >= 5000)
            {
                amount += 50;                   // Bonus
            }
            return base.Deposit(amount);
        }

        public override bool Withdraw(double amount)
        {
            if (WithdrawalCount >= MaxWithdrawals || amount > Balance * 0.2)
                return false;

            if (base.Withdraw(amount))
            {
                WithdrawalCount++;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return base.ToString() + $", Withdrawals Left: {MaxWithdrawals - WithdrawalCount}";
        }
    }
    public static class AccountUtil
    {
        // Utility helper functions for Account class

        public static void Display(List<Account> accounts)
        {
            Console.WriteLine("\n=== Accounts ==========================================");
            foreach (var acc in accounts)
            {
                Console.WriteLine(acc);
            }
        }

        public static void Deposit(List<Account> accounts, double amount)
        {
            Console.WriteLine("\n=== Depositing to Accounts =================================");
            foreach (var acc in accounts)
            {
                if (acc.Deposit(amount))
                    Console.WriteLine($"Deposited {amount} to {acc}");
                else
                    Console.WriteLine($"Failed Deposit of {amount} to {acc}");
            }
        }

        public static void Withdraw(List<Account> accounts, double amount)
        {
            Console.WriteLine("\n=== Withdrawing from Accounts ==============================");
            foreach (var acc in accounts)
            {
                if (acc.Withdraw(amount))
                    Console.WriteLine($"Withdrew {amount} from {acc}");
                else
                    Console.WriteLine($"Failed Withdrawal of {amount} from {acc}");
            }
        }
    }
    public class Program
    {
        static void Main()
        {
            // Accounts
            var accounts = new List<Account>
            {
                new Account(),
                new Account("Larry"),
                new Account("Moe", 2000),
                new Account("Curly", 5000)
            };

            AccountUtil.Display(accounts);
            AccountUtil.Deposit(accounts, 1000);
            AccountUtil.Withdraw(accounts, 2000);

            // Savings
            var savAccounts = new List<Account>
            {
                new SavingsAccount(),
                new SavingsAccount("Superman"),
                new SavingsAccount("Batman", 2000),
                new SavingsAccount("Wonderwoman", 5000, 5.0)
            };

            AccountUtil.Display(savAccounts);
            AccountUtil.Deposit(savAccounts, 1000);
            AccountUtil.Withdraw(savAccounts, 2000);

            // Checking
            var checAccounts = new List<Account>
            {
                new CheckingAccount(),
                new CheckingAccount("Larry2"),
                new CheckingAccount("Moe2", 2000),
                new CheckingAccount("Curly2", 5000)
            };

            AccountUtil.Display(checAccounts);
            AccountUtil.Deposit(checAccounts, 1000);
            AccountUtil.Withdraw(checAccounts, 2000);

            // Trust
            var trustAccounts = new List<Account>
            {
                new TrustAccount(),
                new TrustAccount("Superman2"),
                new TrustAccount("Batman2", 2000),
                new TrustAccount("Wonderwoman2", 5000, 5.0)
            };

            AccountUtil.Display(trustAccounts);
            AccountUtil.Deposit(trustAccounts, 1000);
            AccountUtil.Withdraw(trustAccounts, 2000);

            // Operator Overloading
            double totalBalance = 0;
            foreach (var account in accounts)
            {
                totalBalance += account.Balance;
            }
            Console.WriteLine($"Total balance of all accounts: {totalBalance}");

            Console.WriteLine();
        }
    }
}