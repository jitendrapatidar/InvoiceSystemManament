using AutoMapper;
using InvoiceSystem.Application.Features.Invoices.Commands;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Interfaces;
using MediatR;
namespace InvoiceSystem.Application.Features.Invoices.Handler;

/// <summary>
/// Handles the creation of a new invoice, including adding invoice items, calculating totals, and saving the invoice.
/// </summary>
public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, int>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateInvoiceCommandHandler"/> class.
    /// </summary>
    /// <param name="invoiceRepository">The invoice repository.</param>
    /// <param name="productRepository">The product repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public CreateInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = _mapper.Map<Invoice>(request);
        invoice.InvoiceNumber = GenerateInvoiceNumber();
        invoice.IssueDate = DateOnly.FromDateTime(DateTime.UtcNow);
        invoice.DueDate = DateOnly.FromDateTime(CalculateDueDate(request.PaymentTerms));
        invoice.Status = "Draft";

        foreach (var itemDto in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
            var item = new InvoiceItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = product.UnitPrice,
                TaxRate = product.TaxRate,
                Description = itemDto.Description,
                LineTotal = CalculateLineTotal(itemDto.Quantity, product.UnitPrice, product.TaxRate)
            };
            invoice.InvoiceItems.Add(item);
        }

        await _invoiceRepository.CalculateInvoiceTotalsAsync(invoice);
        await _invoiceRepository.AddInvoiceAsync(invoice);

        return invoice.InvoiceId;
    }

    private string GenerateInvoiceNumber() => $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpper()}";
    private DateTime CalculateDueDate(string paymentTerms) => paymentTerms switch
    {
        "Net 15" => DateTime.UtcNow.AddDays(15),
        "Net 30" => DateTime.UtcNow.AddDays(30),
        "Net 60" => DateTime.UtcNow.AddDays(60),
        _ => DateTime.UtcNow.AddDays(30)
    };
    private decimal CalculateLineTotal(int quantity, decimal unitPrice, decimal taxRate) =>
        quantity * unitPrice * (1 + taxRate / 100);
  
}