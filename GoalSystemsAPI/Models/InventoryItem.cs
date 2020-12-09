using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalSystemsAPI.Models
{
    public class InventoryItem
    {
        public long Id { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Units { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Location { get; set; }
    }
}
