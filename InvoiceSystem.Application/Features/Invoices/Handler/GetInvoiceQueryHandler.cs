using AutoMapper;
using InvoiceSystem.Application.DTO;
using InvoiceSystem.Application.Features.Invoices.Commands;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Interfaces;
using MediatR;

namespace InvoiceSystem.Application.Features.Invoices.Handler;

public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, InvoiceDto>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;

    public GetInvoiceQueryHandler(IInvoiceRepository invoiceRepository, IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _mapper = mapper;
    }

    public async Task<InvoiceDto> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetInvoiceByIdAsync(request.Id);
        if (invoice == null) throw new NotFoundException(nameof(Invoice), request.Id);

        return _mapper.Map<InvoiceDto>(invoice);
    }
}