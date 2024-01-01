using IT008_KeyTime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_KeyTime.Commons
{
    internal class Store
    {
        public static User _user;
        public static User _currentEditing;
        public static RentalItem _currentRentalItem;
        public static InventoryPlanWithAssigneeName _currentInventoryPlan;
        public static InventoryItem _currentInventoryItem;
    }
}
