
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystemEFCore.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }

        [Required]
        [MaxLength(100)]
        public string EntityName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Action { get; set; } = string.Empty;

        public int? RecordId { get; set; }

        public string? Details { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
