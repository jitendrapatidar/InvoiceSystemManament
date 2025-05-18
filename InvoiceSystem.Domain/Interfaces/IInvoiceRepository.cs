using InvoiceSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Domain.Interfaces;

public interface IInvoiceRepository
{
    Task<Invoice> GetInvoiceByIdAsync(int id);
    Task<IEnumerable<Invoice>> GetInvoicesByCustomerAsync(int customerId);
    Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
    Task AddInvoiceAsync(Invoice invoice);
    Task UpdateInvoiceAsync(Invoice invoice);
    Task DeleteInvoiceAsync(int id);
    Task<bool> InvoiceExistsAsync(int id);
    Task CalculateInvoiceTotalsAsync(Invoice invoice);
}
