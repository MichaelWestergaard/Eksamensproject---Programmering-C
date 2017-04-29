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

        public static double amount;
        public static int from_user_id;
        public static int to_user_id;
        public static int from_account_id;
        public static int to_account_id;

        public static string getLastTransaction(int accountID)
        {

            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM transactions WHERE from_account_id = @accountID OR to_account_id = @accountID LIMIT 1";
                cmd.Parameters.AddWithValue("@accountID", accountID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        connection.Close();
                        while (reader.Read())
                        {
                            amount = reader.GetDouble("amount");
                            to_user_id = reader.GetInt32("to_user_id");
                            from_user_id = reader.GetInt32("from_user_id");
                            to_account_id = reader.GetInt32("to_account_id");
                            from_account_id = reader.GetInt32("from_account_id");
                            return amount + " kr. blev sendt fra " + from_user_id + " fra konto " + from_account_id + " til " + to_user_id;
                        }
                    }
                    return "Ingen aktiviter..";
                }

            }
            catch (Exception)
            {
                throw;
            }


        }

        public static void getUser(string username, string password)
        {
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM users WHERE username = @username";
                cmd.Parameters.AddWithValue("@username", username);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User.id = reader.GetInt32("id");
                        User.fullname = reader.GetString("fullname");
                        User.username = reader.GetString("username");
                        User.password = reader.GetString("password");
                        User.role = reader.GetInt32("role");
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            connection.Close();
        }

        public static bool checkUsername(string username)
        {
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM users WHERE username = @username";
                cmd.Parameters.AddWithValue("@username", username);

                int result = cmd.ExecuteNonQuery(); //Check user in database table users

                connection.Close();

                if (result == 0)
                {
                    //Database contains an user with the username
                    return true;
                }
                else
                {
                    //Database does not contain an user with the given username
                    return false;
                }

            }
            catch (Exception)
            {
                throw;
            }

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
                    while (reader.Read())
                    {
                        //UserWindow.addToList(reader.GetInt32("id"), reader.GetString("name"), reader.GetInt32("id").ToString(), reader.GetDouble("amount"));
                        UserWindow.items.Add(new Account() { Account_id = reader.GetInt32("id"), Account_name = reader.GetString("name"), Last_transaction = reader.GetInt32("id").ToString(), Money_amount = reader.GetDouble("amount") });
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            connection.Close();
        }

        public static int getAccountType(int account_id)
        {
            int type = 0;
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT type FROM bank_accounts WHERE id = @account_id";
                cmd.Parameters.AddWithValue("@account_id", account_id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        type = reader.GetInt32("type");
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return type;
            connection.Close();
        }

        public static void getTransactions(int id, int account_id)
        {
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM transactions WHERE from_user_id = @id OR to_user_id = @id AND from_account_id = @account_id OR to_account_id = @account_id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@account_id", account_id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //UserWindow.addToList(reader.GetInt32("id"), reader.GetString("name"), reader.GetInt32("id").ToString(), reader.GetDouble("amount"));
                        UserWindow.items.Add(new Account() { Account_id = reader.GetInt32("id"), Account_name = reader.GetString("name"), Last_transaction = reader.GetInt32("id").ToString(), Money_amount = reader.GetDouble("amount") });
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            connection.Close();
        }

        public static void newUser(string fullname, string username, string password, string account_name)
        {
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO users(fullname, username, password) VALUES (@fullname, @username, @password)";
                cmd.Parameters.AddWithValue("@fullname", fullname);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {
                throw;
            }
            connection.Close();
        }

        public static void newTransaction(int from_user_id, int to_user_id, int from_account_id, int to_account_id, double amount)
        {
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO transactions(from_user_id, to_user_id, from_account_id, to_account_id, amount) VALUES (@from_user_id, @to_user_id, @from_account_id, @to_account_id, @amount)";
                cmd.Parameters.AddWithValue("@from_user_id", from_user_id);
                cmd.Parameters.AddWithValue("@to_user_id", to_user_id);
                cmd.Parameters.AddWithValue("@from_account_id", from_account_id);
                cmd.Parameters.AddWithValue("@to_account_id", to_account_id);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {
                throw;
            }
            connection.Close();
        }

        public static bool updateAccount(int account_id, double value)
        {
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE bank_accounts SET amount = amount + @value WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", account_id);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {
                throw;
            }
            connection.Close();
            return true;
        }

    }
}
