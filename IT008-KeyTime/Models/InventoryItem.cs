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
        public string getInventoryItemStatus()
        {
            switch (status)
            {
                case 0:
                    return "Normal";
                case 1:
                    return "Broken";
                case 2:
                    return "Lost";
                default:
                    return "Unknown";
            }
        }
    }

    public class InventoryItemWithItem : InventoryItem
    {
        public int inventory_plan_id { get; set; }
        public int item_id { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public int status { get; set; }
        public string inventoryStatus { get; set; }

        public InventoryItemWithItem(InventoryItem inventoryItem, string name)
        {
            this.inventory_plan_id = inventoryItem.inventory_plan_id;
            this.item_id = inventoryItem.item_id;
            this.name = name;
            this.inventoryStatus = inventoryItem.getInventoryItemStatus();       
            this.note = inventoryItem.note;
        }
    }
}
