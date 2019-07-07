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
                await account.SignIn();
                await account.SetPost(imgName, message);
            }
            if (profile.proxyHost == null)
            {
                Account account = new Account(userSession);
                await account.SignIn();
                await account.SetPost(imgName, message);
            }

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
                await account.SignIn();
                await account.LikeMedia(mediaUrl);
            }
            if (profile.proxyHost == null)
            {
                Account account = new Account(userSession);
                await account.SignIn();
                await account.LikeMedia(mediaUrl);
            }
        }

        public async Task CommentMedia(string accountName, string mediaUrl, string message)
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
                await account.SignIn();
                await account.CommentMedia(mediaUrl, message);
            }
            if (profile.proxyHost == null)
            {
                Account account = new Account(userSession);
                await account.SignIn();
                await account.CommentMedia(mediaUrl, message);
            }

        }

        public async Task ChangeAvatar(string accountName, string imgName)
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
                await account.SignIn();
                await account.ChangeAvatar(imgName);
            }
            if (profile.proxyHost == null)
            {
                Account account = new Account(userSession);
                await account.SignIn();
                await account.ChangeAvatar(imgName);
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


        private void Clear()
        {
            profiles.Clear();
            accounts.Clear();
        }

    }
}