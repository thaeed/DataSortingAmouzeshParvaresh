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
    //By Saeed Darandi© @thaeed on Socials..
    //Simple app for managing scaning proccess and stuff.. Summer 2022
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

        private void databaseDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            imageGrid.Children.Clear();

            /*
             * Image container has 2 column and 3 rows.. (6 pictures)
             */

            ScrollViewer sc = new ScrollViewer();

            var containerGrid = new Grid();

            ColumnDefinition cd0 = new ColumnDefinition();
            cd0.Width = new GridLength(100, GridUnitType.Star);
            containerGrid.ColumnDefinitions.Add(cd0);
            ColumnDefinition cd1 = new ColumnDefinition();
            cd1.Width = new GridLength(100, GridUnitType.Star);
            containerGrid.ColumnDefinitions.Add(cd1);


            RowDefinition row0 = new RowDefinition();
            row0.Height = new GridLength(400, GridUnitType.Pixel);
            containerGrid.RowDefinitions.Add(row0);
            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(400, GridUnitType.Pixel);
            containerGrid.RowDefinitions.Add(row1);
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(400, GridUnitType.Pixel);
            containerGrid.RowDefinitions.Add(row2);


            int containerCounter = 1;

            imageGrid.Children.Add(sc);
            if (((PersonalInfoModel)databaseDG.SelectedItem) == null)
                return;

            var info = (PersonalInfoModel)databaseDG.SelectedItem as PersonalInfoModel;

            if (info != null)
            {
                var files = Directory.GetFiles(info.DirPath);

                if (files.Length > 0)
                {

                    foreach (var file in files)
                    {

                        var image = new Image();
                        
                        image.Stretch = Stretch.Uniform;
                        image.Source = new BitmapImage(new Uri(file));

                        switch (containerCounter)
                        {
                            case 1:
                                image.SetValue(Grid.ColumnProperty, 0);
                                image.SetValue(Grid.RowProperty, 0);

                                break;
                            case 2:
                                image.SetValue(Grid.ColumnProperty, 1);
                                image.SetValue(Grid.RowProperty, 0);
                                break;
                            case 3:
                                image.SetValue(Grid.ColumnProperty, 0);
                                image.SetValue(Grid.RowProperty, 1);
                                break;
                            case 4:
                                image.SetValue(Grid.ColumnProperty, 1);
                                image.SetValue(Grid.RowProperty, 1);
                                break;
                            case 5:
                                image.SetValue(Grid.ColumnProperty, 0);
                                image.SetValue(Grid.RowProperty, 2);
                                break;
                            case 6:
                                image.SetValue(Grid.ColumnProperty, 1);
                                image.SetValue(Grid.RowProperty, 2);
                                break;
                        }
                        containerCounter++; 

                        containerGrid.Children.Add(image);
                    }

                   

                    sc.Content = containerGrid;
                }
            }
        }
    }
}
