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
        public MapItem(Item item)
        {
            this.id = item.id;
            this.name = item.name;
            this.room = item.room;
            this.description = item.description;
            this.item_status = item.GetItemString();
            this.note = item.note;
        }
    }
}
