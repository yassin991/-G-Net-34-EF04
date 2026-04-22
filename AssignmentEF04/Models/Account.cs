using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentEF04.Models
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public DateTime OpeningDate { get; set; }
        public decimal CurrentBalance { get; set; }

        public string BranchCode { get; set; }
        public Branch Branch { get; set; }

        public List<CustomerAccount> CustomerAccounts { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
