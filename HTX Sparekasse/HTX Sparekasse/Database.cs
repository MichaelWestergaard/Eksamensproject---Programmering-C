using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace HTX_Sparekasse
{
    class Database
    {
        public static string MySQLConnectionString = "Server=localhost;Database=htx_sparekasse;Uid=root;Pwd=;";

        public static MySqlConnection connection = new MySqlConnection(MySQLConnectionString);
        public static MySqlCommand cmd;

        public static void checkUser(string username, string password)
        {
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM users WHERE username = @username";
                cmd.Parameters.AddWithValue("@username", username);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    StringBuilder sb = new StringBuilder();
                    while (reader.Read())
                    {
                        User.id = reader.GetInt32("id");
                        User.username = reader.GetString("username");
                        User.password = reader.GetString("password");
                        User.role = reader.GetInt32("role");

                        Console.WriteLine(reader.GetString("id"));
                        Console.WriteLine(reader.GetString("username"));
                        Console.WriteLine(reader.GetString("password"));
                        Console.WriteLine(reader.GetString("role"));
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            connection.Close();
        }
        
        public static void getAccountsByUserID(int id)
        {
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM bank_accounts WHERE user_id = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    StringBuilder sb = new StringBuilder();
                    while (reader.Read())
                    {
                        UserWindow.items.Add(new Account() { Account_name = reader.GetString("name"), Last_transaction = "test", Money_amount = reader.GetDouble("amount") });
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            connection.Close();
        }
    }
}
