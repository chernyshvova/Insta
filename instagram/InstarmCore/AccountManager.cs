using InstagramApiSharp.Classes;
using InstarmCore.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstarmCore.Utils;
namespace InstarmCore
{
    public class AccountManager
    {
        List<Profile> profiles = new List<Profile>();
        List<Account> accounts = new List<Account>();

        IDbHelper dbhelper = new DbHelperSQLite();
        ProxyHandler proxyhandler = new ProxyHandler();

        public async Task SetPostByTag(string tag, string imgName, string message)
        {
            try
            {

                SetAccounts(tag);
            }
            catch (Exception)
            {
                
            }
            foreach (var account in accounts)
            {
                try
                {
                    await account.SignIn();
                    await account.SetPost(imgName, message);
                }
                catch (Exception)
                {
                   
                }
               
            }
        }
        public async Task SetPostSingle(string accountName, string imgName, string message)
        {
            try
            {
                Account account = getAccount(accountName);
                await account.SignIn();
                await account.SetPost(imgName, message);
            }
            catch (Exception)
            {
                
            }
           
        }
        public async Task SetPostSinglePath(string accountName, string path, string message)
        {
            try
            {
                Account account = getAccount(accountName);
                await account.SignIn();
                await account.SetPostByPath(path, message);
            }
            catch (Exception)
            {
                
            }
         
        }

        public async Task LikeMediaByTag(string tag, string mediaUrl)
        {
            try
            {
                SetAccounts(tag);
            }
            catch (Exception)
            {
                
            }
            
            foreach (var account in accounts)
            {
                try
                {
                    await account.SignIn();
                    await account.LikeMedia(mediaUrl);
                }
                catch (Exception)
                {
                   
                }
               
            }
        }
        public async Task LikeMediaSingle(string accountName, string mediaUrl)
        {
            try
            {
                Account account = getAccount(accountName);
                await account.SignIn();
                await account.LikeMedia(mediaUrl);
            }
            catch (Exception)
            {
                
            }
   
        }

        public async Task ExploreLikeHashtag(string accountName, string hashtag, string pages)
        {
            try
            {
                Account account = getAccount(accountName);
                await account.SignIn();
                await account.ExploreLikeHashtag(hashtag, int.Parse(pages));
            }
            catch (Exception)
            {
                
            }
            
        }
        public async Task FollowUser(string accountName, string username)
        {
            try
            {
                Account account = getAccount(accountName);
                await account.SignIn();
                await account.FollowUser(username);
            }
            catch (Exception)
            {
               
            }
           
        }
        public async Task UnFollowUser(string accountName, string username)
        {
            try
            {
                Account account = getAccount(accountName);
                await account.SignIn();
                await account.UnFollowUser(username);
            }
            catch (Exception)
            {
            }
          
        }


        public async Task DirectCheckMessages(string accountName)
        {
            try
            {
                Account account = getAccount(accountName);
                await account.SignIn();
                await account.DirectCheckMessages();
            }
            catch (Exception)
            {

            }

        }
        public async Task DirectSendMessage(string accountName, string username, string message)
        {
            try
            {
                Account account = getAccount(accountName);
                await account.SignIn();
                await account.DirectSendMessage(username, message);
            }
            catch (Exception)
            {
            }
          
        }

        public async Task DirectAnswerMessage(string accountName, string username, string message)
        {
            try
            {
                Account account = getAccount(accountName);
                await account.SignIn();
                await account.DirectAnswerMessage(username, message);
            }
            catch (Exception)
            {
            }
           
        }


        public async Task CommentMedia(string accountName, string mediaUrl, string message)
        {
            try
            {
                Account account = getAccount(accountName);
                await account.SignIn();
                await account.CommentMedia(mediaUrl, message);
            }
            catch (Exception)
            {
               
            }
           
        }

        public async Task ChangeAvatar(string accountName, string imgName)
        {
            try
            {
                Account account = getAccount(accountName);
                await account.SignIn();
                await account.ChangeAvatar(imgName);
            }
            catch (Exception)
            {
            
            }
          
        }
        public async Task ChangeAvatarPath(string accountName, string path)
        {
            try
            {
                Account account = getAccount(accountName);
                await account.SignIn();
                await account.ChangeAvatarPath(path);
            }
            catch (Exception)
            {
               
            }
           
        }

        private void SetAccounts(string tag)
        {
            Clear();
            profiles = dbhelper.GetProfilesByTag(tag);  //получаем данные аккаунтов из базы данных
            foreach (var item in profiles)
            {
                UserSessionData userSession = new UserSessionData
                {
                    UserName = item.name,
                    Password = item.password
                };

                if (item.proxyHost != null)
                {
                    ProxyData data = new ProxyData(item.proxyHost, item.proxyPort, item.proxyUsername, item.proxyPassword);
                    accounts.Add(new Account(userSession, proxyhandler.getProxy(data), data));
                    Console.WriteLine("Proxies added to session");
                }
                if (item.proxyHost == null)
                {
                    accounts.Add(new Account(userSession));
                }
               
            }
        }

        public Account getAccount(string accountName)
        {
            if (string.IsNullOrEmpty(accountName))
            {
                ExceptionUtils.SetState(Error.E_DATA_NOT_FOUND, ErrorsContract.ACC_NAME);
                System.ArgumentException argEx = new System.ArgumentException(ErrorsContract.ACC_NAME);
                throw argEx;
            }
            Profile profile = new Profile();
            profile = dbhelper.GetProfileByName(accountName);
            if (string.IsNullOrEmpty(profile.name))
            {
                ExceptionUtils.SetState(Error.E_DATA_NOT_FOUND, ErrorsContract.ACC_NAME);
                System.ArgumentException argEx = new System.ArgumentException(ErrorsContract.ACC_NAME);
                throw argEx;
            }
            if (string.IsNullOrEmpty(profile.password))
            {
                ExceptionUtils.SetState(Error.E_DATA_NOT_FOUND, ErrorsContract.ACC_PASSWORD);
                System.ArgumentException argEx = new System.ArgumentException(ErrorsContract.ACC_PASSWORD);
                throw argEx;
            }
            UserSessionData userSession = new UserSessionData
            {
                UserName = profile.name,
                Password = profile.password
            };
            if (!string.IsNullOrEmpty(profile.proxyHost))
            {
                ProxyData data = new ProxyData(profile.proxyHost, profile.proxyPort, profile.proxyUsername, profile.proxyPassword);
                Account account = new Account(userSession, proxyhandler.getProxy(data), data);
                return account;
            }
            else
            {
                Account account = new Account(userSession);
                return account;
            }

        }

        private void Clear()
        {
            profiles.Clear();
            accounts.Clear();
        }

    }
}