using CondoAdmin.Domain.Entities;
using CondoAdmin.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CondoAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _contexto;

        public PaymentController(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<ICollection<Payment>>> GetPayments()
        {
            var payments = await _contexto.Payments
                .Include(p => p.Resident)
                .ToListAsync();

            return Ok(payments);
        }

        // GET by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _contexto.Payments
                .Include(p => p.Resident)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        // POST
        [HttpPost]
        public async Task<ActionResult<Payment>> CreatePayment([FromBody] Payment payment)
        {
            _contexto.Payments.Add(payment);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] Payment payment)
        {
            if (id != payment.Id)
                return BadRequest("El ID no coincide con el pago enviado.");

            var existing = await _contexto.Payments.FindAsync(id);
            if (existing == null)
                return NotFound();

            // PROPIEDADES CORRECTAS
            existing.Amount = payment.Amount;
            existing.DueDate = payment.DueDate;
            existing.PaidAt = payment.PaidAt;
            existing.Notes = payment.Notes;
            existing.Month = payment.Month;
            existing.ResidentId = payment.ResidentId;

            await _contexto.SaveChangesAsync();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _contexto.Payments.FindAsync(id);
            if (payment == null)
                return NotFound();

            _contexto.Payments.Remove(payment);
            await _contexto.SaveChangesAsync();
            return NoContent();
        }
    }
}