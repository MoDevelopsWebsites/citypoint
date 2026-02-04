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
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =======================
        // INDEX
        // =======================
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("Staff"))
            {
                return View(await _context.Bookings
                    .Include(b => b.Room)
                    .Include(b => b.User)
                    .OrderByDescending(b => b.Date)
                    .ToListAsync());
            }

            return View(await _context.Bookings
                .Include(b => b.Room)
                .Where(b => b.UserID == userId)
                .OrderByDescending(b => b.Date)
                .ToListAsync());
        }

        // =======================
        // DETAILS
        // =======================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Staff") && booking.UserID != userId)
                return Forbid();

            return View(booking);
        }

        // =======================
        // CREATE (GET)
        // =======================
        public IActionResult Create()
        {
            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomName");
            return View();
        }

        // =======================
        // CREATE (POST)
        // =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomID,Date,Time")] Booking booking)
        {
            booking.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            booking.Status = "Pending";

            // ❌ Cannot book in the past
            var selectedDateTime = booking.Date.Date
                .Add(TimeSpan.Parse(booking.Time));

            if (selectedDateTime < DateTime.Now)
            {
                ModelState.AddModelError("", "You cannot book a room in the past.");
            }

            // ❌ Prevent duplicate bookings
            bool duplicateExists = await _context.Bookings.AnyAsync(b =>
                b.RoomID == booking.RoomID &&
                b.Date == booking.Date &&
                b.Time == booking.Time);

            if (duplicateExists)
            {
                ModelState.AddModelError("", "This room is already booked for the selected date and time.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomName", booking.RoomID);
            return View(booking);
        }

        // =======================
        // EDIT (GET)
        // =======================
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

        // =======================
        // EDIT (POST)
        // =======================
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

            booking.UserID = existingBooking.UserID;
            booking.Status = existingBooking.Status;

            // ❌ Cannot edit into the past
            var selectedDateTime = booking.Date.Date
                .Add(TimeSpan.Parse(booking.Time));

            if (selectedDateTime < DateTime.Now)
            {
                ModelState.AddModelError("", "You cannot book a room in the past.");
            }

            // ❌ Prevent duplicate bookings (excluding self)
            bool duplicateExists = await _context.Bookings.AnyAsync(b =>
                b.BookingID != booking.BookingID &&
                b.RoomID == booking.RoomID &&
                b.Date == booking.Date &&
                b.Time == booking.Time);

            if (duplicateExists)
            {
                ModelState.AddModelError("", "This room is already booked for the selected date and time.");
            }

            if (ModelState.IsValid)
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomName", booking.RoomID);
            return View(booking);
        }

        // =======================
        // DELETE (STAFF ONLY)
        // =======================
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

        // =======================
        // STAFF ACTIONS
        // =======================
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
    }
}
