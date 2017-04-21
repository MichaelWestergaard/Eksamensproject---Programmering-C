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
    /// Interaction logic for CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Window
    {

        public static string errorMessage;

        public CreateUser()
        {
            InitializeComponent();
        }

        private void create_user_btn_Click(object sender, RoutedEventArgs e)
        {

            //Check if textboxes is empty. If empty write an error message.
            if(fullname.Text != "")
            {
                if(username.Text != "")
                {
                    if(password.Text != "")
                    {
                        if(account_name.Text != "")
                        {
                            //Everything is filled out
                            //Next step: Check if the username is already created.
                            if (!Database.checkUsername(username.Text))
                            {
                                //Username is not found in the database, now we can insert the new user.
                                Database.newUser(fullname.Text, username.Text, password.Text, account_name.Text);

                                //Return to login window
                                var newForm = new MainWindow();
                                newForm.Show();
                                this.Close();
                            }
                            
                        }
                        else
                        {
                            errorMessage = "Du SKAL angive et konto navn!";
                        }
                    }
                    else
                    {
                        errorMessage = "Du SKAL angive en adgangskode!";
                    }
                }
                else
                {
                    errorMessage = "Du SKAL angive et brugernavn!";
                }
            }
            else
            {
                errorMessage = "Du SKAL udfylde navnefeltet!";
            }

            //Display error message
            if(errorMessage != "")
            {
                error_message_label.Visibility = Visibility.Visible; // Show label
                error_message_label.Content = errorMessage; //Change label text to the error message
            }
        }

        private void focus(object sender, RoutedEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            textbox.Text = "";
            textbox.GotFocus -= focus;
        }
    }
}
