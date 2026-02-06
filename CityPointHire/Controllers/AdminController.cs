using CityPointHire.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityPointHire.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DASHBOARD
        public IActionResult Index()
        {
            return View();
        }

        // BOOKINGS MANAGEMENT
        public async Task<IActionResult> Bookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .OrderByDescending(b => b.Date)
                .ToListAsync();

            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int bookingId, string status)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) return NotFound();

            booking.Status = status;
            _context.Update(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Bookings));
        }
    }
}
