using System.Threading.Tasks;

namespace instarm
{
    internal interface IAccount
    {
        Task SignIn(); 
        Task ChangeAvatar(string imgName); 
        Task SetPost(string imgName, string text);
        Task CommentMedia(string mediaId, string message);
        Task LikeMedia(string mediaId);
    }
}
