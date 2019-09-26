using InstarmCore.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace InstarmCore
{
    public class DbHelperMySQL : IDbHelper
    {
        MySqlConnection mysql_connection;


        private void Connect()
        {
            string host = "localhost"; 
            string database = "instagram";
            string user = "root"; 
            string password = "Alpha12345";

            string Connect = "Database=" + database + ";Datasource=" + host + ";User=" + user + ";Password=" + password;
            mysql_connection = new MySqlConnection(Connect);
       
            try
            {
                mysql_connection.Open();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable connect to database:  " + Connect);
                throw;
            }
        }

       
        public Profile GetProfileByName(string name)
        {
            Connect();
            Profile profile = new Profile();
            var data = DoTask(DbContract.SELECTPROFILE + " where " + DbContract.USERNAME + "='" + name + "';");
            if (data.Read())
            {
                profile = DbDataReader(data);
                return profile;
            }
            else
	        {
                Console.WriteLine("Error while reading data from db");
                System.ArgumentException argEx = new System.ArgumentException("Error while read database from GetProfileByName() ");
                throw argEx;
            }
        }

        private Profile DbDataReader(MySqlDataReader reader)
        {
            Profile prof = new Profile();
                if (!reader.IsDBNull(0))
                {
                    prof.name = reader.GetString(0);
                }
                if (!reader.IsDBNull(1))
                {
                    prof.password = reader.GetString(1);
                }
                if (!reader.IsDBNull(2))
                {
                    prof.tag = reader.GetString(2);
                }
                if (!reader.IsDBNull(3))
                {
                    prof.proxyHost = reader.GetString(3);
                }
                if (!reader.IsDBNull(4))
                {
                    prof.proxyPort = reader.GetString(4);
                }
                if (!reader.IsDBNull(5))
                {
                    prof.proxyUsername = reader.GetString(5);
                }
                if (!reader.IsDBNull(6))
                {
                    prof.proxyPassword = reader.GetString(6);
                }
            return prof;
        }

        public List<Profile> GetProfilesByTag(string tag)
        {
            List<Profile> profilesList = new List<Profile>();
            Connect();
            var data = DoTask(DbContract.SELECTPROFILE + " where " + DbContract.TAG + "='" + tag + "';");
            while (data.Read())
            {
                Profile profile = new Profile();
                profile = DbDataReader(data);
                profilesList.Add(profile);
            }
            Close();
            return profilesList;
        }
        public void writeMessages(List<Message> data)
        {
            //реализовать функцию
        }
        private void Close() {
            mysql_connection.Close();
        }

        private MySqlDataReader DoTask(string sqlCommandText)
        {
            MySqlCommand mysql_query = mysql_connection.CreateCommand();
            mysql_query.CommandText = sqlCommandText;
            return mysql_query.ExecuteReader();
        }
    }
}
