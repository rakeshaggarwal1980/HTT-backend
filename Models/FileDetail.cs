using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTAPI.Models
{
    /// <summary>
    /// File Management
    /// </summary>
    public class FileDetail
    {
        /// <summary>
        /// primary key
        /// File saved in local directory with same key and extension
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// File original name
        /// </summary>
        [Required, Column(TypeName = "nvarchar(200)")]
        public string FileName { get; set; }
    }
}
