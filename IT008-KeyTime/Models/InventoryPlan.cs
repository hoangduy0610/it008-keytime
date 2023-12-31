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

    public class InventoryPlanWithAssigneeName
    {
        public int id { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public int assignee_id { get; set; }
        public int status { get; set; }
        public DateTime deadline { get; set; }
        public string assigneeName { get; set; }
        public string statusName { get; set; }
        public InventoryPlanWithAssigneeName(InventoryPlan inventoryPlan, string assigneeName)
        {
            this.id = inventoryPlan.id;
            this.name = inventoryPlan.name;
            this.note = inventoryPlan.note;
            this.assignee_id = inventoryPlan.assignee_id;
            this.status = inventoryPlan.status;
            this.deadline = inventoryPlan.deadline;
            this.assigneeName = assigneeName;
            // statusName based on enum InventoryPlanStatus with value status
            this.statusName = Enum.GetName(typeof(Enums.InventoryPlanStatus), inventoryPlan.status);
        }
    }
}
