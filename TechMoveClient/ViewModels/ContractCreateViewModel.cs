using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using TechMoveClient.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechMoveClient.ViewModels
{
    public class ContractCreateViewModel
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        public string ServiceLevel { get; set; } // Standard or Express

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public ContractStatus Status { get; set; }

        public IFormFile? AgreementFile { get; set; }

        public List<SelectListItem> Clients { get; set; } = new();
    }
}