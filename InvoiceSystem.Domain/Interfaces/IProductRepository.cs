using InvoiceSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Domain.Interfaces;

/// <summary>
/// Defines methods for accessing and managing <see cref="Product"/> entities in the repository.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Gets a product by its unique identifier.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>The product with the specified identifier, or null if not found.</returns>
    Task<Product> GetByIdAsync(int id);

    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <returns>An enumerable collection of all products.</returns>
    Task<IEnumerable<Product>> GetAllAsync();

    /// <summary>
    /// Gets all active products.
    /// </summary>
    /// <returns>An enumerable collection of active products.</returns>
    Task<IEnumerable<Product>> GetActiveProductsAsync();

    /// <summary>
    /// Adds a new product to the repository.
    /// </summary>
    /// <param name="product">The product to add.</param>
    Task AddAsync(Product product);

    /// <summary>
    /// Updates an existing product in the repository.
    /// </summary>
    /// <param name="product">The product to update.</param>
    Task UpdateAsync(Product product);

    /// <summary>
    /// Deletes a product by its unique identifier.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Determines whether a product exists in the repository.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>True if the product exists; otherwise, false.</returns>
    Task<bool> ProductExistsAsync(int id);
}
