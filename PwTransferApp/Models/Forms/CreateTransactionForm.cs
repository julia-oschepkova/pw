using System;

namespace PwTransferApp.Models.Forms
{
    public class CreateTransactionForm
    {
        public Guid RecipientAccountId { get; set; }

        public double Amount { get; set; }

        public string Description { get; set; }
    }
}