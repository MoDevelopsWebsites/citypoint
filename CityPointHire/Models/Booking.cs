using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityPointHire.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        [BindNever] // Ignore model binding
        public string? UserID { get; set; }

        [ForeignKey("UserID")]
        [BindNever]
        public IdentityUser? User { get; set; }

        [Required(ErrorMessage = "You must select a room.")]
        public int RoomID { get; set; }

        [ForeignKey("RoomID")]
        [BindNever]
        public Room? Room { get; set; }

        [Required(ErrorMessage = "Please select a date.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please select a time.")]
        public string Time { get; set; }

        [BindNever]
        public string? Status { get; set; }
    }
}
