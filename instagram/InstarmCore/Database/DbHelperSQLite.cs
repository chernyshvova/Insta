using InstarmCore.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace InstarmCore.Database
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
                Console.WriteLine(Utils.ErrorsContract.DB_CONNECT);
                Console.WriteLine(ErrorsContract.DB_CONNECT + ex.Message);
                ExeptionUtils.SetState(Error.E_DB_CONNECTION, ex.Message);
                throw ex;
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
                System.ArgumentException argEx = new System.ArgumentException(ErrorsContract.DB_FIND);
                ExeptionUtils.SetState(Error.E_DB_DATA_NOT_FOUND, ErrorsContract.DB_FIND);
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
                Console.WriteLine(ErrorsContract.DB_FIND);
                System.ArgumentException argEx = new System.ArgumentException(ErrorsContract.DB_FIND);
                ExeptionUtils.SetState(Error.E_DB_DATA_NOT_FOUND, ErrorsContract.DB_FIND);
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
                Console.WriteLine(ErrorsContract.DB_READ);
                Close();
                System.ArgumentException argEx = new System.ArgumentException(ErrorsContract.DB_READ);
                ExeptionUtils.SetState(Error.E_DB_READING, ErrorsContract.DB_READ);
                throw argEx; 
            }
        }

        public void writeMessages(List<Message> data)
        {
            Connect();
            SQLiteCommand cmd = sqlConnection.CreateCommand();
            foreach (var item in data)
            {
                try
                {
                    cmd.CommandText = DbContract.INSERTMESSAGE;
                    cmd.Parameters.AddWithValue("@id", item.id);
                    cmd.Parameters.AddWithValue("@threadid", item.threadId);
                    cmd.Parameters.AddWithValue("@sender", item.sender);
                    cmd.Parameters.AddWithValue("@reciver", item.reciver);
                    cmd.Parameters.AddWithValue("@message", item.message);
                    cmd.Parameters.AddWithValue("@time", item.timestamp);

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ErrorsContract.DB_READ + ex);
                    ExeptionUtils.SetState(Error.E_DB_WRITING, ErrorsContract.DB_READ + ex);
                    throw ex;
                }
                
            }
            Close();
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
            try
            {
                SQLiteCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = sqlCommandText;
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ErrorsContract.DB_EXECUTE + ex);
                ExeptionUtils.SetState(Error.E_DB_WRITING, ErrorsContract.DB_EXECUTE + ex);
                throw ex;
            }

        }

        public void CreateTables() {
            try
            {
                if (!File.Exists(Environment.CurrentDirectory + @"\" + DbContract.DATABASE))
                {
                    SQLiteConnection.CreateFile(DbContract.DATABASE);
                }
                Connect();
                SQLiteCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = DbContract.CREATEACCOUNTTABLE;
                cmd.ExecuteNonQuery();
                cmd.CommandText = DbContract.CREATEMSGTABLE;
                cmd.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ErrorsContract.DB_CREATE + ex);
                ExeptionUtils.SetState(Error.E_DB_CREATING, ErrorsContract.DB_CREATE + ex);
                throw ex;
            }
          
        }

        private void Close()
        {
            sqlConnection.Close();
        }

    }
}
