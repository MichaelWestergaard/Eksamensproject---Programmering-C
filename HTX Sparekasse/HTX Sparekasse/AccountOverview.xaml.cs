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
    /// Interaction logic for AccountOverview.xaml
    /// </summary>
    public partial class AccountOverview : Window
    {

        public static UserWindow userwindow = new UserWindow();

        public static int account_id;
        public static string name;

        public AccountOverview()
        {
            InitializeComponent();
            account_name.Content = name;
        }

        public void deposit_btn_Click(object sender, RoutedEventArgs e)
        {
            double deposit_value;
            if(Double.TryParse(deposit_amount.Text, out deposit_value)) // if textbox has a double format
            {
                if (deposit_value > 0) //Check if the amount is greater than 0
                {
                    //Update the account in the database:
                    Database.updateAccount(account_id, deposit_value);

                    //Updating userwindow
                    Database.getAccountsByUserID(User.id);
                    userwindow.updateList();
                }
            }
        }

        private void withdraw_btn_Click(object sender, RoutedEventArgs e)
        {
            double withdraw_value;
            if (Double.TryParse(widthdraw_amount.Text, out withdraw_value)) // if textbox has a double format
            {
                if (withdraw_value > 0) //Check if the amount is greater than 0
                {
                    //Update the account in the database:
                    Database.updateAccount(account_id, withdraw_value * -1); //Make withdraw value negative

                    //Updating userwindow
                    Database.getAccountsByUserID(User.id);
                    userwindow.updateList();
                }
            }
        }
    }
}
