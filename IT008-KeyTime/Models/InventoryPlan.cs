using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_KeyTime.Models
{
    [Table("tbl_inventory_plans")]
    public class InventoryPlan
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public int assignee_id { get; set; }
        public int status { get; set; }
        public DateTime deadline { get; set; }
    }
}
