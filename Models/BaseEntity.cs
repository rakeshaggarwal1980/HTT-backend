using HTTAPI.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTAPI.Models
{
    /// <summary>
    /// Base entity for audit columns
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Creator, Owner
        /// </summary>
        [Required, Column(TypeName = "nvarchar(100)")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Record creation date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Record modified by
        /// </summary>
        [Column(TypeName = "nvarchar(100)")]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Record modification date
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Record status
        /// </summary>
        public EntityStatus Status { get; set; }
    }
}
