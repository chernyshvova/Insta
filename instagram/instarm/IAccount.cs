using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instarm
{
    internal interface IAccount
    {
        Task SignIn(); 
        Task ChangeAvatar(string imgName); 
        // сделать что-то с путями файлов, это пиздец какой-то, запутаться можно
        Task SetPost(string imgName, string text);
        Task CommentMedia(string mediaId, string message);
        Task LikeMedia(string mediaId);
    }
}
