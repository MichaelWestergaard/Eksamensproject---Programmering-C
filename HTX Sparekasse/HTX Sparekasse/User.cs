using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTX_Sparekasse
{
    class User
    {
        public static int id;
        public static string fullname;
        public static string username;
        public static string password;
        public static int role;

        public bool login(string user, string pwd)
        {
            if(user == username && pwd == password)
            {
                return true;
            }
            return false;
        }
    }
}
