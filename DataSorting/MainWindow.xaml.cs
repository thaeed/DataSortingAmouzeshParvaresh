using System;
using System.Collections.Generic;
using System.IO;
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

namespace DataSorting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataDBContext _dbContext = new DataDBContext();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // this is for demo purposes only, to make it easier
            // to get up and running
            _dbContext.Database.EnsureCreated();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<PersonalInfoModel> list = new();
            var reader = new StreamReader(@"D:\test3.csv");

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                var personal = new PersonalInfoModel()
                {
                    Firstname = values[0],
                    Lastname = values[1],
                    Fathername = values[2],
                    MeliCode = values[3],
                    PersonalCode = Convert.ToInt32(values[4])
                };
            }


            foreach (var value in listA)
            {
                MessageBox.Show(value.ToString());
            }

        }
    }
}
