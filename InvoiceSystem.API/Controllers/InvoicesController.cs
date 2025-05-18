using InvoiceSystem.Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
 
using InvoiceSystem.Application.Features.Invoices.Commands;
using InvoiceSystem.Application.Features.Invoices.Handler;
 
 

namespace InvoiceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        ///  GetInvoices
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoices()
        {
            var query = new GetAllInvoicesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        // GET: api/invoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDetailsDto>> GetInvoice(int id)
        {
            var query = new GetInvoiceQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        
        // POST: api/invoices
        [HttpPost]
        public async Task<ActionResult<int>> CreateInvoice([FromBody] CreateInvoiceCommand command)
        {
            var invoiceId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetInvoice), new { id = invoiceId }, invoiceId);
        }

        // PUT: api/invoices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] UpdateInvoiceCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            await _mediator.Send(new DeleteInvoiceCommand { Id = id });
            return NoContent();
        }

        // POST: api/invoices/5/pay
        [HttpPost("{id}/pay")]
        public async Task<IActionResult> MarkAsPaid(int id, [FromBody] MarkInvoiceAsPaidCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }


    }
}


 