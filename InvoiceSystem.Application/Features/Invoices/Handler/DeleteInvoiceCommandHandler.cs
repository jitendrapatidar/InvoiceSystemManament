using AutoMapper;
using Azure.Core;
using InvoiceSystem.Application.Features.Invoices.Commands;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Interfaces;
using MediatR;

namespace InvoiceSystem.Application.Features.Invoices.Handler;

public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, Unit>
{
    private readonly IInvoiceRepository _invoiceRepository;

    public DeleteInvoiceCommandHandler(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }
    public async Task<Unit> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetInvoiceByIdAsync(request.Id);
        if (invoice == null) throw new NotFoundException(nameof(Invoice), request.Id);

        await _invoiceRepository.DeleteInvoiceAsync(request.Id);
        return Unit.Value;
    }
}
