
namespace instarm.Database
{
    public static class DbContract
    {
        public const string DATABASE = "instagram.db";
        public const string TABLENAME = "main";
        public const string USERNAME = "username";
        public const string PASSWORD = "password";
        public const string TAG = "tag";
        public const string PROXYHOST = "proxyhost";
        public const string PROXYPORT = "proxyport";
        public const string PROXYUSERNAME = "proxyUsername";
        public const string PROXYPASSWORD = "proxyPassword";
        public const string SELECTPROFILE = "SELECT " + USERNAME + "," + PASSWORD + "," + TAG + "," + PROXYHOST + "," + PROXYPORT + "," + PROXYUSERNAME + "," + PROXYPASSWORD + " FROM " + TABLENAME;

        
    }
}
