using System.ComponentModel.DataAnnotations;

namespace ShopWebApi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [MinLength(3, ErrorMessage = "This field must be 3 to 60 characters long")]
        [MaxLength(60, ErrorMessage = "This field must be 3 to 60 characters long")]
        public string Title { get; set; }

        [MaxLength(60, ErrorMessage = "This field must have a maximum of 1024 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Category invalid.")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}