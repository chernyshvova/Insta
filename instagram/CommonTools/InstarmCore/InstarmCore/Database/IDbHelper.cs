
using System.Collections.Generic;

namespace InstarmCore
{
    interface IDbHelper
    {
        Profile GetProfileByName(string name);
        List<Profile> GetProfilesByTag(string tag);
        void writeMessages(List<Message> data);
    }
}
