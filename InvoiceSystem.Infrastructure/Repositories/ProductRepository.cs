using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Interfaces;
using InvoiceSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Infrastructure.Repositories;

/// <summary>
/// Repository for managing <see cref="Product"/> entities.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly ReadInvoiceSystemDbContext _ReadInvoicecontext;
    private readonly WriteInvoiceSystemDbContext _WriteInvoicecontext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class.
    /// </summary>
    /// <param name="ReadInvoicecontext">The read-only database context.</param>
    /// <param name="WriteInvoicecontext">The write-enabled database context.</param>
    public ProductRepository(ReadInvoiceSystemDbContext ReadInvoicecontext, WriteInvoiceSystemDbContext WriteInvoicecontext)
    {
        _ReadInvoicecontext = ReadInvoicecontext;
        _WriteInvoicecontext = WriteInvoicecontext;
    }

    /// <summary>
    /// Gets a product by its identifier.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>The product if found; otherwise, null.</returns>
    public async Task<Product> GetByIdAsync(int id)
    {
        return await _ReadInvoicecontext.Products
            .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    /// <summary>
    /// Gets all products ordered by name.
    /// </summary>
    /// <returns>A collection of all products.</returns>
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _ReadInvoicecontext.Products
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Gets all active products ordered by name.
    /// </summary>
    /// <returns>A collection of active products.</returns>
    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _ReadInvoicecontext.Products
            .Where(p => p.IsActive.Value)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new product.
    /// </summary>
    /// <param name="product">The product to add.</param>
    public async Task AddAsync(Product product)
    {
        await _WriteInvoicecontext.Products.AddAsync(product);
        await _WriteInvoicecontext.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="product">The product to update.</param>
    public async Task UpdateAsync(Product product)
    {
        _WriteInvoicecontext.Products.Update(product);
        await _WriteInvoicecontext.SaveChangesAsync();
    }

    /// <summary>
    /// Soft deletes a product by marking it as inactive.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    public async Task DeleteAsync(int id)
    {
        var product = await GetByIdAsync(id);
        if (product != null)
        {
            // Soft delete by marking as inactive
            product.IsActive = false;
            await UpdateAsync(product);

            // Alternatively for hard delete:
            // _context.Products.Remove(product);
            // await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Checks if a product exists by its identifier.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>True if the product exists; otherwise, false.</returns>
    public async Task<bool> ProductExistsAsync(int id)
    {
        return await _ReadInvoicecontext.Products.AnyAsync(p => p.ProductId == id);
    }
}
