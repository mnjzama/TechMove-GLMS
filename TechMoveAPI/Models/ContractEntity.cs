using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMoveAPI.Models
{
    public class ContractEntity
    {
        [Key]
        public int ContractId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public ContractStatus Status { get; set; }

        [Required]
        public string ServiceLevel { get; set; }

        public string? AgreementFilePath { get; set; }

        // FK
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        // Navigation Property
        public ICollection<ServiceRequest> ServiceRequests { get; set; }
    }

    public enum ContractStatus
    {
        Draft,
        Active,
        Expired,
        OnHold
    }
}