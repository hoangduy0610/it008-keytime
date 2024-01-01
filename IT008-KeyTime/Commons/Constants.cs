using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_KeyTime.Commons
{
    internal class Constants
    {
        // Edit the Connection String here
        private const string host = "localhost";
        private const string port = "5432";
        private const string username = "postgres";
        private const string password = "postgres";
        private const string dbName = "keytime";

        public const string PostgresConnection = "Host=ep-shrill-thunder-650028.us-east-2.aws.neon.tech;Username=hoangduy06104;Password=u2PnqmQ1WSXt;Database=keytime";
        //public const string PostgresConnection = "Host=" + host + ";Port=" + port +";Username=" + username + ";Password=" + password + ";Database=" + dbName;
    }
}
