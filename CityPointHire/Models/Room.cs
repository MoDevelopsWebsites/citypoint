using System.ComponentModel.DataAnnotations;

namespace CityPointHire.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }

        [Required]
        public string RoomName { get; set; }

        [Required]
        public int Capacity { get; set; }

        public string Facilities { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
