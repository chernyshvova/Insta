using System;
using System.Threading.Tasks;
using System.IO;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;
using System.Net.Http;

namespace instarm
{
    internal class Account : IAccount
    {
        private readonly IInstaApi InstaApi;
        private readonly UserSessionData userSession;
        private ProxyData proxydata = new ProxyData(null, null,null,null);
        const string stateFile = @"\state.bin";
        private const string pathAccount = @"\accounts\";
        private const string pathImg = @"\images\";
        private const string pathAvatar = @"\avatars\";
        public Account(UserSessionData userSession)
        {
            this.userSession = userSession;
            InstaApi = InstaApiBuilder.CreateBuilder()
            .SetUser(userSession)
            .UseLogger(new DebugLogger(LogLevel.All)) // use logger for requests and debug messages
            .SetRequestDelay(RequestDelay.FromSeconds(2, 2))  //отсрочка запросов
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
            var relatedPath = Environment.CurrentDirectory + pathAccount + userSession.UserName + stateFile;
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
                    RunChallenge();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            if (!InstaApi.IsUserAuthenticated)
            {
                Console.WriteLine($"Logging in as {userSession.UserName}");
                delay.Disable();
                var logInResult = await InstaApi.LoginAsync();
                delay.Enable();
                if (!logInResult.Succeeded)
                {
                    Console.WriteLine($"Unable to login: {logInResult.Info.Message}");
                    return;
                }
            }
            var currentUser = await InstaApi.GetCurrentUserAsync();
            Console.WriteLine(
                $"Logged in: username - {currentUser.Value.UserName}, full name - {currentUser.Value.FullName}");
        }

        /// <summary>
        /// Смена аватарки профиля
        /// </summary>
        /// <returns></returns>
        public async Task ChangeAvatar(string imgName)
        {
            var picturePath = Environment.CurrentDirectory + pathAvatar + imgName;
                                                                    // note: only JPG and JPEG format will accept it in instagram!
            if (File.Exists(picturePath))
            {
                try
                {
                    var pictureBytes = File.ReadAllBytes(picturePath);
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
                DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory + pathAvatar);
                dirInfo.Create();
                Console.WriteLine("File not found : " + picturePath);
                Console.WriteLine("Add image into next directory: " + Environment.CurrentDirectory + pathAvatar);
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
            var relatedPath = Environment.CurrentDirectory + pathImg + imgName;

            if (File.Exists(relatedPath))
            {
                Console.WriteLine("Loading..");
                var mediaImage = new InstaImageUpload
                {
                    // leave zero, if you don't know how height and width is it.
                    Height = 0,
                    Width = 0,
                    Uri = relatedPath
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
                DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory + pathImg );
                dirInfo.Create();
                Console.WriteLine("File not found : " + relatedPath);
                Console.WriteLine("Add image into next directory: " + Environment.CurrentDirectory + pathImg);
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
    }
}
