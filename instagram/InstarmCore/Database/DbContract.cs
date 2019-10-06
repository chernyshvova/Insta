
namespace InstarmCore.Database
{
    public static class DbContract
    {
        public const string DATABASE = "instagram.db";
        public const string TABLENAME = "main";
        public const string TABLEMSG = "messages";
        public const string USERNAME = "username";
        public const string PASSWORD = "password";
        public const string TAG = "tag";
        public const string PROXYHOST = "proxyhost";
        public const string PROXYPORT = "proxyport";
        public const string PROXYUSERNAME = "proxyUsername";
        public const string PROXYPASSWORD = "proxyPassword";
        public const string SELECTPROFILE = "SELECT " + USERNAME + "," + PASSWORD + "," + TAG + "," + PROXYHOST + "," + PROXYPORT + "," + PROXYUSERNAME + "," + PROXYPASSWORD + " FROM " + TABLENAME;
        public const string CREATEACCOUNTTABLE = "CREATE TABLE IF NOT EXISTS "+ TABLENAME + " (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, username TEXT, password TEXT UNIQUE, tag TEXT, proxyhost TEXT, proxyport TEXT, proxyUsername TEXT, proxyPassword TEXT);";
        public const string CREATEMSGTABLE = "CREATE TABLE IF NOT EXISTS " + TABLEMSG + " (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, messageid TEXT UNIQUE, threadid TEXT, sender TEXT, reciver TEXT, message TEXT, Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP);";
        public const string INSERTMESSAGE = "INSERT INTO " + TABLEMSG + " ( messageid , threadid, sender, reciver, message, Timestamp) VALUES (@id, @threadid, @sender, @reciver, @message , @time);";
    }
}
