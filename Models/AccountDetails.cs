using System;

namespace AccountLibrary.API.Models
{
    public class AccountDetails
    {
        public string AccountIdentifier { get; set; }
        public string AccountType { get; set; }
        public string AccountSubType { get; set; }
        
        public string AccountHolderName { get; set; }
        public string AccountStatus { get; set; }
        
        public string AccountName { get; set; }
        public string OpenDate { get; set; }
        public string SortCode { get; set; }
        public string ProductName { get; set; }
        public double AvailableBalance { get; set; }
        public double LedgerBalance { get; set; }

    }

}
