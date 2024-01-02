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
        private string host = "localhost";
        private string port = "5432";
        private string username = "postgres";
        private string password = "postgres";
        private string dbName = "keytime";

        public string PostgresConnection = "Host=ep-shrill-thunder-650028.us-east-2.aws.neon.tech;Username=hoangduy06104;Password=u2PnqmQ1WSXt;Database=keytime";
        //public string PostgresConnection = "Host=" + host + ";Port=" + port +";Username=" + username + ";Password=" + password + ";Database=" + dbName;

        public Constants()
        {
            Ini MyIni = new Ini("config.ini");

            if(!MyIni.KeyExists("DBHOST", "KeyTime"))
            {
                MyIni.Write("DBHOST", "ep-shrill-thunder-650028.us-east-2.aws.neon.tech", "KeyTime");
            }
            if (!MyIni.KeyExists("DBPORT", "KeyTime"))
            {
                MyIni.Write("DBPORT", "5432", "KeyTime");
            }
            if (!MyIni.KeyExists("DBUSER", "KeyTime"))
            {
                MyIni.Write("DBUSER", "hoangduy06104", "KeyTime");
            }
            if (!MyIni.KeyExists("DBPASS", "KeyTime"))
            {
                MyIni.Write("DBPASS", "u2PnqmQ1WSXt", "KeyTime");
            }
            if (!MyIni.KeyExists("DBNAME", "KeyTime"))
            {
                MyIni.Write("DBNAME", "keytime", "KeyTime");
            }

            this.host = MyIni.Read("DBHOST", "KeyTime");
            this.port = MyIni.Read("DBPORT", "KeyTime");
            this.username = MyIni.Read("DBUSER", "KeyTime");
            this.password = MyIni.Read("DBPASS", "KeyTime");
            this.dbName = MyIni.Read("DBNAME", "KeyTime");

            this.PostgresConnection = "Host=" + host + ";Port=" + port + ";Username=" + username + ";Password=" + password + ";Database=" + dbName;
        }
    }
}
