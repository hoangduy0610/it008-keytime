using Dapper.Contrib.Extensions;
using IT008_KeyTime.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_KeyTime.Models
{
    [Table("tbl_users")]
    public class User
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public int role { get; set; }
        public string GetRoleString()
        {
            switch (role)
            {
                case 1:
                    return "Admin";
                case 2:
                    return "Property Manager";
                case 3:
                    return "Inventory Manager";
                case 4:
                    return "User";
                default:
                    return "Unknown";
            }
        }
    }

    public class MapUser : User
    {
        public string user_role { get; set; }

        public MapUser(User user)
        {
            this.id = user.id;
            this.username = user.username;
            this.password = user.password;
            this.name = user.name;
            this.email = user.email;
            this.phone = user.phone;
            this.address = user.address;
            this.user_role = user.GetRoleString();
            //this.roleString = Enum.GetName(typeof(Roles), user.role);
        }
    }
}
