using InvoiceSystem.Application.DTO;
using InvoiceSystem.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Application.Features.Invoices.Commands
{
    public class CreateInvoiceCommand : IRequest<int>
    {
        public int CustomerId { get; set; }
        public string PaymentTerms { get; set; } = "Net 30";
        public string Notes { get; set; } = string.Empty;
        public List<InvoiceItemDto> Items { get; set; } = new();
    }

    public class UpdateInvoiceCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string? PaymentTerms { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }
        public List<InvoiceItemDto>? Items { get; set; }
    }

    public class DeleteInvoiceCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class MarkInvoiceAsPaidCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "Bank Transfer";
        public string? ReferenceNumber { get; set; }
    }

    public class GetInvoiceQuery : IRequest<InvoiceDto>
    {
        public int Id { get; set; }
    }
    
}

