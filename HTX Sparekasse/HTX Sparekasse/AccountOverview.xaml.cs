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
        public List<Transaction> transactions = new List<Transaction>();

        public int account_id;
        public string name;
        public double amount;

        public AccountOverview()
        {
            InitializeComponent();
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

                    //Add the transaction to the database:
                    Database.newTransaction(0, User.id, 0, account_id, deposit_value);

                    transactions.Insert(0, new Transaction() { message = deposit_value + " blev indsat på kontoen. Du har nu " + (amount + deposit_value) + " på kontoen." });

                    transaction_list.ItemsSource = transactions;
                    transaction_list.Items.Refresh();

                    //Update account total.
                    amount += deposit_value;
                    money_amount.Content = "Saldo: " + amount + " kr.";
                    
                }
            }
        }

        private void withdraw_btn_Click(object sender, RoutedEventArgs e)
        {
            double withdraw_value;
            bool proceed = false;

            if (Double.TryParse(withdraw_amount.Text, out withdraw_value)) // if textbox has a double format
            {
                if (withdraw_value > 0) //Check if the amount is greater than 0
                {
                    //Check if account allows overdraft
                    if(withdraw_value > amount)
                    {
                        int type = Database.getAccountType(account_id);

                        if(type == 0) //Doesn't allow overdraft
                        {
                            proceed = false;
                        }
                        else if(type == 1 && withdraw_value <= 5000) //Type 1 allows overdraft, but only 5000 at the time
                        {
                            proceed = true;
                        }
                        else if(type == 2 && withdraw_value <= 100000) //Type 2 allows overdraft, but only 100000 at the time
                        {
                            proceed = true;
                        }
                    }
                    else
                    {
                        proceed = true;
                    }

                    if (proceed)
                    {
                        //Update the account in the database:
                        Database.updateAccount(account_id, withdraw_value * -1); //Make withdraw value negative
                        
                        //Add the transaction to the database:
                        Database.newTransaction(User.id, 0, account_id, 0, withdraw_value);
                        
                        transactions.Insert(0, new Transaction() { message = withdraw_value + " kr. blev hævet på kontoen. Du har nu " + (amount - withdraw_value) + " kr. på kontoen." }); //Insert because I want this on the op of the list

                        transaction_list.ItemsSource = transactions;
                        transaction_list.Items.Refresh();

                        //Update account total.
                        amount -= withdraw_value;
                        money_amount.Content = "Saldo: " + amount + " kr.";

                        //Updating userwindow
                    } else
                    {
                        transactions.Insert(0, new Transaction() { message = "Der blev forsøgt at hæve " + withdraw_value + " kr., men der var ikke nok penge på kontoen." });

                        transaction_list.ItemsSource = transactions;
                        transaction_list.Items.Refresh();
                    }
                }
            }
        }

        private void delete_account_Click(object sender, RoutedEventArgs e)
        {
            Database.deleteAccount(account_id);
            this.Close();
        }

        private void transaction_btn_Click(object sender, RoutedEventArgs e)
        {
            var transactionWindow = new TransactionWindow();

            transactionWindow.account_id = account_id;
            transactionWindow.amount = amount;

            transactionWindow.account_name.Content = name;
            transactionWindow.money_amount.Content = "Saldo: " + amount + " kr.";

            transactionWindow.Show();
            this.Close();
        }
    }
}
