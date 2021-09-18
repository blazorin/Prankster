using System;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using Shared.Enums;
using Shared.Utils;

namespace Model.Data
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        
        [Required]
        public Package Package { get; set; }
        
        [Required] public Store Store { get; set; }

        [Required] public decimal Amount { get; set; }
        
        [Required] public DateTime Date { get; set; }
        
        [Required] public StoreTransactionStatus TransactionStatus { get; set; }

        [StringLength(FieldLenghts.Transaction.Note)]
        public string AdminNote { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}