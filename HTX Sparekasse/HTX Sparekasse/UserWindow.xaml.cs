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

namespace HTX_Sparekasse
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public static List<Account> items = new List<Account>();
        public static List<Valuta> valuta = new List<Valuta>();

        public UserWindow()
        {
            InitializeComponent();
            fullname.Content = User.fullname; //Set username
            date.Content = DateTime.Now.ToString(); //Set date  
            
            Database.getAccountsByUserID(User.id); //Set bank accounts for this user id

            account_list.ItemsSource = items; //Insert accounts into listview

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


            //valuta converter
            string convertFrom;

            if(amount_input.Text != "") //Check if the input field is not empty
            {
                if(from_valuta.SelectedIndex > -1 && to_valuta.SelectedIndex > -1) // Check if comboboxes is selected
                {
                    switch (from_valuta.SelectedIndex)
                    {
                        case 0:
                            convertFrom = "DKK";
                            break;

                    }
                }
            }

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

    }
}
