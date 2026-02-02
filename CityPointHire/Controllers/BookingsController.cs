using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CityPointHire.Data;
using CityPointHire.Models;

namespace CityPointHire.Controllers
{
    [Authorize] // All actions require login
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Staff"))
            {
                // Staff can see all bookings
                var allBookings = _context.Bookings.Include(b => b.Room).Include(b => b.User);
                return View(await allBookings.ToListAsync());
            }
            else
            {
                // Normal users see only their own bookings
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var bookings = _context.Bookings
                    .Include(b => b.Room)
                    .Where(b => b.UserID == userId);
                return View(await bookings.ToListAsync());
            }
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null) return NotFound();

            // Only owner or staff can view
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Staff") && booking.UserID != userId)
                return Forbid();

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomName");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomID,Date,Time")] Booking booking)
        {
            // Assign server-side fields
            booking.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            booking.Status = "Pending";

            if (booking.RoomID == 0)
                ModelState.AddModelError("RoomID", "You must select a room.");

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomName", booking.RoomID);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Staff") && booking.UserID != userId)
                return Forbid();

            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomName", booking.RoomID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingID,RoomID,Date,Time")] Booking booking)
        {
            if (id != booking.BookingID) return NotFound();

            var existingBooking = await _context.Bookings.AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookingID == id);

            if (existingBooking == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Staff") && existingBooking.UserID != userId)
                return Forbid();

            // Preserve server-assigned fields
            booking.UserID = existingBooking.UserID;
            booking.Status = existingBooking.Status;

            if (ModelState.IsValid)
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomName", booking.RoomID);
            return View(booking);
        }

        // DELETE: USERS CANNOT DELETE BOOKINGS
        // Only Staff can delete
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // STAFF ONLY: Approve Booking
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Approve(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            booking.Status = "Approved";
            _context.Update(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // STAFF ONLY: Deny Booking
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Deny(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            booking.Status = "Denied";
            _context.Update(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingID == id);
        }
    }
}
