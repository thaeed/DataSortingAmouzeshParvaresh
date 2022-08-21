using DataSorting.WaitingWindow;
using Microsoft.Data.Sqlite;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace DataSorting
{
    /// <summary>
    /// Interaction logic for AddDataWindow.xaml
    /// </summary>
    public partial class AddDataWindow : Window
    {
        private DataDBContext _dbContext = new DataDBContext();
        public AddDataWindow()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.ValidateNames = true;
            openFileDialog.Filter = "csv files (*.csv)|*.csv;" ;

           
            var result = openFileDialog.ShowDialog();
            if (result ?? false)
            {
                txtPath.Text = openFileDialog.FileName;
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int duplicateCount = 0;
            int successCount = 0;
            try
            {
                if (string.IsNullOrEmpty(txtPath.Text))
                    return;

                btnBrowse.IsEnabled = false;
                btnUpdate.IsEnabled = false;

                List<PersonalInfoModel> list = new();
                var reader = new StreamReader(txtPath.Text);

                WaitBox.Show(this);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (values[0] == "firstname")
                        continue;

                    var personal = new PersonalInfoModel()
                    {
                        Firstname = values[0],
                        Lastname = values[1],
                        Fathername = values[2],
                        MeliCode = values[3],
                        PersonalCode = values[4]
                    };

                    list.Add(personal);
                }

                WaitBox.Close(this);

                pBar.Maximum = list.Count;
                pBar.Value = 0;

                foreach (PersonalInfoModel? value in list)
                {
                    pBar.Value++;
                    try
                    {
                        _dbContext.Add(value);
                       await _dbContext.SaveChangesAsync();
                        successCount++; 
                    }catch(SqliteException)
                    {
                       duplicateCount++; 
                    }
                }

                MessageBox.Show($"تعداد موفق: {successCount}\nایراد یا تکراری: {duplicateCount}");

                this.Close();

            }
            catch (Exception ex)
            {
                btnUpdate.IsEnabled = true;
                btnBrowse.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
        }
    }
}
