using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstarmCore.Utils
{
    public static class ErrorsContract
    {
        public const string DB_CREATE = "Error while creating tables in db ";
        public const string DB_CONNECT = "Can`t connect to database ";
        public const string DB_EXECUTE = "Error while executing command ";
        public const string DB_WRITE = "Error while inserting data to db ";
        public const string DB_READ = "Error while reading data from db ";
        public const string DB_FIND = "Can`t find any data in database ";

        public const string ACC_SIGNIN = "Error while signin ";
        public const string ACC_AVATAR = "Error while changing profile picture ";
        public const string ACC_AVATAR_2 = "Error during changing avatar ";
        public const string ACC_NOTFOUND = "File not found: ";
        public const string ACC_MEDIA = "Cant get mediaID, check url ";
        public const string ACC_MEDIA_URL = "Cant get mediaID from mediaUrl ";
        public const string ACC_POST = "Cant post photo: ";
        public const string ACC_COMMENT = "Cant comment media: ";
        public const string ACC_LIKE = "Cant Like media: ";
        public const string ACC_FEED_LIKE = "Cant like in feed explore ";
        public const string ACC_FEED_TAG = "Cant get tag feed ";

        public const string ACC_FOLLOW = "Cant Follow/Unfollow user: ";
        public const string ACC_GET_FOLLOWERS = "Cant get followers: ";

        public const string ACC_DIRECT_INBOX = "Cant get inbox messages ";
        public const string ACC_DIRECT_THREADS = "Cant get inbox threads ";
        public const string ACC_DIRECT_SEND = "Cant send message ";
        public const string ACC_DIRECT_SEND_FAIL = "Error while trying to send message ";

        public const string ACC_CH_PHONE_CONFIRM = "Error confirming phone ";
        public const string ACC_CH_PHONE_ERROR  = "Error while trying get challenge from phone number ";
        
        public const string ACC_CH_EMAIL_CONFIRM_CODE = "Error while trying to send verify code to email ";
        public const string ACC_CH_EMAIL_CONFIRM_EX = "Exception while trying get challenge from email ";
        public const string ACC_CH_EMAIL_CONFIRM_FAIL = "Error Challenge isnt succeed.. ";
        public const string ACC_CH_VERIFY = "Fail to verify login ";
        public const string ACC_CH_TWOFACTOR_FAIL = "Error while trying to get two factor auth ";
        
    }
}
