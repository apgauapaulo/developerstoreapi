using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.Application.Services;  // Added to use logging
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;  // Para log de eventos

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    public class SalesController : Controller
    {
        private readonly DefaultContext _context;
        private readonly ISaleService _saleService;
        private readonly IEventLoggingService _eventLoggingService;  // Added for event logging

        public SalesController(DefaultContext context, ISaleService saleService, IEventLoggingService eventLoggingService)
        {
            _context = context;
            _saleService = saleService;
            _eventLoggingService = eventLoggingService;  // Dependency injection for event logging
        }

        /// <summary>
        /// Creates a new sale.
        /// </summary>
        /// <remarks>
        /// This endpoint allows you to create a new sale. 
        /// It applies business rule validation and calculates any applicable discount.
        /// </remarks>
        /// <param name="sale">Sale object to be created</param>
        /// <returns>Returns the created sale object with a 201 status code.</returns>
        /// <response code="201">Sale created successfully</response>
        /// <response code="400">Invalid sale data based on business rules</response>
        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] Sale sale)
        {
            // Applies validations before saving
            if (!await _saleService.ValidateSaleAsync(sale))
            {
                return BadRequest("The sale is invalid based on the business rules.");
            }

            // Calculates the discount (if any) and applies it to the sale
            var discount = await _saleService.CalculateDiscountAsync(sale);
            sale.TotalAmount = sale.Items.Sum(i => i.TotalAmount) * (1 - discount); // Applies the discount to the total

            // Saves the sale
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            // Log the event of sale creation
            _eventLoggingService.LogVendaCriada(sale.Id);  // Log event for sale creation

            return CreatedAtAction("GetSaleById", new { id = sale.Id }, sale);
        }

        /// <summary>
        /// Retrieves a sale by its ID.
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves the details of a sale based on its unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the sale.</param>
        /// <returns>Returns the sale object if found, otherwise returns 404 status code.</returns>
        /// <response code="200">Sale found</response>
        /// <response code="404">Sale not found</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleById(Guid id)
        {
            var sale = await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id); // Searches for the sale using the ID

            if (sale == null)
            {
                return NotFound();
            }

            return Ok(sale);
        }

        /// <summary>
        /// Updates an existing sale.
        /// </summary>
        /// <remarks>
        /// This endpoint allows updating an existing sale. The sale ID in the URL must match the ID in the request body.
        /// </remarks>
        /// <param name="id">The unique identifier of the sale.</param>
        /// <param name="updatedSale">Sale object containing the updated information.</param>
        /// <returns>Returns a 204 status code for a successful update.</returns>
        /// <response code="204">Sale updated successfully</response>
        /// <response code="400">Sale ID mismatch or invalid data</response>
        /// <response code="404">Sale not found</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(Guid id, [FromBody] Sale updatedSale)
        {
            if (id != updatedSale.Id)
            {
                return BadRequest("The sale ID in the URL does not match the sale ID in the body.");
            }

            var existingSale = await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (existingSale == null)
            {
                return NotFound();
            }

            // Applies validations before updating
            if (!await _saleService.ValidateSaleAsync(updatedSale))
            {
                return BadRequest("The sale is invalid based on the business rules.");
            }

            // Calculates the discount (if any) and applies it to the sale
            var discount = await _saleService.CalculateDiscountAsync(updatedSale);
            updatedSale.TotalAmount = updatedSale.Items.Sum(i => i.TotalAmount) * (1 - discount); // Applies the discount to the total amount

            existingSale.Items = updatedSale.Items;
            existingSale.Customer = updatedSale.Customer;
            existingSale.Branch = updatedSale.Branch;
            existingSale.TotalAmount = updatedSale.TotalAmount; // Updates the total amount

            await _context.SaveChangesAsync();

            // Log the event of sale modification
            _eventLoggingService.LogVendaModificada(id);  // Log event for sale modification

            return NoContent();
        }

        /// <summary>
        /// Cancels a sale.
        /// </summary>
        /// <remarks>
        /// This endpoint allows you to cancel an existing sale. 
        /// Once cancelled, the sale cannot be restored.
        /// </remarks>
        /// <param name="id">The unique identifier of the sale to cancel.</param>
        /// <returns>Returns a 204 status code if the sale is successfully cancelled.</returns>
        /// <response code="204">Sale cancelled successfully</response>
        /// <response code="404">Sale not found</response>
        /// <response code="400">Sale is already cancelled</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelSale(Guid id)
        {
            var sale = await _context.Sales
                .FirstOrDefaultAsync(s => s.Id == id); // Retrieves the sale by ID

            if (sale == null)
            {
                return NotFound();
            }

            if (sale.IsCancelled)
            {
                return BadRequest("The sale is already cancelled.");
            }

            sale.IsCancelled = true;  // Marks the sale as canceled
            await _context.SaveChangesAsync();

            // Log the event of sale cancellation
            _eventLoggingService.LogVendaCancelada(id);  // Log event for sale cancellation

            return NoContent();
        }
    }
}
