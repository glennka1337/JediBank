namespace JediBank
{
    internal class Loan
    {
        public string LoanId { get; set; }
        //public Currency Currency { get; set; }
        public decimal Interest { get; set; }
        public decimal Total { get; set; }
        public decimal AmountPaid { get; set; }

        //Will create methods after structure is agreed upon. CreateLoan(), PayOff()?
    }
}
