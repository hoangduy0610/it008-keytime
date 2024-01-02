using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_KeyTime.Models
{
    [Table("tbl_items")]
    public class Item
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string room { get; set; }
        public string description { get; set; }
        public int status { get; set; }
        public string note { get; set; }
       
        public string GetItemString()
        {
            switch (status)
            {
                case 0:
                    return "Idle";
                case 1:
                    return "In use";
                case 2:
                    return "Broken";
                case 3:
                    return "Lost";
                default:
                    return "Unknown";
            }
        }

        
    }

    public class MapItem : Item 
    {
        public string item_status { get; set; }
        public string GetInventoryStatusString(int status)
        {
            switch (status)
            {
                case 0:
                    return "NORMAL";
                case 1:
                    return "BROKEN";
                case 2:
                    return "LOST";
                default:
                    return "Unknown";
            }
        }
        public MapItem(Item item)
        {
            this.id = item.id;
            this.name = item.name;
            this.room = item.room;
            this.description = item.description;
            this.item_status = item.GetItemString();
            this.note = item.note;
        }
        public MapItem(Item item, InventoryItem iitem, bool isInventory)
        {
            this.id = item.id;
            this.name = item.name;
            this.room = item.room;
            this.description = item.description;
            this.status = iitem.status;
            this.item_status = (isInventory ? GetInventoryStatusString(iitem.status) :item.GetItemString());
            this.note = iitem.note;
        }
    }
}
