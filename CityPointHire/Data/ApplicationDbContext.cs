using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CityPointHire.Models;

namespace CityPointHire.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Staff> Staff { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ⚠️ REQUIRED for Identity
            base.OnModelCreating(modelBuilder);

            // ✅ Seed Rooms
            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    RoomID = 1,
                    RoomName = "Executive Boardroom",
                    Capacity = 8,
                    Facilities = "4K Display, Video Conferencing, Whiteboard",
                    Price = 45.00m
                },
                new Room
                {
                    RoomID = 2,
                    RoomName = "Training Suite",
                    Capacity = 40,
                    Facilities = "Projector, Sound System, Breakout Area",
                    Price = 65.00m
                },
                new Room
                {
                    RoomID = 3,
                    RoomName = "Grand Event Hall",
                    Capacity = 100,
                    Facilities = "Stage, Ambient Lighting, Catering Ready",
                    Price = 120.00m
                }
            );
        }
    }
}
