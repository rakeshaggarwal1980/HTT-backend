using HTTAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTAPI.Models
{
    /// <summary>
    /// Zone
    /// </summary>
    public class Zone
    {
        /// <summary>
        /// primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Location Name
        /// </summary>
        [Required, Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required, Column(TypeName = "nvarchar(200)")]
        public string Type { get; set; }

        /// <summary>
        /// Location position\order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Record status
        /// </summary>
        public EntityStatus Status { get; set; }

    }
}
