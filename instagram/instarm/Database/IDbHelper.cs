
using System.Collections.Generic;

namespace instarm
{
    interface IDbHelper
    {
        Profile GetProfileByName(string name);
        List<Profile> GetProfilesByTag(string tag);
    }
}
