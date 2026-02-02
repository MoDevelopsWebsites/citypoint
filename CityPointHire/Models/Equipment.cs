using System.ComponentModel.DataAnnotations;

namespace CityPointHire.Models
{
    public class Equipment
    {
        [Key]
        public int EquipmentID { get; set; }

        [Required]
        public string EquipmentName { get; set; }

        public bool Availability { get; set; }
    }
}
