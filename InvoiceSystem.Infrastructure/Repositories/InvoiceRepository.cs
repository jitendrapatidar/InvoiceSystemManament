using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Interfaces;
using InvoiceSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Invoice entities in the Invoice System.
/// </summary>
public class InvoiceRepository : IInvoiceRepository
{
    private readonly ReadInvoiceSystemDbContext _ReadInvoicecontext;
    private readonly WriteInvoiceSystemDbContext _WriteInvoicecontext;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvoiceRepository"/> class.
    /// </summary>
    /// <param name="ReadInvoicecontext">The read-only database context.</param>
    /// <param name="WriteInvoicecontext">The write-enabled database context.</param>
    public InvoiceRepository(ReadInvoiceSystemDbContext ReadInvoicecontext, WriteInvoiceSystemDbContext WriteInvoicecontext)
    {
        _ReadInvoicecontext = ReadInvoicecontext;
        _WriteInvoicecontext = WriteInvoicecontext;
    }

    /// <inheritdoc/>
    public async Task<Invoice> GetInvoiceByIdAsync(int id)
    {
        var reuslt = await _ReadInvoicecontext.Invoices.AsNoTracking()
            .Include(i => i.Customer)
            .Include(i => i.InvoiceItems)
                .ThenInclude(item => item.Product)
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);
        return reuslt != null ? reuslt : new Invoice();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Invoice>> GetInvoicesByCustomerAsync(int customerId)
    {
        return await _ReadInvoicecontext.Invoices
            .Where(i => i.CustomerId == customerId)
            .Include(i => i.Customer)
            .OrderByDescending(i => i.IssueDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
    {
        return await _ReadInvoicecontext.Invoices
            .Include(i => i.Customer)
            .OrderByDescending(i => i.IssueDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task AddInvoiceAsync(Invoice invoice)
    {
        await _WriteInvoicecontext.Invoices.AddAsync(invoice);
        await _WriteInvoicecontext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateInvoiceAsync(Invoice invoice)
    {
        _WriteInvoicecontext.Invoices.Update(invoice);
        await _WriteInvoicecontext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteInvoiceAsync(int id)
    {
        var invoice = await GetInvoiceByIdAsync(id);
        if (invoice != null)
        {
            _WriteInvoicecontext.Invoices.Remove(invoice);
            await _WriteInvoicecontext.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task<bool> InvoiceExistsAsync(int id)
    {
        return await _ReadInvoicecontext.Invoices.AnyAsync(e => e.InvoiceId == id);
    }

    /// <inheritdoc/>
    public async Task CalculateInvoiceTotalsAsync(Invoice invoice)
    {
        if (invoice.InvoiceItems == null || !invoice.InvoiceItems.Any())
        {
            invoice.Subtotal = 0;
            invoice.TaxAmount = 0;
            invoice.TotalAmount = 0;
            return;
        }

        // Calculate subtotal (sum of all line items before tax)
        invoice.Subtotal = invoice.InvoiceItems.Sum(item => item.Quantity * item.UnitPrice);

        // Calculate tax amount (sum of all taxes for each line item)
        invoice.TaxAmount = invoice.InvoiceItems.Sum(item =>
            (item.Quantity * item.UnitPrice) * (item.TaxRate / 100));

        // Calculate total amount (subtotal + tax)
        invoice.TotalAmount = invoice.Subtotal + invoice.TaxAmount;

        // Update invoice status based on payments if needed
        if (invoice.Payments != null && invoice.Payments.Any())
        {
            var totalPaid = invoice.Payments.Sum(p => p.Amount);
            if (totalPaid >= invoice.TotalAmount)
            {
                invoice.Status = "Paid";
            }
            else if (totalPaid > 0)
            {
                invoice.Status = "Partially Paid";
            }
        }

        invoice.UpdatedAt = DateTime.UtcNow;
    }

     
}
