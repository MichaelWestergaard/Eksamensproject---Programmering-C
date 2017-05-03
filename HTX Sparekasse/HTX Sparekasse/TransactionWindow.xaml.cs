using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HTX_Sparekasse
{
    /// <summary>
    /// Interaction logic for TransactionWindow.xaml
    /// </summary>
    public partial class TransactionWindow : Window
    {
        public int account_id;
        public double amount;

        public TransactionWindow()
        {
            InitializeComponent();
        }

        private void make_transaction_button_Click(object sender, RoutedEventArgs e)
        {
            double money_amount;
            int id;
            if (amount_money_transaction.Text != "" && send_to_id.Text != "")
            {
                if (Double.TryParse(amount_money_transaction.Text, out money_amount) && Int32.TryParse(send_to_id.Text, out id))
                {
                    if (money_amount <= amount)
                    {
                        Database.updateAccount(account_id, -money_amount);
                        Database.updateAccount(id, money_amount);
                        Database.newTransaction(User.id, 0, account_id, id, money_amount);
                        this.Close();
                    }
                }
            }
                
        }
    }
}
