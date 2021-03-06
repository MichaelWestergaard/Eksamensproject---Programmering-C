﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HTX_Sparekasse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User user = new User();
        private static UserWindow userwindow = new UserWindow();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Database.getUser(usernameField.Text, passwordField.Password);

            if (usernameField.Text != "")
            {
                if (passwordField.Password != "")
                {
                    if (user.login(usernameField.Text, passwordField.Password))
                    {
                        if (usernameField.Text == "admin")
                        {
                            var newForm = new AdminWindow();
                            newForm.Show();
                            this.Close();
                        }
                        else
                        {
                            var newForm = new UserWindow();
                            newForm.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        //Wrong username or password
                    }
                }
                else
                {
                    //You need password
                }
            }
            else
            {
                //You need username
            }


        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            var newForm = new CreateUser();
            newForm.Show();
        }
    }
}