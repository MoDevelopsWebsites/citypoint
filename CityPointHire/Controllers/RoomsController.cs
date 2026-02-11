// Imports base system functionality
using System;
// Imports LINQ for queries
using System.Linq;
// Imports support for asynchronous programming
using System.Threading.Tasks;
// Imports MVC controller support
using Microsoft.AspNetCore.Mvc;
// Imports Entity Framework Core features
using Microsoft.EntityFrameworkCore;
// Imports project-specific data context
using CityPointHire.Data;
// Imports project-specific models
using CityPointHire.Models;
// Imports authorization attributes
using Microsoft.AspNetCore.Authorization;

// Namespace for project controllers
namespace CityPointHire.Controllers
{
    // Requires all actions in this controller to have authenticated users
    [Authorize]
    public class RoomsController : Controller
    {
        // Private field to hold the database context
        private readonly ApplicationDbContext _context;

        // Constructor injection for the database context
        public RoomsController(ApplicationDbContext context)
        {
            _context = context; // Assign injected context
        }

        // =========================
        // VIEW ROOMS (ALL USERS)
        // =========================
        public async Task<IActionResult> Index()
        {
            // Fetch all rooms from the database and pass to view
            return View(await _context.Rooms.ToListAsync());
        }

        // =========================
        // VIEW ROOM DETAILS (ALL USERS)
        // =========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // Check if id parameter is missing
                return NotFound(); // Return 404 if no ID provided

            // Fetch a single room by ID
            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomID == id);

            if (room == null) // Check if room exists
                return NotFound(); // Return 404 if not found

            // Pass the room to the view
            return View(room);
        }

        // =========================
        // CREATE ROOM (ADMIN ONLY)
        // =========================
        [Authorize(Roles = "Admin")] // Only users in Admin role can create
        public IActionResult Create()
        {
            return View(); // Return the create room form
        }

        [HttpPost] // Responds to POST requests
        [ValidateAntiForgeryToken] // Protects against CSRF
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> Create([Bind("RoomID,RoomName,Capacity,Facilities,Price")] Room room)
        {
            if (ModelState.IsValid) // Validate the submitted model
            {
                _context.Add(room); // Add new room to context
                await _context.SaveChangesAsync(); // Save changes to database
                return RedirectToAction(nameof(Index)); // Redirect to room list
            }
            return View(room); // Return form with validation errors
        }

        // =========================
        // EDIT ROOM (ADMIN ONLY)
        // =========================
        [Authorize(Roles = "Admin")] // Only Admins can edit rooms
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // Check if ID parameter is missing
                return NotFound(); // Return 404 if no ID provided

            // Find the room to edit by ID
            var room = await _context.Rooms.FindAsync(id);

            if (room == null) // Check if room exists
                return NotFound(); // Return 404 if not found

            // Pass the room to the edit view
            return View(room);
        }

        [HttpPost] // Responds to POST requests
        [ValidateAntiForgeryToken] // Protects against CSRF
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> Edit(int id, [Bind("RoomID,RoomName,Capacity,Facilities,Price")] Room room)
        {
            if (id != room.RoomID) // Verify the route ID matches the room ID
                return NotFound(); // Return 404 if mismatch

            if (ModelState.IsValid) // Validate the submitted model
            {
                try
                {
                    _context.Update(room); // Mark room as updated
                    await _context.SaveChangesAsync(); // Save changes to database
                }
                catch (DbUpdateConcurrencyException) // Handle concurrency conflicts
                {
                    if (!RoomExists(room.RoomID)) // Check if room still exists
                        return NotFound(); // Return 404 if room deleted
                    else
                        throw; // Rethrow other exceptions
                }
                return RedirectToAction(nameof(Index)); // Redirect to room list
            }
            return View(room); // Return form with validation errors
        }

        // =========================
        // DELETE ROOM (ADMIN ONLY)
        // =========================
        [Authorize(Roles = "Admin")] // Only Admins can delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // Check if ID parameter is missing
                return NotFound(); // Return 404 if no ID provided

            // Fetch the room to delete
            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomID == id);

            if (room == null) // Check if room exists
                return NotFound(); // Return 404 if not found

            // Pass room to delete confirmation view
            return View(room);
        }

        [HttpPost, ActionName("Delete")] // Respond to POST requests named "Delete"
        [ValidateAntiForgeryToken] // CSRF protection
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id); // Find room by ID

            if (room != null) // If room exists
                _context.Rooms.Remove(room); // Remove room from context

            await _context.SaveChangesAsync(); // Save changes to database
            return RedirectToAction(nameof(Index)); // Redirect to room list
        }

        // =========================
        // HELPER METHOD: Check if room exists
        // =========================
        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.RoomID == id); // Returns true if room exists
        }
    }
}
