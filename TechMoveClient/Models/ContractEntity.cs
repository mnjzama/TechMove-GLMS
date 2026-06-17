using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMoveClient.Models
{
    public class ContractEntity
    {
        public int ContractId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ContractStatus Status { get; set; }
        public string ServiceLevel { get; set; }
        public string? AgreementFilePath { get; set; }
        public int ClientId { get; set; }
        public string? ClientName { get; set; }
    }

    public enum ContractStatus
    {
        Draft,
        Active,
        Expired,
        OnHold
    }
}