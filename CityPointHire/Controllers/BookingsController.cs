using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        // User view - show all rooms to book
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Staff") || User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(AdminIndex));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get all rooms
            var allRooms = await _context.Rooms.ToListAsync();

            // Get rooms the user already booked
            var myBookings = await _context.Bookings
                .Include(b => b.Room)
                .Where(b => b.UserID == userId)
                .OrderByDescending(b => b.Date)
                .ToListAsync();

            // Optionally, remove booked rooms from available list
            var availableRooms = allRooms
                .Where(r => !myBookings.Any(b => b.RoomID == r.RoomID))
                .ToList();

            return View(new Tuple<IEnumerable<Room>, IEnumerable<Booking>>(availableRooms, myBookings));
        }


        // Admin/Staff view - show all bookings
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AdminIndex()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .OrderByDescending(b => b.Date)
                .ToListAsync();

            return View(bookings);
        }

        // Update status
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateStatus(int bookingId, string status)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) return NotFound();

            booking.Status = status;
            _context.Update(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminIndex));
        }

        // =======================
        // Create Booking (user only)
        // =======================
        // GET: Bookings/Create?roomId=1
        public async Task<IActionResult> Create(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return NotFound();

            var booking = new Booking
            {
                RoomID = room.RoomID,
                Room = room
            };

            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingID == bookingId && b.UserID == userId);

            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomID,Date,Time")] Booking booking)
        {
            booking.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            booking.Status = "Pending";

            var selectedDateTime = booking.Date.Date.Add(TimeSpan.Parse(booking.Time));
            if (selectedDateTime < DateTime.Now)
                ModelState.AddModelError("", "You cannot book a room in the past.");

            bool duplicateExists = await _context.Bookings.AnyAsync(b =>
                b.RoomID == booking.RoomID &&
                b.Date == booking.Date &&
                b.Time == booking.Time);

            if (duplicateExists)
                ModelState.AddModelError("", "This room is already booked for the selected date and time.");

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }



            // Re-fetch room to redisplay form
            booking.Room = await _context.Rooms.FindAsync(booking.RoomID);

            return View(booking);
        }
    }
}
