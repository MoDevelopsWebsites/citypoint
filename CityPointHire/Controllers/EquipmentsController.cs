// Imports LINQ for querying collections
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
    public class EquipmentsController : Controller
    {
        // Private field to hold the database context
        private readonly ApplicationDbContext _context;

        // Constructor injection for the database context
        public EquipmentsController(ApplicationDbContext context)
        {
            _context = context; // Assign injected context
        }

        // ===============================
        // VIEW ALL EQUIPMENT (ALL USERS)
        // ===============================
        public async Task<IActionResult> Index()
        {
            // Fetch all equipment records from the database
            var equipment = await _context.Equipment.ToListAsync();
            // Pass the list to the Index view
            return View(equipment);
        }

        // ===============================
        // VIEW EQUIPMENT DETAILS (ALL USERS)
        // ===============================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // Check if ID parameter is missing
                return NotFound(); // Return 404 if no ID provided

            // Fetch single equipment by ID
            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(m => m.EquipmentID == id);

            if (equipment == null) // Check if equipment exists
                return NotFound(); // Return 404 if not found

            // Pass equipment to details view
            return View(equipment);
        }

        // ===============================
        // CREATE EQUIPMENT (ADMIN ONLY)
        // ===============================
        [Authorize(Roles = "Admin")] // Only Admin role can create equipment
        public IActionResult Create()
        {
            return View(); // Return the create form view
        }

        [HttpPost] // Respond to POST requests
        [ValidateAntiForgeryToken] // Protects against CSRF
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> Create(Equipment equipment)
        {
            if (ModelState.IsValid) // Validate submitted model
            {
                _context.Add(equipment); // Add equipment to database context
                await _context.SaveChangesAsync(); // Save changes to database
                return RedirectToAction(nameof(Index)); // Redirect to equipment list
            }
            return View(equipment); // Return form with validation errors
        }

        // ===============================
        // EDIT EQUIPMENT (ADMIN ONLY)
        // ===============================
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // Check if ID parameter is missing
                return NotFound(); // Return 404 if no ID provided

            // Find equipment by ID
            var equipment = await _context.Equipment.FindAsync(id);

            if (equipment == null) // Check if equipment exists
                return NotFound(); // Return 404 if not found

            // Pass equipment to edit view
            return View(equipment);
        }

        [HttpPost] // Respond to POST requests
        [ValidateAntiForgeryToken] // Protects against CSRF
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> Edit(int id, Equipment equipment)
        {
            if (id != equipment.EquipmentID) // Ensure route ID matches equipment ID
                return NotFound(); // Return 404 if mismatch

            if (ModelState.IsValid) // Validate submitted model
            {
                _context.Update(equipment); // Mark equipment as updated
                await _context.SaveChangesAsync(); // Save changes to database
                return RedirectToAction(nameof(Index)); // Redirect to list
            }

            return View(equipment); // Return form with validation errors
        }

        // ===============================
        // DELETE EQUIPMENT (ADMIN ONLY)
        // ===============================
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // Check if ID parameter is missing
                return NotFound(); // Return 404 if no ID provided

            // Fetch equipment by ID
            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(m => m.EquipmentID == id);

            if (equipment == null) // Check if equipment exists
                return NotFound(); // Return 404 if not found

            // Pass equipment to delete confirmation view
            return View(equipment);
        }

        [HttpPost, ActionName("Delete")] // Respond to POST named "Delete"
        [ValidateAntiForgeryToken] // CSRF protection
        [Authorize(Roles = "Admin")] // Admin only
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id); // Find equipment by ID

            if (equipment != null) // Check if equipment exists
                _context.Equipment.Remove(equipment); // Remove from context

            await _context.SaveChangesAsync(); // Save changes to database
            return RedirectToAction(nameof(Index)); // Redirect to list
        }
    }
}
