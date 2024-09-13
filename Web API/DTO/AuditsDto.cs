using static Hexa_Hub.Models.MultiValues;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Hexa_Hub.DTO
{
    public class AuditsDto
    {
        [Required]
        [Key]
        public int AuditId { get; set; }

        [Required]
        public int AssetId { get; set; }

        [Required]
        public int UserId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? AuditDate { get; set; }

        public string? AuditMessage { get; set; }

 
        [DefaultValue(AuditStatus.Sent)]
        public AuditStatus? Audit_Status { get; set; } = AuditStatus.Sent;
    }
}
