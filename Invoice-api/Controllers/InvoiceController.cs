using Microsoft.AspNetCore.Mvc;
using Invoice_api.Manager;
using Invoice_api.Infraestructure.Dtos;

namespace Invoice_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceManager _invoiceManager;

        public InvoiceController(InvoiceManager invoiceManager)
        {
            _invoiceManager = invoiceManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceToSaveDto invoiceDto)
        {
            if (invoiceDto == null)
            {
                return BadRequest("La factura es requerida.");
            }

            var createdInvoice = await _invoiceManager.CreateInvoiceAsync(invoiceDto);
            return CreatedAtAction(nameof(GetInvoiceById), new { id = createdInvoice.InvoiceId }, createdInvoice);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(long id)
        {
            var invoice = await _invoiceManager.GetInvoiceByIdAsync(id);
            return invoice != null ? Ok(invoice) : NotFound($"Factura con ID {id} no encontrada.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(long id, [FromBody] InvoiceToSaveDto invoiceDto)
        {
            if (invoiceDto == null)
            {
                return BadRequest("La factura es requerida.");
            }

            var updatedInvoice = await _invoiceManager.UpdateInvoiceAsync(invoiceDto, id);
            return Ok(updatedInvoice);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(long id)
        {
            var deleted = await _invoiceManager.DeleteInvoiceAsync(id);
            return deleted ? Ok($"Factura con ID {id} eliminada.") : NotFound($"Factura con ID {id} no encontrada.");
        }
    }
}
