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
    }
}
