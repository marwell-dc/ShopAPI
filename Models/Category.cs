using System.ComponentModel.DataAnnotations;

namespace ShopWebApi.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [MinLength(3, ErrorMessage = "This field must be 3 to 60 characters long")]
        [MaxLength(60, ErrorMessage = "This field must be 3 to 60 characters long")]
        public string Title { get; set; }
    }
}