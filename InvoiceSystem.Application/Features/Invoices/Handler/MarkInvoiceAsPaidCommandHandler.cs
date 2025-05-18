using AutoMapper;
using Azure.Core;
using InvoiceSystem.Application.Features.Invoices.Commands;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Interfaces;
using MediatR;

namespace InvoiceSystem.Application.Features.Invoices.Handler;

public class MarkInvoiceAsPaidCommandHandler : IRequestHandler<MarkInvoiceAsPaidCommand, Unit>
{
    private readonly IInvoiceRepository _invoiceRepository;

    public MarkInvoiceAsPaidCommandHandler(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<Unit> Handle(MarkInvoiceAsPaidCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetInvoiceByIdAsync(request.Id);
        if (invoice == null) throw new NotFoundException(nameof(Invoice), request.Id);

        invoice.Payments.Add(new Payment
        {
            Amount = request.Amount,
            PaymentDate = DateOnly.FromDateTime(DateTime.UtcNow),
            PaymentMethod = request.PaymentMethod,
            ReferenceNumber = request.ReferenceNumber
        });

        await _invoiceRepository.CalculateInvoiceTotalsAsync(invoice);
        await _invoiceRepository.UpdateInvoiceAsync(invoice);

        return Unit.Value;
    }
}
