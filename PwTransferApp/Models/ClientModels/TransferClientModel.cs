using System;

namespace PwTransferApp.Models.ClientModels
{
    public class TransferClientModel
    {
        public Guid Id { get; set; }

        public UserClientModel Counterpart { get; set; }

        public PwAccountClientModel CounterpartAccount { get; set; }

        public double Amount { get; set; }

        public TransferDirection Direction { get; set; }

        public double Balance { get; set; }

        public string Description { get; set; }
    }
}