using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_KeyTime.Models
{
    [Table("rls_rental_items")]
    public class RentalItem
    {
        [Key]
        public int id { get; set; }
        public int user_id { get; set; }
        public int item_id { get; set; }
        public DateTime rental_start { get; set; }
        public DateTime expect_return { get; set; }
        public DateTime actual_return { get; set; }
        public int status { get; set; }
        public string getRentalStatus()
        {
            switch (status)
            {
                case 0:
                    return "Requested";
                case 1:
                    return "Approved";
                case 2:
                    return "Late";
                case 3:
                    return "Returned";
                case 4:
                    return "Canceled";
                default:
                    return "Unknown";
            }
        }
    }

    public class MapRentalItem : RentalItem 
    {
        public string rental_status { get; set; }
        public string item_name { get; set; }
        public string user_name { get; set; }
        public MapRentalItem(RentalItem rentalItem, string item_name, string user_name)
        {
            this.id = rentalItem.id;
            this.user_id = rentalItem.user_id;
            this.item_id = rentalItem.item_id;
            this.rental_start = rentalItem.rental_start;
            this.expect_return = rentalItem.expect_return;
            this.actual_return = rentalItem.actual_return;
            this.rental_status = rentalItem.getRentalStatus();
            this.item_name = item_name;
            this.user_name = user_name;
            //this.roleString = Enum.GetName(typeof(Roles), user.role);
        }
    } 
}
