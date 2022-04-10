using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWebApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [MinLength(3, ErrorMessage = "This field must be 3 to 20 characters long")]
        [MaxLength(20, ErrorMessage = "This field must be 3 to 20 characters long")]
        public string Username { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [MinLength(3, ErrorMessage = "This field must be 3 to 20 characters long")]
        [MaxLength(20, ErrorMessage = "This field must be 3 to 20 characters long")]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}