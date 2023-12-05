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
    }
}
