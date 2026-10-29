
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystemEFCore.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? ISBN { get; set; }

        public int PublicationYear { get; set; }

        public decimal Price { get; set; }

        public int AvailableCopies { get; set; }

        // Foreign keys
        public int CategoryId { get; set; }

        public int AuthorId { get; set; }

        // Navigation properties
        public Category? Category { get; set; }

        public Author? Author { get; set; }
    }
}
