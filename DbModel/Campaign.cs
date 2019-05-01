using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DunnhumbyHomeWork.DbModel
{
    [Table("Campaign", Schema = "prmanagement")]
    public class Campaign
    {
        [Key]
        [Column("Id", TypeName = "varchar(36)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public string Id { get; set; }
        
        [Column("ProductId", TypeName = "varchar(256)")]
        [StringLength(256)]
        [Required]
        public string Name { get; set; }
        
        [Column("ProductId", TypeName = "varchar(36)")]
        [StringLength(36)]
        [Required]
        public string ProductId { get; set; }
        
        [Column("StartDate", TypeName = "datetime")]
        [Required]
        public DateTime StartDate { get; set; }
        
        [Column("EndDate", TypeName = "datetime")]
        [Required]
        public DateTime EndDate { get; set; }
        
        [Column("IsActive", TypeName = "bool")]
        [Required]
        public bool IsActive { get; set; }
        
        [Column("LastUpdate", TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required]
        public DateTime LastUpdate { get; set; }

        [Column("LastUser", TypeName = "varchar(32)")]
        [StringLength(32)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required]
        public string LastUser { get; set; }

        [Column("CreationTime", TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public DateTime CreationTime { get; set; }

        [Column("Creator", TypeName = "varchar(32)")]
        [StringLength(32)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public string Creator { get; set; }
        
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
