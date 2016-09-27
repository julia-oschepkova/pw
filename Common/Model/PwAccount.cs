using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Model
{
    public class PwAccount : Entity
    {
        [Index]
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public double Balance { get; set; }

        public bool IsDefault { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}