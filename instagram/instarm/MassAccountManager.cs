using InstagramApiSharp.Classes;
using instarm.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace instarm
{
    class MassAccountManager
    {
        List<Profile> profiles = new List<Profile>();
        List<Account> accounts = new List<Account>();

        IDbHelper dbhelper = new DbHelperSQLite();
        ProxyHandler proxyhandler = new ProxyHandler();

        public async Task SetPostByTag(string tag, string imgName, string message)
        {
            SetAccounts(tag);
            foreach (var account in accounts)
            {
                await account.SignIn();
                await account.SetPost(imgName, message);
            }
        }
        public async Task SetPostSingle(string accountName, string imgName, string message)
        {
            Account account = getAccount(accountName);
            await account.SignIn();
            await account.SetPost(imgName, message);
        }

        public async Task LikeMediaByTag(string tag, string mediaUrl)
        {
            SetAccounts(tag);
            foreach (var account in accounts)
            {
                await account.SignIn();
                await account.LikeMedia(mediaUrl);
            }
        }
        public async Task LikeMediaSingle(string accountName, string mediaUrl)
        {
            Account account = getAccount(accountName);
            await account.SignIn();
            await account.LikeMedia(mediaUrl);         
        }

        public async Task ExploreLikeHashtag(string accountName, string hashtag, string pages)
        {
            Account account = getAccount(accountName);
            await account.SignIn();
            await account.ExploreLikeHashtag(hashtag, int.Parse(pages));          
        }
        public async Task FollowUser(string accountName, string username) {
            Account account = getAccount(accountName);
            await account.SignIn();
            await account.FollowUser(username);       
        }
        public async Task UnFollowUser(string accountName, string username)
        {
            Account account = getAccount(accountName);
            await account.SignIn();
            await account.UnFollowUser(username);
        }


        public async Task DirectCheckMessages(string accountName)
        {
            Account account = getAccount(accountName);
            await account.SignIn();
            await account.DirectCheckMessages();
        }
        public async Task DirectSendMessage(string accountName, string username, string message)
        {
            Account account = getAccount(accountName);
            await account.SignIn();
            await account.DirectSendMessage(username, message);
        }

        public async Task DirectAnswerMessage(string accountName, string username, string message)
        {
            Account account = getAccount(accountName);
            await account.SignIn();
            await account.DirectAnswerMessage(username, message);
        }


        public async Task CommentMedia(string accountName, string mediaUrl, string message)
        {
            Account account = getAccount(accountName);
            await account.SignIn();
            await account.CommentMedia(mediaUrl, message);
        }

        public async Task ChangeAvatar(string accountName, string imgName)
        {
            Account account = getAccount(accountName);
            await account.SignIn();
            await account.ChangeAvatar(imgName);     
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

        private Account getAccount(string accountName)
        {
            Profile profile = new Profile();
            profile = dbhelper.GetProfileByName(accountName);
            UserSessionData userSession = new UserSessionData
            {
                UserName = profile.name,
                Password = profile.password
            };
            if (profile.proxyHost != null)
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