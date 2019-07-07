using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace instarm.Database
{
    public class DbHelperSQLite : IDbHelper
    {
        SQLiteConnection sqlConnection; 

        private void Connect()
        {
            try
            {
                sqlConnection = new SQLiteConnection()
                {
                    ConnectionString = "Data Source = " + DbContract.DATABASE
                };
                sqlConnection.Open();               
                }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Can`t connect to database");
                Console.WriteLine(ex.Message);
            }

            if (sqlConnection.State == ConnectionState.Open)
            {
                    // Do Something          
            }
        }

        public List<Profile> GetProfilesByTag(string tag)
        {
            List<Profile> profilesList = new List<Profile>();
            Connect();
            var data = DoTask(DbContract.SELECTPROFILE + " where " + DbContract.TAG + "='" + tag + "';");
            if (!data.HasRows)
            {
                System.ArgumentException argEx = new System.ArgumentException("Can`t find any data by tag in database");
                throw argEx;
            }
            while (data.Read())
            {
                Profile profile = new Profile();
                profile = DbDataReader(data);
                profilesList.Add(profile);
            }
            Close();
            return profilesList;
        }
        public Profile GetProfileByName(string name)
        {
            Connect();
            Profile profile = new Profile();
            var data = DoTask(DbContract.SELECTPROFILE + " where " + DbContract.USERNAME + "='" + name + "';");
            if (!data.HasRows)
            {
                System.ArgumentException argEx = new System.ArgumentException("Can`t find any data by profilename in database");
                throw argEx;
            }
            if (data.Read())
            {
                profile = DbDataReader(data);
                Close();
                return profile;
            }
            else
            {
                Console.WriteLine("Error while reading data from db");
                Close();
                return profile;
            }
        }
        private Profile DbDataReader(SQLiteDataReader reader)
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

        private SQLiteDataReader DoTask(string sqlCommandText)
        {
            SQLiteCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = sqlCommandText;
            return cmd.ExecuteReader();
        }

        private void Close()
        {
            sqlConnection.Close();
        }
    }
}
