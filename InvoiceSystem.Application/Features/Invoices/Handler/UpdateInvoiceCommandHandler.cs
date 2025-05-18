using AutoMapper;
using Azure.Core;
using InvoiceSystem.Application.Features.Invoices.Commands;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Interfaces;
using MediatR;

namespace InvoiceSystem.Application.Features.Invoices.Handler;

public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, Unit>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetInvoiceByIdAsync(request.Id);
        if (invoice == null) throw new NotFoundException(nameof(Invoice), request.Id);

        if (request.PaymentTerms != null) invoice.PaymentTerms = request.PaymentTerms;
        if (request.Notes != null) invoice.Notes = request.Notes;
        if (request.Status != null) invoice.Status = request.Status;

        if (request.Items != null)
        {
            invoice.InvoiceItems.Clear();
            foreach (var itemDto in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                invoice.InvoiceItems.Add(new InvoiceItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.UnitPrice,
                    TaxRate = product.TaxRate,
                    Description = itemDto.Description,
                    LineTotal = itemDto.Quantity * product.UnitPrice * (1 + product.TaxRate / 100)
                });
            }
        }

        await _invoiceRepository.CalculateInvoiceTotalsAsync(invoice);
        await _invoiceRepository.UpdateInvoiceAsync(invoice);

        return Unit.Value;
    }

}
