using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityPointHire.Models
{
    public class Staff
    {
        [Key]
        public int StaffID { get; set; }

        [Required]
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public IdentityUser User { get; set; }

        [Required]
        public string StaffRole { get; set; }
    }
}
