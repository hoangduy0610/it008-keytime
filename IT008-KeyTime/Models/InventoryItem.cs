using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_KeyTime.Models
{
    [Table("rls_inventory_items")]
    public class InventoryItem
    {
        [Key]
        public int id { get; set; }
        public int inventory_plan_id { get; set; }
        public int item_id { get; set; }
        public string note { get; set; }
        public int status { get; set; }
    }

    public class InventoryItemWithItem
    {
        public int inventory_plan_id { get; set; }
        public int item_id { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public int status { get; set; }
    }
}
