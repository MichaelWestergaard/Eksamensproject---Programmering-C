using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTX_Sparekasse
{
    public class Account
    {
        public int Account_id { get; set; }
        public string Account_name { get; set; }
        public string Last_transaction { get; set; }
        public double Money_amount { get; set; }
    }
}
