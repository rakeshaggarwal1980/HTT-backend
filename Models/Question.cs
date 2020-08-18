using HTTAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTAPI.Models
{
    /// <summary>
    /// Question
    /// </summary>
    public class Question
    {
        /// <summary>
        /// primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Question 
        /// </summary>
        [Required, Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }

        [Required, Column(TypeName = "nvarchar(200)")]
        public string Type { get; set; }

        /// <summary>
        /// position\order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Record status
        /// </summary>
        public EntityStatus Status { get; set; }

    }
}
