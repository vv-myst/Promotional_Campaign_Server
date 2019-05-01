using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DunnhumbyHomeWork.DbModel
{
    [Table("Product", Schema = "prmanagement")]
    public class Product
    {
        [Key]
        [Column("Id", TypeName = "varchar(36)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public string Id { get; set; }

        [Column("Name", TypeName = "varchar(256)")]
        [StringLength(256)]
        [Required]
        public string Name { get; set; }

        [Column("Category", TypeName = "varchar(32)")]
        [StringLength(32)]
        [Required]
        public string Category { get; set; }
        
        [Column("LastUpdate", TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required]
        public DateTime LastUpdate { get; set; }

        [Column("LastUser", TypeName = "varchar(32)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(32)]
        [Required]
        public string LastUser { get; set; }

        [Column("CreationTime", TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public DateTime CreationTime { get; set; }

        [Column("Creator", TypeName = "varchar(32)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(32)]
        [Required]
        public string Creator { get; set; }
    }
}
