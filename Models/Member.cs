
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystemEFCore.Models
{
    public class Member
    {
        public int MemberId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        public DateTime MembershipDate { get; set; } = DateTime.Today;

        public bool IsActive { get; set; } = true;
    }
}
