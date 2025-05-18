using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Application.DTO;

public class InvoiceDto
{
    public int Id { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public string? Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string? CustomerName { get; set; }
}

