using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTX_Sparekasse
{
    class UserHandler
    {
        public bool login(string username, string password)
        {
            if (username == "admin" && password == "123")
            {
                return true;
            }
            else if (username == "user" && password == "123")
            {
                return true;
            }
            return false;
        }
    }
}
