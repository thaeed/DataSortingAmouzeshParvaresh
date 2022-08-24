using DataSorting.WaitingWindow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            this.KeyDown += ResetFilterKeyDown;

            txtFirstname.PreviewKeyDown += ResetFilterKeyDown;
            txtLastname.PreviewKeyDown += ResetFilterKeyDown;
            txtFathername.PreviewKeyDown += ResetFilterKeyDown;
            txtMeliCode.PreviewKeyDown += ResetFilterKeyDown;
            txtPersonalCode.PreviewKeyDown += ResetFilterKeyDown;

            _dbContext.Database.EnsureCreated();


            WaitBox.Show(this);
            var dbResult = await _dbContext.Employees.OrderBy(e => e.Lastname).ToListAsync();
            databaseDG.ItemsSource = dbResult;
            WaitBox.Close(this);
        }

        private async void ResetFilterKeyDown(object sender, KeyEventArgs e)
        {


            if (e.Key == Key.Delete)
            {
                imageGrid.Children.Clear();

                txtFirstname.Text = txtLastname.Text = txtFathername.Text = txtMeliCode.Text = txtPersonalCode.Text = string.Empty;


                txtFirstname.Focus();


                txtPath.Text = string.Empty;

                WaitBox.Show(this);
                var dbResult = await _dbContext.Employees.OrderBy(e => e.Lastname).ToListAsync();
                databaseDG.ItemsSource = dbResult;
                WaitBox.Close(this);
            }
        }



        private async void btnSyncFiles_Click(object sender, RoutedEventArgs e)
        {
            FindingFilesWindow findingFilesWindow = new()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            findingFilesWindow.ShowDialog();



            WaitBox.Show(this);
            var dbResult = await _dbContext.Employees.OrderBy(e => e.Lastname).ToListAsync();
            databaseDG.ItemsSource = dbResult;
            WaitBox.Close(this);
        }

        private async void btnAddData_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataWindow = new()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            addDataWindow.ShowDialog();



            WaitBox.Show(this);
            var dbResult = await _dbContext.Employees.OrderBy(e => e.Lastname).ToListAsync();
            databaseDG.ItemsSource = dbResult;
            WaitBox.Close(this);
        }

        private void databaseDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            imageGrid.Children.Clear();
            txtPath.Text = string.Empty;
            if (((PersonalInfoModel)databaseDG.SelectedItem) == null)
                return;

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


            var info = (PersonalInfoModel)databaseDG.SelectedItem as PersonalInfoModel;
            txtPath.Text = info.DirPath;



            if (info != null)
            {
                if (!Directory.Exists(info.DirPath))
                    return;
                var files = Directory.GetFiles(info.DirPath);

                if (files.Length > 0)
                {

                    foreach (var file in files)
                    {
                        try
                        {
                            var image = new Image();

                            image.Stretch = Stretch.Uniform;
                            image.Source = new BitmapImage(new Uri(file));

                            image.MouseDown += delegate
                            {
                                Window win = new Window();
                                win.Height = 500;
                                win.Width = 500;
                                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                                Grid winGrid = new Grid();
                                winGrid.VerticalAlignment = VerticalAlignment.Center;
                                winGrid.HorizontalAlignment = HorizontalAlignment.Center;

                                RowDefinition winGridRow = new RowDefinition();
                                winGridRow.Height = new GridLength(100, GridUnitType.Star);

                                winGrid.RowDefinitions.Add(winGridRow);

                                Image winImage = new Image();
                                winImage.Source = new BitmapImage(new Uri(file));
                                winImage.Stretch = Stretch.Fill;


                                winGrid.Children.Add(winImage);


                                win.Content = winGrid;

                                win.Title = info.Firstname + " " + info.Lastname + " " + info.PersonalCode;
                                win.ShowDialog();




                            };

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




                            sc.Content = containerGrid;
                        }
                        catch { }
                    }
                }
            }
        }


        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPath.Text))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = txtPath.Text,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
        }

        private async void txtPath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPath.Text))
            {
                Clipboard.SetText(txtPath.Text);

                Label lbl = new()
                {
                    Foreground = Brushes.PaleVioletRed,
                    FlowDirection = FlowDirection.RightToLeft,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                lbl.Content = "آدرس کپی شد.";

                copyInfoPanel.Children.Add(lbl);


                await Task.Delay(1000);
                copyInfoPanel.Children.Remove(lbl);

            }
        }

        private async void btnIncomplete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileDialog = new SaveFileDialog
                {
                    Filter = "CSV file |*.csv",
                    Title = "ذخیره اطلاعات نواقص",
                    AddExtension = true,
                    DefaultExt = "csv"
                };
                var fileResult = fileDialog.ShowDialog();

                if (fileResult == false)
                    return;




                var incompletes = await _dbContext.Employees.Where(e => e.FileCount == 0).OrderBy(p => p.Lastname).ToListAsync();


                if (incompletes.Count == 0)
                {
                    MessageBox.Show("نواقصی موجود نمیباشد.");
                    return;
                }

                var write = new StreamWriter(fileDialog.FileName, false, Encoding.UTF8);


                var exportStringHeader = string.Format("{0},{1},{2},{3},{4}"
                    , "نام", "نام خانوادگی", "نام پدر", "کد ملی", "کد پرسنلی");
                await write.WriteLineAsync(exportStringHeader);
                await write.FlushAsync();

                foreach (var incomplete in incompletes)
                {
                    var exportStringLine = string.Format("{0},{1},{2},{3},{4}",
                        incomplete.Firstname,
                        incomplete.Lastname,
                        incomplete.Fathername,
                        incomplete.MeliCode,
                        incomplete.PersonalCode);

                    await write.WriteLineAsync(exportStringLine);
                    await write.FlushAsync();
                }

                write.Close();

                MessageBox.Show("اطلاعات با موفقیت ذخیره شد.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private async void txtFirstname_TextChanged(object sender, TextChangedEventArgs e)
        {
            var firstname = txtFirstname.Text.Replace("ی", "ي");
            var lastname = txtLastname.Text.Replace("ی", "ي");
            var fathername = txtFathername.Text.Replace("ی", "ي");

            var result = await _dbContext.Employees.Where(e => e.Firstname.StartsWith(firstname))
                .Where(e => e.Lastname.StartsWith(lastname))
                .Where(e => e.Fathername.StartsWith(fathername))
                .Where(e => e.MeliCode.StartsWith(txtMeliCode.Text))
                .Where(e => e.PersonalCode.StartsWith(txtPersonalCode.Text))
                .OrderBy(e => e.Firstname)
                .ToListAsync();



            databaseDG.ItemsSource = result;
        }

        private async void txtLastname_TextChanged(object sender, TextChangedEventArgs e)
        {
            var firstname = txtFirstname.Text.Replace("ی", "ي");
            var lastname = txtLastname.Text.Replace("ی", "ي");
            var fathername = txtFathername.Text.Replace("ی", "ي");

            var result = await _dbContext.Employees.Where(e => e.Lastname.StartsWith(lastname))
                  .Where(e => e.Firstname.StartsWith(firstname))
                  .Where(e => e.Fathername.StartsWith(fathername))
                  .Where(e => e.MeliCode.StartsWith(txtMeliCode.Text))
                  .Where(e => e.PersonalCode.StartsWith(txtPersonalCode.Text))
                  .OrderBy(e => e.Firstname)
                  .ToListAsync();




            databaseDG.ItemsSource = result;
        }

        private async void txtFathername_TextChanged(object sender, TextChangedEventArgs e)
        {
            var firstname = txtFirstname.Text.Replace("ی", "ي");
            var lastname = txtLastname.Text.Replace("ی", "ي");
            var fathername = txtFathername.Text.Replace("ی", "ي");

            var result = await _dbContext.Employees.Where(e => e.Fathername.StartsWith(fathername))
                  .Where(e => e.Firstname.StartsWith(firstname))
                  .Where(e => e.Lastname.StartsWith(lastname))
                  .Where(e => e.MeliCode.StartsWith(txtMeliCode.Text))
                  .Where(e => e.PersonalCode.StartsWith(txtPersonalCode.Text))
                  .OrderBy(e => e.Firstname)
                  .ToListAsync();




            databaseDG.ItemsSource = result;
        }

        private async void txtMeliCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            var firstname = txtFirstname.Text.Replace("ی", "ي");
            var lastname = txtLastname.Text.Replace("ی", "ي");
            var fathername = txtFathername.Text.Replace("ی", "ي");

            var result = await _dbContext.Employees.Where(e => e.MeliCode.StartsWith(txtMeliCode.Text))
                  .Where(e => e.Firstname.StartsWith(firstname))
                  .Where(e => e.Lastname.StartsWith(lastname))
                  .Where(e => e.Fathername.StartsWith(fathername))
                  .Where(e => e.PersonalCode.StartsWith(txtPersonalCode.Text))
                  .OrderBy(e => e.Firstname)
                  .ToListAsync();




            databaseDG.ItemsSource = result;
        }

        private async void txtPersonalCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            var firstname = txtFirstname.Text.Replace("ی", "ي");
            var lastname = txtLastname.Text.Replace("ی", "ي");
            var fathername = txtFathername.Text.Replace("ی", "ي");

            var result = await _dbContext.Employees.Where(e => e.PersonalCode.StartsWith(txtPersonalCode.Text))
                  .Where(e => e.Firstname.StartsWith(firstname))
                  .Where(e => e.Lastname.StartsWith(lastname))
                  .Where(e => e.Fathername.StartsWith(fathername))
                  .Where(e => e.MeliCode.StartsWith(txtMeliCode.Text))
                  .OrderBy(e => e.Firstname)
                  .ToListAsync();




            databaseDG.ItemsSource = result;
        }
    }
}
