
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvoiceSystem.API.Controllers;
using InvoiceSystem.Application.DTO;
using InvoiceSystem.Application.Features.Invoices.Commands;
using InvoiceSystem.Application.Features.Invoices.Handler;

namespace InvoiceSystem.UnitTests.Controllers;

[TestClass]
public class InvoicesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly InvoicesController _controller;

    public InvoicesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new InvoicesController(_mediatorMock.Object);
    }
    [TestMethod]
    public async Task GetInvoices_ReturnsOk_WithInvoices()
    {
        // Arrange
        var invoices = new List<InvoiceDto> { new InvoiceDto { Id = 1 }, new InvoiceDto { Id = 2 } };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllInvoicesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(invoices);

        // Act
        var result = await _controller.GetInvoices();

        // Assert

        var okResul = result.Result as OkObjectResult;
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(okResul);

    }

    [TestMethod]
    public async Task GetInvoice_ReturnsOk_WhenFound()
    {
        // Arrange
        var invoice = new InvoiceDetailsDto { Id = 1 };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetInvoiceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(invoice);

        // Act
        var result = await _controller.GetInvoice(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(okResult);
    }

    [TestMethod]
    public async Task GetInvoice_ReturnsNotFound_WhenNull()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetInvoiceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((InvoiceDetailsDto)null);

        // Act
        var result = await _controller.GetInvoice(1);

        // Assert

        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result.Result as NotFoundResult);
    }

    [TestMethod]
    public async Task CreateInvoice_ReturnsCreatedAtAction()
    {
        // Arrange
        //var command = new CreateInvoiceCommand();
        var command = new CreateInvoiceCommand
        {
            // Assuming CustomerId is required, set a sample value
            CustomerId = 1,
            PaymentTerms = "Net 30",
            Notes = "Test invoice",
            Items = new List<InvoiceItemDto>
            {
                new InvoiceItemDto
                {
                    ProductId = 100,
                    ProductName = "Sample Product",
                    Quantity = 1,
                    UnitPrice = 245.00m,
                    TaxRate = 0,
                    Description = "Test item",
                    LineTotal = 245.00m
                }
            }
         };
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.CreateInvoice(command);

        // Assert
        var createdResult = result.Result as CreatedAtActionResult;
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(createdResult);
    }

    [TestMethod]
    public async Task UpdateInvoice_ReturnsNoContent_WhenIdMatches()
    {
        // Arrange
        var command = new UpdateInvoiceCommand { Id = 5 };

        // Act
        var result = await _controller.UpdateInvoice(5, command);

        // Assert

        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);

    }

    [TestMethod]
    public async Task UpdateInvoice_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var command = new UpdateInvoiceCommand { Id = 2 };

        // Act
        var result = await _controller.UpdateInvoice(1, command);

        // Assert
        var assertresult = result as BadRequestResult;
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(assertresult);
    }

    [TestMethod]
    public async Task DeleteInvoice_ReturnsNoContent()
    {
        // Act
        var result = await _controller.DeleteInvoice(1);

        // Assert
        var assertresult = result as NoContentResult;
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(assertresult);
    }

    [TestMethod]
    public async Task MarkAsPaid_ReturnsNoContent_WhenIdMatches()
    {
        // Arrange
        var command = new MarkInvoiceAsPaidCommand { Id = 1 };

        // Act
        var result = await _controller.MarkAsPaid(1, command);

        // Assert
        var assertresult = result as NoContentResult;
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(assertresult);
    }

    [TestMethod]
    public async Task MarkAsPaid_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var command = new MarkInvoiceAsPaidCommand { Id = 2 };

        // Act
        var result = await _controller.MarkAsPaid(1, command);

        // Assert
        var assertresult = result as BadRequestResult;
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(assertresult);
    }

}
