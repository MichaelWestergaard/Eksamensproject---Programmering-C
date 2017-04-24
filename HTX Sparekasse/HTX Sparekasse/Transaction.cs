using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTX_Sparekasse
{
    public class Transaction
    {
        public int from_user_id { get; set; }
        public int to_user_id { get; set; }
        public int from_account_id { get; set; }
        public int to_account_id { get; set; }
        public double amount { get; set; }
    }
}
