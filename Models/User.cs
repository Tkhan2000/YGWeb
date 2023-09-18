using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace YGWeb.Models
{
    public class User
    {
        [Required]
        [Key]
        [MaxLength(20)]
        public string username { get; set; }
        [Required]
        [MaxLength(20)]
        public string password { get; set; }
    }
}
