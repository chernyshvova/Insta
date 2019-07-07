using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instarm
{
    interface IDbHelper
    {
        Profile GetProfileByName(string name);
        List<Profile> GetProfilesByTag(string tag);
    }
}
