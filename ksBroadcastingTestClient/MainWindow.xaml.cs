using ksBroadcastingNetwork;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace ksBroadcastingTestClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var mainVM = (DataContext as MainViewModel);
            mainVM?.OnCloseWindowCommand?.Execute(null);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            var mainVM = (DataContext as MainViewModel);
            var k = (Key)Enum.Parse(typeof(Key), ConfigurationManager.AppSettings?[KeyBindings.Keybindings.FocusCarUp.ToString()], true);
            if (e.Key == k)
            {
                mainVM?.BroadcastingVM.RequestFocusedCar(-1);
                return;
            }
            k = (Key)Enum.Parse(typeof(Key), ConfigurationManager.AppSettings?[KeyBindings.Keybindings.FocusCarDown.ToString()], true);
            if (e.Key == k)
            {
                mainVM?.BroadcastingVM.RequestFocusedCar(1);
                return;
            }
            k = (Key)Enum.Parse(typeof(Key), ConfigurationManager.AppSettings?[KeyBindings.Keybindings.ChangeCamera.ToString()], true);
            if (e.Key == k)
            {
                mainVM?.BroadcastingVM.CameraChange();
                return;
            }
            k = (Key)Enum.Parse(typeof(Key), ConfigurationManager.AppSettings?[KeyBindings.Keybindings.ChangeCameraSet.ToString()], true);
            if (e.Key == k)
            {
                mainVM?.BroadcastingVM.CameraSetChange();
                return;
            }
        }
    }
}
