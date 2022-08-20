using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;

namespace DataSorting.WaitingWindow
{
    public partial class WaitBox
    {
        private static WWindow window;
        public static void Show(Window _win)
        {
            _win.IsEnabled = false;
            _win.IsHitTestVisible = false;
            _win.Effect = new BlurEffect() { Radius = 6 };

            window = new WWindow() { Owner = _win, WindowStartupLocation = WindowStartupLocation.CenterOwner, ShowInTaskbar = false };

            window.Show();
        }

        public static void Close(Window _win)
        {
            _win.IsEnabled = true;
            _win.IsHitTestVisible = true;
            _win.Effect = new BlurEffect() { Radius = 0 };
            window.Close();

        }
    }
}
