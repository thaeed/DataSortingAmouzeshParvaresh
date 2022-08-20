using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for FindingFilesWindow.xaml
    /// </summary>
    public partial class FindingFilesWindow : Window
    {
        private DataDBContext _dbContex = new DataDBContext();
        public FindingFilesWindow()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;

            openFileDialog.FileName = "select";
            var result = openFileDialog.ShowDialog();
            if (result ?? false)
            {
                txtPath.Text = openFileDialog.FileName.Replace(@"\select", "");
            }


        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtPath.Text))
                    return;

                var dirs = Directory.GetDirectories(txtPath.Text);

                if (dirs.Length > 0)
                    pBar.Maximum = dirs.Length;

                pBar.Value = 0;

                btnBrowse.IsEnabled = false;
                btnUpdate.IsEnabled = false;
                foreach (var dir in dirs)
                {
                    
                    pBar.Value++;
                    var emp = await _dbContex.Employees.Where(e => e.PersonalCode == System.IO.Path.GetFileName(dir)).FirstOrDefaultAsync();
                    
                    if (emp == null)
                        continue;

                    emp.DirPath = dir;
                    emp.Created = Directory.GetCreationTime(dir);
                    emp.Updated = Directory.GetLastWriteTime(dir);
                    emp.FileCount = Directory.GetFiles(dir).Length;

                    _dbContex.Update(emp);
                    await _dbContex.SaveChangesAsync();


                }

                MessageBox.Show("عملیات تمام شد.");

                this.DialogResult = true;
                this.Close();

                btnBrowse.IsEnabled = true;
                btnUpdate.IsEnabled = true;

            }
            catch (Exception ex)
            {
                btnBrowse.IsEnabled = true;
                btnUpdate.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }

        }
    }
}
