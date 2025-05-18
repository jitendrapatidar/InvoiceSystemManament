 
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Domain.Entities;
public partial class Payment
{
    [Key]
    public int PaymentId { get; set; }

    public int InvoiceId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public DateOnly PaymentDate { get; set; }

    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; }

    [StringLength(50)]
    public string ReferenceNumber { get; set; }

    [StringLength(255)]
    public string Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("Payments")]
    public virtual Invoice Invoice { get; set; }
}