// Imports base system functionality
using System;
// Imports LINQ support for queries
using System.Linq;
// Imports Claims for user identity
using System.Security.Claims;
// Imports support for async operations
using System.Threading.Tasks;
// Imports authorization attributes
using Microsoft.AspNetCore.Authorization;
// Imports MVC controller support
using Microsoft.AspNetCore.Mvc;
// Imports Entity Framework Core features
using Microsoft.EntityFrameworkCore;
// Imports project-specific data context
using CityPointHire.Data;
// Imports project-specific models
using CityPointHire.Models;

// Defines the namespace for controllers
namespace CityPointHire.Controllers
{
    // Requires all actions in this controller to have authenticated users
    [Authorize]
    public class BookingsController : Controller
    {
        // Private field to hold the database context
        private readonly ApplicationDbContext _context;

        // Constructor injection for database context
        public BookingsController(ApplicationDbContext context)
        {
            _context = context; // Assigns the injected context
        }

        // =========================
        // User view - show all rooms and user's bookings
        // =========================
        public async Task<IActionResult> Index()
        {
            // Redirect staff/admin to admin dashboard instead
            if (User.IsInRole("Staff") || User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(AdminIndex));
            }

            // Get current logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get all rooms in the system
            var allRooms = await _context.Rooms.ToListAsync();

            // Get all bookings for the current user
            var myBookings = await _context.Bookings
                .Include(b => b.Room) // Include the room info for each booking
                .Where(b => b.UserID == userId) // Only current user's bookings
                .OrderByDescending(b => b.Date) // Sort newest to oldest
                .ToListAsync();

            // Remove rooms the user has already booked from available list
            var availableRooms = allRooms
                .Where(r => !myBookings.Any(b => b.RoomID == r.RoomID)) // Exclude booked
                .ToList();

            // Pass a tuple with available rooms and user's bookings to the view
            return View(new Tuple<IEnumerable<Room>, IEnumerable<Booking>>(availableRooms, myBookings));
        }

        // =========================
        // Admin/Staff view - show all bookings
        // =========================
        [Authorize(Roles = "Admin,Staff")] // Only accessible to Admin and Staff roles
        public async Task<IActionResult> AdminIndex()
        {
            // Get all bookings including room and user information
            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .OrderByDescending(b => b.Date) // Newest bookings first
                .ToListAsync();

            // Pass the bookings list to the view
            return View(bookings);
        }

        // =========================
        // Update booking status (Admin/Staff only)
        // =========================
        [HttpPost] // Only responds to POST requests
        [ValidateAntiForgeryToken] // Prevent CSRF attacks
        [Authorize(Roles = "Admin,Staff")] // Only Admin/Staff
        public async Task<IActionResult> UpdateStatus(int bookingId, string status)
        {
            // Find booking by ID
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) return NotFound(); // Return 404 if not found

            // Update the status field
            booking.Status = status;
            // Mark entity as modified
            _context.Update(booking);
            // Save changes to database
            await _context.SaveChangesAsync();
            // Redirect back to admin dashboard
            return RedirectToAction(nameof(AdminIndex));
        }

        // =========================
        // GET: Bookings/Create?roomId=1
        // Show booking form for a specific room
        // =========================
        public async Task<IActionResult> Create(int roomId)
        {
            // Fetch the room being booked
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return NotFound(); // 404 if room does not exist

            // Initialize a booking model prefilled with room info
            var booking = new Booking
            {
                RoomID = room.RoomID,
                Room = room
            };

            // Pass booking model to the view
            return View(booking);
        }

        // =========================
        // POST: Cancel a booking
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            // Get current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch booking that belongs to the current user
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingID == bookingId && b.UserID == userId);

            if (booking == null)
            {
                return NotFound(); // Return 404 if not found
            }

            // Remove the booking from the database
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            // Redirect back to user's booking page
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // POST: Create a booking
        // =========================
        [HttpPost] // Responds to POST requests
        [ValidateAntiForgeryToken] // Protect against CSRF
        public async Task<IActionResult> Create([Bind("RoomID,Date,Time")] Booking booking)
        {
            // Assign the booking to the current user
            booking.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Default booking status
            booking.Status = "Pending";

            // Combine date and time into a single DateTime
            var selectedDateTime = booking.Date.Date.Add(TimeSpan.Parse(booking.Time));
            if (selectedDateTime < DateTime.Now)
                ModelState.AddModelError("", "You cannot book a room in the past."); // Validate date

            // Check if the room is already booked at this date and time
            bool duplicateExists = await _context.Bookings.AnyAsync(b =>
                b.RoomID == booking.RoomID &&
                b.Date == booking.Date &&
                b.Time == booking.Time);

            if (duplicateExists)
                ModelState.AddModelError("", "This room is already booked for the selected date and time."); // Validation

            if (ModelState.IsValid)
            {
                // Add booking to the database
                _context.Add(booking);
                await _context.SaveChangesAsync();
                // Redirect back to booking index
                return RedirectToAction(nameof(Index));
            }

            // Re-fetch the room to display in the form if validation fails
            booking.Room = await _context.Rooms.FindAsync(booking.RoomID);

            // Return the form with validation errors
            return View(booking);
        }
    }
}
