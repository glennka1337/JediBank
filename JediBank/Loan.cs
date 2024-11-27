namespace JediBank
{
    internal class Loan
    {
        public decimal Amount { get; set; }
        public string LoanId { get; set; }
        //public Currency Currency { get; set; }
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
            Total = Amount * Interest;
        }
    }
}
