using DataSorting.WaitingWindow;
using Microsoft.EntityFrameworkCore;
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
        private PersonalInfoModel searchModel = new PersonalInfoModel();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            searchDG.KeyDown += SearchDG_KeyDown;
            this.KeyDown += SearchDG_KeyDown;
            // this is for demo purposes only, to make it easier
            // to get up and running
            _dbContext.Database.EnsureCreated();

            var dump = new List<PersonalInfoModel>();

            dump.Add(new PersonalInfoModel());
            searchDG.ItemsSource = dump;

            WaitBox.Show(this);
            var dbResult = await _dbContext.Employees.ToListAsync();
            databaseDG.ItemsSource = dbResult;
            WaitBox.Close(this);
        }

        private void SearchDG_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Insert)
            {
                searchDG.DataContext = null;
                var dump = new List<PersonalInfoModel>();

                dump.Add(new PersonalInfoModel());
                searchDG.ItemsSource = dump;
                searchDG.CurrentCell = new DataGridCellInfo(
                searchDG.Items[0], searchDG.Columns[0]);
                searchDG.BeginEdit();

                searchModel = new PersonalInfoModel();
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<PersonalInfoModel> list = new();
            var reader = new StreamReader(@"D:\test.csv");

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
                    PersonalCode = values[4],
                    Created = DateTime.Now
                };

                list.Add(personal);
            }


            foreach (PersonalInfoModel? value in list)
            {

                _dbContext.Add(value);
                _dbContext.SaveChanges();
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //for (int i = 12000000; i <= 12000600; i++)
            //{
            //    Directory.CreateDirectory(@"E:\TESTAP\" + i.ToString());
            //}

            var files = Directory.GetDirectories(@"E:\TESTAP");

            MessageBox.Show(System.IO.Path.GetFileName(files[0]));
            foreach (var file in files)
            {
                var result = _dbContext.Employees.Where(e => e.PersonalCode.ToString() == System.IO.Path.GetFileName(file)).FirstOrDefault();
                if (result != null)
                    MessageBox.Show(result.Firstname + result.Lastname);
            }


        }



        private async void FirstnameChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            searchModel.Firstname = tb.Text;
            var result = await _dbContext.Employees.Where(e => e.Firstname.Contains(searchModel.Firstname) || e.Lastname.Contains(searchModel.Lastname)
                  || e.Fathername.Contains(searchModel.Fathername) || e.MeliCode.StartsWith(searchModel.MeliCode) || e.PersonalCode.StartsWith(searchModel.PersonalCode)).ToListAsync();
            databaseDG.ItemsSource = result;
        }

        private async void LastnameChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            searchModel.Lastname = tb.Text;
            var result = await _dbContext.Employees.Where(e => e.Firstname.Contains(searchModel.Firstname) || e.Lastname.Contains(searchModel.Lastname)
                     || e.Fathername.Contains(searchModel.Fathername) || e.MeliCode.StartsWith(searchModel.MeliCode) || e.PersonalCode.StartsWith(searchModel.PersonalCode)).ToListAsync();
            databaseDG.ItemsSource = result;
        }

        private async void FathernameChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            searchModel.Fathername = tb.Text;
            var result = await _dbContext.Employees.Where(e => e.Firstname.Contains(searchModel.Firstname) || e.Lastname.Contains(searchModel.Lastname)
                   || e.Fathername.Contains(searchModel.Fathername) || e.MeliCode.StartsWith(searchModel.MeliCode) || e.PersonalCode.StartsWith(searchModel.PersonalCode)).ToListAsync();
            databaseDG.ItemsSource = result;
        }

        private async void MeliCodeChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            searchModel.Firstname = tb.Text;
            var result = await _dbContext.Employees.Where(e => e.Firstname.Contains(searchModel.Firstname) || e.Lastname.Contains(searchModel.Lastname)
                    || e.Fathername.Contains(searchModel.Fathername) || e.MeliCode.StartsWith(searchModel.MeliCode) || e.PersonalCode.StartsWith(searchModel.PersonalCode)).ToListAsync();
            databaseDG.ItemsSource = result;
        }

        private async void PersonalCodeChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            searchModel.PersonalCode = tb.Text;
            var result = await _dbContext.Employees.Where(e => e.Firstname.Contains(searchModel.Firstname) || e.Lastname.Contains(searchModel.Lastname)
                     || e.Fathername.Contains(searchModel.Fathername) || e.MeliCode.StartsWith(searchModel.MeliCode) || e.PersonalCode.StartsWith(searchModel.PersonalCode)).ToListAsync();
            databaseDG.ItemsSource = result;
        }


        private void btnSyncFiles_Click(object sender, RoutedEventArgs e)
        {
            FindingFilesWindow findingFilesWindow = new();

            findingFilesWindow.ShowDialog();

            Window_Loaded(null, null);
        }

        private void btnAddData_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataWindow = new();

            addDataWindow.ShowDialog();

            Window_Loaded(null, null);
        }
    }
}
