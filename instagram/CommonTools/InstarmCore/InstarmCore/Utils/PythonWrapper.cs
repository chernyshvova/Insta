using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstarmCore.Utils;

namespace InstarmCore
{
    class PythonWrapper : DynamicObject
    {

        public int getAccount(string accountName, out string result)
        {
            result = "";
            AccountManager mg = new AccountManager();
            Account acc = mg.getAccount(accountName);
            result = ExeptionUtils.ErrorMessage;
            return (int)ExeptionUtils.currentState;
        }

    }
}
