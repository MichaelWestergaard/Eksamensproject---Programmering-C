using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Data;

namespace HTX_Sparekasse
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public static List<Account> items = new List<Account>();
        public List<Valuta> valuta = new List<Valuta>();
        public string convertFrom, convertTo;
        public double convertedValue;

        public UserWindow()
        {
            InitializeComponent();
            fullname.Content = User.fullname; //Set username
            date.Content = DateTime.Now.ToString(); //Set date  
            
            updateList();

            string jsonUSD = GET("http://api.fixer.io/latest?base=USD&symbols=DKK"); //API call for USD, EUR and GBP
            string jsonEUR = GET("http://api.fixer.io/latest?base=EUR&symbols=DKK");
            string jsonGDP = GET("http://api.fixer.io/latest?base=GBP&symbols=DKK");
            
            var objectUSD = JObject.Parse(jsonUSD); //Parse json string to JObject
            var objectEUR = JObject.Parse(jsonEUR);
            var objectGBP = JObject.Parse(jsonGDP);
            
            valuta.Add(new Valuta() { Valuta_name = "USD", Value = Convert.ToDouble(objectUSD["rates"]["DKK"])}); //Add valuta to the valuta list
            valuta.Add(new Valuta() { Valuta_name = "EUR", Value = Convert.ToDouble(objectEUR["rates"]["DKK"])});
            valuta.Add(new Valuta() { Valuta_name = "GBP", Value = Convert.ToDouble(objectGBP["rates"]["DKK"])});

            valuta_list.ItemsSource = valuta; //Insert valuta into listview

        }

        public void updateList()
        {
            account_list.ItemsSource = null;
            Database.getAccountsByUserID(User.id); //Get and set bank accounts for this user id
            account_list.ItemsSource = items; //Insert accounts into listview
        }

        public static string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

        private void amount_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (from_valuta.SelectedIndex > -1 && to_valuta.SelectedIndex > -1) //Check if the comboboxes is selected
            {
                updateValutaConverter(); //Convert valuta
            }
        }

        private void from_valuta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateValutaConverter(); //Convert valuta
        }
        
        private void to_valuta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateValutaConverter(); //Convert valuta
        }

        private void account_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            int list_index = account_list.SelectedIndex; //Get index of listview
            AccountOverview.account_id = items[list_index].Account_id; //Get and set account id
            AccountOverview.name = items[list_index].Account_name; //Get and set account name

            //Go to account overview
            var accountOverview = new AccountOverview();
            accountOverview.Show();

        }

        public void updateValutaConverter()
        {

            convertedValue = 0;
            //valuta converter

            if (amount_input.Text != "") //Check if the input field is not empty
            {
                if (from_valuta.SelectedIndex > -1 && to_valuta.SelectedIndex > -1) // Check if comboboxes is selected
                {
                    switch (from_valuta.SelectedIndex)
                    {
                        case 0:
                            convertFrom = "DKK";
                            break;

                        case 1:
                            convertFrom = "USD";
                            break;

                        case 2:
                            convertFrom = "EUR";
                            break;

                        case 3:
                            convertFrom = "GBP";
                            break;

                    }

                    switch (to_valuta.SelectedIndex)
                    {
                        case 0:
                            convertTo = "DKK";
                            break;

                        case 1:
                            convertTo = "USD";
                            break;

                        case 2:
                            convertTo = "EUR";
                            break;

                        case 3:
                            convertTo = "GBP";
                            break;

                    }

                    //If convertfrom and -to is the same, then set the converted value to amount_input
                    if (convertFrom == convertTo)
                    {
                        convertedValue = Convert.ToDouble(amount_input.Text);
                        converted_amount.Text = (convertedValue).ToString(); //Insert converted value into the textbox
                    }
                    else
                    {
                        string jsonCall = GET("http://api.fixer.io/latest?base=" + convertFrom + "&symbols=" + convertTo); // API call for valuta

                        var JSONObject = JObject.Parse(jsonCall); //Parse json string to JObject

                        convertedValue = Convert.ToDouble(JSONObject["rates"][convertTo]); //Save the value to convertedValue
                        converted_amount.Text = (convertedValue * Convert.ToDouble(amount_input.Text)).ToString(); //Insert convertedValue * amount_input into the textbox
                    }


                }
            }
        }
    }
}
