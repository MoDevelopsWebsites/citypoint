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
    }
}
