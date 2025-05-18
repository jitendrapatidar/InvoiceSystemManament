using InvoiceSystem.Application.DTO;
using MediatR;
using System.Collections.Generic;

namespace InvoiceSystem.Application.Queries;

public class GetAllInvoicesQuery : IRequest<List<InvoiceDto>> { }

public class GetInvoiceQuery : IRequest<InvoiceDto>
{
    public int Id { get; set; }
}