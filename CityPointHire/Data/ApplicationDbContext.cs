// Imports Identity EntityFramework support
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// Imports Entity Framework Core
using Microsoft.EntityFrameworkCore;

// Imports project models
using CityPointHire.Models;

// Defines the namespace for data-related classes
namespace CityPointHire.Data
{
    // ApplicationDbContext inherits from IdentityDbContext to include Identity tables
    public class ApplicationDbContext : IdentityDbContext
    {
        // Constructor that passes options to the base IdentityDbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Creates a database table for Rooms
        public DbSet<Room> Rooms { get; set; }

        // Creates a database table for Equipment
        public DbSet<Equipment> Equipment { get; set; }

        // Creates a database table for Bookings
        public DbSet<Booking> Bookings { get; set; }

        // Creates a database table for Staff
        public DbSet<Staff> Staff { get; set; }

        // Overrides the model creation method to configure database schema and seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Calls the base Identity configuration (VERY IMPORTANT)
            base.OnModelCreating(modelBuilder);

            // Seeds initial Room data into the database
            modelBuilder.Entity<Room>().HasData(

                // Adds Room with ID 1
                new Room { RoomID = 1, RoomName = "Executive Boardroom", Capacity = 8, Facilities = "4K Display, Video Conferencing, Whiteboard", Price = 45.00m },

                // Adds Room with ID 2
                new Room { RoomID = 2, RoomName = "Training Suite", Capacity = 40, Facilities = "Projector, Sound System, Breakout Area", Price = 65.00m },

                // Adds Room with ID 3
                new Room { RoomID = 3, RoomName = "Grand Event Hall", Capacity = 100, Facilities = "Stage, Ambient Lighting, Catering Ready", Price = 120.00m },

                // Adds Room with ID 4
                new Room { RoomID = 4, RoomName = "Innovation Lab", Capacity = 15, Facilities = "Interactive Screens, Flexible Seating", Price = 55.00m },

                // Adds Room with ID 5
                new Room { RoomID = 5, RoomName = "Creative Studio", Capacity = 20, Facilities = "Smart Board, Natural Lighting", Price = 50.00m },

                // Adds Room with ID 6
                new Room { RoomID = 6, RoomName = "Conference Room A", Capacity = 12, Facilities = "HD Display, Conference Phone", Price = 40.00m },

                // Adds Room with ID 7
                new Room { RoomID = 7, RoomName = "Conference Room B", Capacity = 10, Facilities = "Whiteboard, HDMI Setup", Price = 35.00m },

                // Adds Room with ID 8
                new Room { RoomID = 8, RoomName = "Workshop Room 1", Capacity = 25, Facilities = "Projector, Flip Charts", Price = 60.00m },

                // Adds Room with ID 9
                new Room { RoomID = 9, RoomName = "Workshop Room 2", Capacity = 30, Facilities = "Dual Screens, Sound System", Price = 70.00m },

                // Adds Room with ID 10
                new Room { RoomID = 10, RoomName = "Seminar Hall", Capacity = 60, Facilities = "Tiered Seating, AV System", Price = 90.00m },

                // Adds Room with ID 11
                new Room { RoomID = 11, RoomName = "Interview Room 1", Capacity = 4, Facilities = "Private Space, Desk Setup", Price = 20.00m },

                // Adds Room with ID 12
                new Room { RoomID = 12, RoomName = "Interview Room 2", Capacity = 4, Facilities = "Quiet Zone, Office Desk", Price = 20.00m },

                // Adds Room with ID 13
                new Room { RoomID = 13, RoomName = "Team Collaboration Hub", Capacity = 18, Facilities = "Modular Tables, Smart TV", Price = 55.00m },

                // Adds Room with ID 14
                new Room { RoomID = 14, RoomName = "Strategy Room", Capacity = 14, Facilities = "Wall Whiteboards, Screen Casting", Price = 48.00m },

                // Adds Room with ID 15
                new Room { RoomID = 15, RoomName = "Networking Lounge", Capacity = 35, Facilities = "Comfort Seating, Refreshment Area", Price = 75.00m },

                // Adds Room with ID 16
                new Room { RoomID = 16, RoomName = "Auditorium", Capacity = 150, Facilities = "Stage, Full AV, Lighting Rig", Price = 150.00m },

                // Adds Room with ID 17
                new Room { RoomID = 17, RoomName = "Digital Media Room", Capacity = 12, Facilities = "Recording Equipment, Editing PC", Price = 65.00m },

                // Adds Room with ID 18
                new Room { RoomID = 18, RoomName = "Executive Meeting Room", Capacity = 6, Facilities = "Premium Seating, Smart Display", Price = 50.00m },

                // Adds Room with ID 19
                new Room { RoomID = 19, RoomName = "Community Space", Capacity = 45, Facilities = "Open Plan, Projector", Price = 80.00m },

                // Adds Room with ID 20
                new Room { RoomID = 20, RoomName = "Private Consultation Room", Capacity = 3, Facilities = "Confidential Setup, Desk & Chairs", Price = 25.00m }

            );
        }
    }
}
