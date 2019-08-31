﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace InstarmGUI.Database
{
    public class DbHelperSQLite
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
             //   Console.WriteLine("Can`t connect to database");
                Console.WriteLine(ex.Message);
            }

            if (sqlConnection.State == ConnectionState.Open)
            {
                // Do Something          
            }
        }
        private void Connect(string path)
        {
            try
            {
                sqlConnection = new SQLiteConnection()
                {
                    ConnectionString = "Data Source = " + path
                };

                sqlConnection.Open();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
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
                // Console.WriteLine("Can`t find any data by profilename in database");
                ArgumentException argEx = new System.ArgumentException("Can`t find any data by profilename in database");
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
           //     Console.WriteLine("Error while reading data from db");
                Close();
                return profile;
            }
        }
        public List<Profile> GetProfiles(string path)
        {
            List<Profile> profiles = new List<Profile>();
            bool isValid = true;
            if (string.IsNullOrEmpty(path))
            {
                isValid = false;
            }
            if (isValid)
            {
                Connect(path);
                var data = DoTask(DbContract.SELECTPROFILE + ";");
                if (!data.HasRows)
                {
                    //   System.ArgumentException argEx = new System.ArgumentException("Can`t find any data by tag in database");
                    MessageBox.Show("Can`t find any data in database");
                    // throw argEx;
                }
                while (data.Read())
                {
                    Profile profile = new Profile();
                    profile = DbDataReader(data);
                    profiles.Add(profile);
                }
                Close();
            }
           
            return profiles;
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

        public void CreateTables()
        {
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
            catch (Exception)
            {
                MessageBox.Show("Error while creating tables in db");
                throw;
            }

        }

        private void Close()
        {
            sqlConnection.Close();
        }

    }
}