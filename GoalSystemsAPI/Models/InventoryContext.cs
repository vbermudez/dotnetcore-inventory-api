using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalSystemsAPI.Models
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("Data Source=Data/inventory.db");
        public DbSet<InventoryItem> InventoryItems { get; set; }
    }
}
