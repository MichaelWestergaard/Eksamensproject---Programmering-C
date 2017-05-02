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
    /// Interaction logic for NewAccount.xaml
    /// </summary>
    public partial class NewAccount : Window
    {
        public NewAccount()
        {
            InitializeComponent();
        }

        private void create_account_button_Click(object sender, RoutedEventArgs e)
        {
            if(accountName.Text != "" && accountType.SelectedIndex > -1)
            {
                string account_name = accountName.Text;
                int account_type = accountType.SelectedIndex;

                if (Database.newAccount(User.id, account_name, account_type))
                {
                    this.Close();
                }
            }
        }
    }
}
