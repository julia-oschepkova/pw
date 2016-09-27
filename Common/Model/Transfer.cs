using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Model
{
    public class Transfer : Entity
    {
        [Index("Ids", Order = 1)]
        public Guid SourceAccountId { get; set; }

        [Index("Ids", Order = 2)]
        public Guid DestinationAccountId { get; set; }

        public double Amount { get; set; }

        public string  Description { get; set; }

        public TransferStatus Status { get; set; }

        public DateTimeOffset TransferDateTime { get; set; }
    }
}