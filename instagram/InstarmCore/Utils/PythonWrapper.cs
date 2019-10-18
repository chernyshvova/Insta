using System;
using System.Dynamic;
using System.Threading.Tasks;

namespace InstarmCore.Utils
{
    public class PythonWrapper : DynamicObject
    {
        AccountManager mg = new AccountManager();

        public int GetAccount(string accountName, out string result)
        {
            ExceptionUtils.SetDefault();
            result = "";
            try
            {
                Account acc = mg.getAccount(accountName);
            }
            catch (Exception)
            {
                result = ExceptionUtils.ErrorMessage;
                return (int)ExceptionUtils.currentState;
            }
           
            result = ExceptionUtils.ErrorMessage;
            return (int)ExceptionUtils.currentState;
        }
        public int SingIn(string accountName, out string result)
        {
            ExceptionUtils.SetDefault();
            result = "";
            var task = _SingIn(accountName);
            task.Wait();
            result = ExceptionUtils.ErrorMessage;
            return (int)ExceptionUtils.currentState;

        }
        private async Task _SingIn(string name)
        {
            await mg.SignIn(name);
        }




        public int SetPost(string accountName, string path, string message, out string result)
        {
            ExceptionUtils.SetDefault();
            result = "";
            var taskResult = _SetPost(accountName, path, message);
            result = ExceptionUtils.ErrorMessage;
            return (int)ExceptionUtils.currentState;
        }
        private async Task _SetPost(string name, string path, string message)
        {
            await mg.SetPostSinglePath(name, path, message);
        }

        public int LikeMedia(string accountName, string medioaUrl, out string result)
        {
            result = "";
            ExceptionUtils.SetDefault();
            var taskResult = _LikeMedia(accountName, medioaUrl);
            result = ExceptionUtils.ErrorMessage;
            return (int)ExceptionUtils.currentState;
        }
        private async Task _LikeMedia(string accountName, string medioaUrl)
        {
            await mg.LikeMediaSingle(accountName, medioaUrl);
        }



        public int FollowUser(string accountName, string username ,out string result)
        {
            ExceptionUtils.SetDefault();
            result = "";
            var taskResult = _FollowUser(accountName, username);
            result = ExceptionUtils.ErrorMessage;
            return (int)ExceptionUtils.currentState;
        }
        private async Task _FollowUser(string accountName, string username)
        {
            await mg.FollowUser(accountName, username);
        }

        public int UnFollowUser(string accountName, string username, out string result)
        {
            ExceptionUtils.SetDefault();
            result = "";
            var taskResult = _UnFollowUser(accountName, username);
            result = ExceptionUtils.ErrorMessage;
            return (int)ExceptionUtils.currentState;
        }
        private async Task _UnFollowUser(string accountName, string username)
        {
            await mg.UnFollowUser(accountName, username);
        }

        public int DirectCheck(string accountName, out string result)
        {
            ExceptionUtils.SetDefault();
            result = "";
            var taskResult = _DirectCheck(accountName);

            result = ExceptionUtils.ErrorMessage;
            return (int)ExceptionUtils.currentState;
        }
        private async Task _DirectCheck(string accountName)
        {
            await mg.DirectCheckMessages(accountName);
        }

        public int DirectSend(string accountName, string targetAccountName, string message, out string result)
        {
            ExceptionUtils.SetDefault();
            result = "";
            var taskResult = _DirectSend(accountName, targetAccountName, message);

            result = ExceptionUtils.ErrorMessage;
            return (int)ExceptionUtils.currentState;
        }
        private async Task _DirectSend(string accountName, string targetAccountName, string message)
        {
            await mg.DirectSendMessage(accountName, targetAccountName, message);
        }

        public int CommentMedia(string accountName, string mediaUrl, string message , out string result)
        {
            ExceptionUtils.SetDefault();
            result = "";
            var taskResult = _CommentMedia(accountName, mediaUrl, message);

            result = ExceptionUtils.ErrorMessage;
            return (int)ExceptionUtils.currentState;
        }
        private async Task _CommentMedia(string accountName, string mediaUlr, string message)
        {
            await mg.CommentMedia(accountName, mediaUlr, message);
        }



        public int SetAvatar(string accountName, string imgName, out string result)
        {
            ExceptionUtils.SetDefault();
            result = "";
            var taskResult = _SetAvatar(accountName, imgName);

            result = ExceptionUtils.ErrorMessage;
            return (int)ExceptionUtils.currentState;
        }
        private async Task _SetAvatar(string accountName, string imgName)
        {
            await mg.ChangeAvatar(accountName, imgName);
        }
    }
}
