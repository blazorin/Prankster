using System;
using Shared.Enums;

namespace Shared.Dto
{
    public class AdminTransactionDto
    {
        public string TransactionId { get; set; }
        public PackageDto Package { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public StoreTransactionStatus TransactionStatus { get; set; }
        public string AdminNote { get; set; }
    }
}