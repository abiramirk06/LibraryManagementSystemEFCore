
namespace LibraryManagementSystemEFCore.Models
{
    public class Author
    {
        public int AuthorId { get; set; }

        public string AuthorName { get; set; } = string.Empty;

        public string? Biography { get; set; }

        public string? Country { get; set; }
    }
}
