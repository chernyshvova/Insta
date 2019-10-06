using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstarmCore;

namespace InstarmCore.Utils
{
    public class PythonWrapper : DynamicObject
    {

        public int getAccount(string accountName, out string result)
        {
            result = "";
            AccountManager mg = new AccountManager();
            try
            {
                Account acc = mg.getAccount(accountName);
            }
            catch (Exception)
            {
                result = ExeptionUtils.ErrorMessage;
                return (int)ExeptionUtils.currentState;
            }
           
            result = ExeptionUtils.ErrorMessage;
            return (int)ExeptionUtils.currentState;
        }

    }
}
