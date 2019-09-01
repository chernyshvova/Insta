using System;
using System.Threading.Tasks;
using System.IO;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;
using System.Net.Http;
using InstagramApiSharp;
using System.Linq;
using InstagramApiSharp.Classes.SessionHandlers;
using System.Text.RegularExpressions;

namespace instarm
{
    internal class Account : IAccount
    {
        private readonly IInstaApi InstaApi;
        private readonly UserSessionData userSession;
        private ProxyData proxydata = new ProxyData(null, null,null,null);

        public Account(UserSessionData userSession)
        {
            this.userSession = userSession;
            InstaApi = InstaApiBuilder.CreateBuilder()
            .SetUser(userSession)
            .UseLogger(new DebugLogger(LogLevel.All)) // use logger for requests and debug messages
            .SetRequestDelay(RequestDelay.FromSeconds(2, 2))  //отсрочка запросов
            .SetSessionHandler(new FileSessionHandler() { FilePath = (Environment.CurrentDirectory + PathContract.pathAccount + userSession.UserName + PathContract.stateFile) })
            .Build();
        }
        public Account(UserSessionData userSession, HttpClientHandler proxy, ProxyData data)
        {
            proxydata = data;
            this.userSession = userSession;
            InstaApi = InstaApiBuilder.CreateBuilder()
            .SetUser(userSession)
            .UseLogger(new DebugLogger(LogLevel.All))
            .SetRequestDelay(RequestDelay.FromSeconds(2, 2))
            .SetSessionHandler(new FileSessionHandler() { FilePath = (Environment.CurrentDirectory + PathContract.pathAccount + userSession.UserName + PathContract.stateFile) })
            .UseHttpClientHandler(proxy)
            .Build();
        }

        /// <summary>
        /// Вход в аккаунт
        /// </summary>
        /// <returns></returns>
        public async Task SignIn()
        {
            var delay = RequestDelay.FromSeconds(2, 2);
            var relatedPath = Environment.CurrentDirectory + PathContract.pathAccount + userSession.UserName + PathContract.stateFile;
            try
            {
                if (File.Exists(relatedPath))
                {
                    Console.WriteLine("Loading state from file");
                    using (var fs = File.OpenRead(relatedPath))
                    {
                        InstaApi.LoadStateDataFromStream(fs);
                    }
                }
                else
                {
                    var currentUser = await InstaApi.LoginAsync();
                    if (currentUser.Succeeded)
                    {
                        SaveSession();
                    }
                    else
                    {
                        await GetChallenge();
                        SaveSession();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            LoadSession();
            if (!InstaApi.IsUserAuthenticated)
            {
                var currentUser = await InstaApi.LoginAsync();              
                if (currentUser.Succeeded)
                {
                    SaveSession();
                }
                else
                {
                    await GetChallenge();
                    SaveSession();
                    LoadSession();
                }   
            }
            else
            {
                Console.WriteLine("Session loaded!");
            }
        }

        /// <summary>
        /// Смена аватарки профиля
        /// </summary>
        /// <returns></returns>
        public async Task ChangeAvatar(string imgName)
        {
            var picturePath = Environment.CurrentDirectory + PathContract.pathAvatar + imgName;
            await ChangeAvatarAsync(picturePath);
        }
        public async Task ChangeAvatarPath(string path)
        {
            await ChangeAvatarAsync(path);
        }

        private async Task ChangeAvatarAsync(string path)
        {
            // note: only JPG and JPEG format will accept it in instagram!
            if (File.Exists(path))
            {
                try
                {
                    var pictureBytes = File.ReadAllBytes(path);
                    var result = await InstaApi.AccountProcessor.ChangeProfilePictureAsync(pictureBytes);
                    if (result.Succeeded)
                    {
                        Console.WriteLine("New profile picture: " + result.Value.ProfilePicUrl);
                    }
                    else
                        Console.WriteLine("Error while changing profile picture: " + result.Info.Message);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error: No access to image");
                }
            }
            else
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory + PathContract.pathAvatar);
                dirInfo.Create();
                Console.WriteLine("File not found : " + path);
                Console.WriteLine("Add image into next directory: " + Environment.CurrentDirectory + PathContract.pathAvatar);
            }
        }
        private string GetIdFromUri(string mediaUrl)
        {
            Task<string> task = Task.Run(async () => await media(mediaUrl));
            task.Wait();
            if (task.Result != null)
            {
                return task.Result;
            }
            else
            {
                System.ArgumentException argEx = new System.ArgumentException("Cant get mediaID, check url");
                throw argEx;
            }

        }

        private async Task<string> media(string mediaUrl) {
            var mediaId = await InstaApi.MediaProcessor.GetMediaIdFromUrlAsync(new Uri(mediaUrl));
            if (!mediaId.Succeeded)
            {
                return null;
            }        
            return mediaId.Value.ToString();
        }


        /// <summary>
        /// Добавление поста на стенку
        /// </summary>
        /// <param name="imgName">Путь к изображению вида img.jpg</param>
        /// <param name="message">Подпись к изображению</param>
        /// <returns></returns>
        public async Task SetPost(string imgName, string message)
        {
            var relatedPath = Environment.CurrentDirectory + PathContract.pathImg + imgName;
            await SetPostAsync(relatedPath, message);
        }

        public async Task SetPostByPath(string path, string message)
        {
            await SetPostAsync(path, message);
        }
        private async Task SetPostAsync(string path, string message) {
            if (File.Exists(path))
            {
                Console.WriteLine("Loading..");
                var mediaImage = new InstaImageUpload
                {
                    // leave zero, if you don't know how height and width is it.
                    Height = 0,
                    Width = 0,
                    Uri = path
                };
                /* Add user tag (tag people)
                mediaImage.UserTags.Add(new InstaUserTagUpload
                {
                    Username = "chol",
                    X = 0.5,
                    Y = 0.5
                }); */
                var result = await InstaApi.MediaProcessor.UploadPhotoAsync(mediaImage, message);
                Console.WriteLine(result.Succeeded
                    ? $"Media created: {result.Value.Pk}, {result.Value.Caption}"
                    : $"Unable to upload photo: {result.Info.Message}");
            }
            else
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory + PathContract.pathImg);
                dirInfo.Create();
                Console.WriteLine("File not found : " + path);
                Console.WriteLine("Add image into next directory: " + Environment.CurrentDirectory + PathContract.pathImg);
            }
        }


        /// <summary>
        /// Комментирование поста
        /// </summary>
        /// <param name="mediaUrl">Ссылкак на изображение</param>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        public async Task CommentMedia(string mediaUrl, string message) {
            var commentResult = await InstaApi.CommentProcessor.CommentMediaAsync(GetIdFromUri(mediaUrl), message);
            Console.WriteLine(commentResult.Succeeded
                ? $"Comment created: {commentResult.Value.Pk}, text: {commentResult.Value.Text}"
                : $"Unable to create comment: {commentResult.Info.Message}");
        }

        /// <summary>
        /// Лайк изображения
        /// </summary>
        /// <param name="mediaUrl">Ссылкак на изображение<</param>
        /// <returns></returns>
        public async Task LikeMedia(string mediaUrl)
        {
            var likeResponse = await InstaApi.MediaProcessor.LikeMediaAsync(GetIdFromUri(mediaUrl));
            Console.WriteLine(likeResponse.Succeeded
                ? $"Liked!: {likeResponse.Value}"
                : $"Error.Something goes wrong: {likeResponse.Info}");
        }
        
        public async Task FollowUser(string username)
        {
            var userInfo = await InstaApi.UserProcessor.GetUserInfoByUsernameAsync(username);
            if (userInfo.Succeeded)
            {
                var followUser = await InstaApi.UserProcessor.FollowUserAsync(userInfo.Value.Pk);
                Console.WriteLine(followUser.Succeeded
              ? $"Folloved!: {userInfo.Value.Username}"
              : $"Error.Something goes wrong: {followUser.Info}");
            }
        }
        public async Task UnFollowUser(string username)
        {
            var userInfo = await InstaApi.UserProcessor.GetUserInfoByUsernameAsync(username);
            if (userInfo.Succeeded)
            {
                var followUser = await InstaApi.UserProcessor.UnFollowUserAsync(userInfo.Value.Pk);
                Console.WriteLine(followUser.Succeeded
                ? $"Unfolloved: {userInfo.Value.Username}"
:                $"Error.Something goes wrong: {followUser.Info}");
            }
        }

        public async Task GetSelfFollowers (int pages)
        {
            var userFollovers = await InstaApi.UserProcessor.GetCurrentUserFollowersAsync(PaginationParameters.MaxPagesToLoad(pages));
        }
        public async Task GetuserFollowers(string username, int pages)
        {
                var follovers = await InstaApi.UserProcessor.GetUserFollowersAsync("argylefreeman", PaginationParameters.MaxPagesToLoad(pages));
        }

        public async Task ExploreLikeHashtag(string hashtag, int pages)
        {
            int counter = 0;
            var tagFeed = await InstaApi.FeedProcessor.GetTagFeedAsync(hashtag, PaginationParameters.MaxPagesToLoad(pages));
            if (tagFeed.Succeeded)
            {
                foreach (var media in tagFeed.Value.Medias)
                {
                    Console.WriteLine(media.Pk);
                    Console.WriteLine(media.Code);
                    Console.WriteLine(media.LikesCount);
                    Console.WriteLine(media.User);
                    Console.WriteLine("===================");
                    var likeResult = await InstaApi.MediaProcessor.LikeMediaAsync(media.InstaIdentifier);
                    var resultString = likeResult.Value ? "liked" : "not liked";
                    if (likeResult.Value)
                    {
                        counter++;
                    }
                    Console.WriteLine($"Media {media.Code} {resultString}");
                }
                Console.WriteLine("Liked: " + counter + " medias");
            }
        }

        public async Task DirectCheckMessages()
        {
            var inbox = await InstaApi.MessagingProcessor.GetDirectInboxAsync(PaginationParameters.MaxPagesToLoad(5));
            if (inbox.Value.Inbox.UnseenCount!=0)
            {
                Console.WriteLine("Unreaded messages: " + inbox.Value.Inbox.UnseenCount);
                Messages msg = new Messages(userSession.UserName);
                foreach (var thread in inbox.Value.Inbox.Threads)
                {
                    if (thread.HasUnreadMessage)
                    {
                        bool readed = false; 
                        var threads = await InstaApi.MessagingProcessor.GetDirectInboxThreadAsync(thread.ThreadId, PaginationParameters.MaxPagesToLoad(20));
                        foreach (var item in threads.Value.Items.AsEnumerable().Reverse())
                        {
                            Console.WriteLine(item.Text);
                                msg.StackMessage(thread.Title, thread.VieweId ,item.UserId.ToString(), item.ItemId, item.Text, item.TimeStamp);
                                if (!readed)
                                {
                                    var mark = await InstaApi.MessagingProcessor.MarkDirectThreadAsSeenAsync(thread.ThreadId, item.ItemId);
                                    readed = true;
                                }                      
                        }                       
                    }
                }
                msg.WriteMessages();
            }
            else
            {
                Console.WriteLine("There are no unreaded messages");
            }
        }

        public async Task DirectSendMessage(string username, string message)
        {
            try
            {
                var user = await InstaApi.UserProcessor.GetUserAsync(username);
                var userId = user.Value.Pk.ToString();
                var directText = await InstaApi.MessagingProcessor.SendDirectTextAsync(userId, null, message);
            }
            catch (Exception)
            {
                Console.WriteLine("Error while trying to send message!");
                throw;
            }
        }

        public async Task DirectAnswerMessage(string username, string message)
        {
            try
            {
                var inbox = await InstaApi.MessagingProcessor.GetDirectInboxAsync(PaginationParameters.MaxPagesToLoad(1));
                var desireThread = inbox.Value.Inbox.Threads
                    .Find(u => u.Users.FirstOrDefault().UserName.ToLower() == username);
                var requestedThreadId = desireThread.ThreadId;
                var directText = await InstaApi.MessagingProcessor.SendDirectTextAsync(null, requestedThreadId, message);

            }
            catch (Exception)
            {
                Console.WriteLine("Error while trying to send message!");
                throw;
            }         
        }

        private void RunChallenge()
        {
            Console.WriteLine("Starting chalange auth...");
            if (proxydata.host!=null)
            {
                Console.WriteLine("Using proxy...");
                var p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "ChallengeRequire.exe";
                p.StartInfo.Arguments = userSession.UserName +" " + userSession.Password +" "+ proxydata.host +" "+ proxydata.port +" "+ proxydata.username +" "+ proxydata.password;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
            }
            else
            {
                var p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "ChallengeRequire.exe";
                p.StartInfo.Arguments = userSession.UserName + " " + userSession.Password;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
            }
            Console.WriteLine("Press 1 if challenge is created");
            Console.WriteLine("Press 2 to exit");

            var key = Console.ReadLine();
            Console.WriteLine(Environment.NewLine);
            int answer = Convert.ToInt32(key);
            if (answer != 1)
            {
                Console.WriteLine("Error");
                return;
            }
        }

        private async Task GetChallenge()
        {
            var currentUser = await InstaApi.LoginAsync();
            if (currentUser.Succeeded)
            {
                SaveSession();
            }
            else
            {
                if (currentUser.Value == InstaLoginResult.ChallengeRequired)
                {
                    var challenge = await InstaApi.GetChallengeRequireVerifyMethodAsync();
                    if (challenge.Succeeded)
                    {
                        if (challenge.Value.SubmitPhoneRequired)
                        {
                            Console.WriteLine("Please type a valid phone number(with country code).\r\ni.e: +989123456789");
                            string phoneNumber = Console.ReadLine();
                            if (!phoneNumber.StartsWith("+"))
                                phoneNumber = $"+{phoneNumber}";
                            var submitPhone = await InstaApi.SubmitPhoneNumberForChallengeRequireAsync(phoneNumber);
                            if (submitPhone.Succeeded)
                            {
                                try
                                {
                                    var phoneResult = await InstaApi.RequestVerifyCodeToSMSForChallengeRequireAsync();
                                    if (phoneResult.Succeeded)
                                    {
                                        Console.WriteLine("We sent verify code to this phone number(it's end with this):\n" + phoneResult.Value.StepData.ContactPoint);
                                        await VerifyData();
                                    }
                                    else
                                        Console.WriteLine(phoneResult.Info.Message, "ERROR");
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Error while trying get challenge from phone number");
                                    throw;
                                }
                            }
                            else
                                Console.WriteLine("Error while trying get challenge from phone");
                        }
                        else
                        {
                            try
                            {
                                var email = await InstaApi.RequestVerifyCodeToEmailForChallengeRequireAsync();
                                if (email.Succeeded)
                                {
                                    Console.WriteLine($"We sent verify code to this email:\n{email.Value.StepData.ContactPoint}");
                                    await VerifyData();
                                }
                                else
                                    Console.WriteLine("Error while trying get challenge from email");
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Exception while trying get challenge from email");
                                throw;
                            }                                                 
                        }
                    }
                    else
                        Console.WriteLine("Error while trying get challenge");
                }
                else if (currentUser.Value == InstaLoginResult.TwoFactorRequired)
                {
                    await TwoFactorAuth();
                }
            }
        }

        private void LoadSession()
        {
            Console.WriteLine("Loading session..");
            InstaApi.SessionHandler.Load();
        }

        private void SaveSession()
        {
            if (InstaApi == null)
                return;
            if (!InstaApi.IsUserAuthenticated)
                return;
            if (!File.Exists(PathContract.pathAccount))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory + PathContract.pathAccount + userSession.UserName);
                dirInfo.Create();
            }
            InstaApi.SessionHandler.Save();
        }

        private async Task VerifyData()
        {
            Console.WriteLine("Please enter verification code: ");
            string code = Console.ReadLine();
            code = code.Trim();
            code = code.Replace(" ", "");
            var regex = new Regex(@"^-*[0-9,\.]+$");
            if (!regex.IsMatch(code))
            {
                Console.WriteLine("Verification code is numeric!!!", "ERROR");
                return;
            }
            if (code.Length != 6)
            {
                Console.WriteLine("Verification code must be 6 digits!!!", "ERROR");
                return;
            }
            try
            {
                var verifyLogin = await InstaApi.VerifyCodeForChallengeRequireAsync(code);
                if (verifyLogin.Succeeded)
                {
                    SaveSession();
                    Console.WriteLine("Connected!");
                }
                else
                {
                    if (verifyLogin.Value == InstaLoginResult.TwoFactorRequired)
                    {
                       await TwoFactorAuth();
                    }
                    else
                        Console.WriteLine(verifyLogin.Info.Message, "ERROR");
                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message, "Exception"); }
        }

        private async Task TwoFactorAuth()
        {
            if (InstaApi == null)
                return;

            Console.WriteLine("Runned two factor auth..");
            Console.WriteLine("Please type your two factor code and press enter");
            string authCode = Console.ReadLine();
            var twoFactorLogin = await InstaApi.TwoFactorLoginAsync(authCode);            // send two factor code
            Console.WriteLine(twoFactorLogin.Value);
            if (twoFactorLogin.Succeeded)
            {
                SaveSession();
            }
            else
            {
                Console.WriteLine("Eror while trying to get two factor auth");
            }
        }
    }
}
