using JediBank.CurrencyFolder;

namespace JediBank
{
    internal class Loan
    {
        
        public decimal LoanAmount { get; set; }
        public int LoanId { get; set; }
        public Currency Currency { get; set; }
        public decimal Interest { get; set; }
        public decimal Total { get; set; }
        public decimal AmountPaid { get; set; }

        //Will create methods after structure is agreed upon. CreateLoan(), PayOff()?
        public void Pay(decimal amount)
        {
            Total -= amount;
            AmountPaid += amount;
        }

        public void CalculateTotal()
        {
            Total = LoanAmount * Interest;
        }

        public void CalculateInterest()
        {
            if (LoanAmount <= 10000)
            {
                Interest = 1.05m;
            }
            else if (LoanAmount <= 25000)
            {
                Interest = 1.04m;
            }
            else if (LoanAmount <= 50000)
            {
                Interest = 1.03m;
            }
            else if (LoanAmount <= 300000)
            {
                Interest = 1.02m;
            }
            else
            {
                Interest = 1.01m;
            }
        }
    }
}
