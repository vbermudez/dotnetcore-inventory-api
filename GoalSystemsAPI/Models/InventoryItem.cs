using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoalSystemsAPI.Models
{
    public class InventoryItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(4290)]
        public string Barcode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
        
        [Required]
        public long Units { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Location { get; set; }
        public bool Expired { get; set; }
    }
}
