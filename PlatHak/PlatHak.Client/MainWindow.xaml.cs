using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using PlatHak.Client.Common;

namespace PlatHak.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetConfig();
        }

        private void SetConfig()
        {
            CheckBoxClientVSync.IsChecked = Properties.Settings.Default.Client_Vsync;
            CheckBoxClientFullscreen.IsChecked = Properties.Settings.Default.Client_Fullscreen;
            TextBoxServerAddress.Text = Properties.Settings.Default.Server_Address;
            CheckBoxServerSpectator.IsChecked = Properties.Settings.Default.Server_Spectator;

            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.Client_LastUsername))
                TextBoxUsername.Text = Properties.Settings.Default.Client_LastUsername;
        }

        private void ButtonSettings_OnClick(object sender, RoutedEventArgs e)
        {
            GridConfig.Visibility = Visibility.Visible;
            ((Control) sender).Visibility = Visibility.Hidden;
        }

        private void TextBoxUsername_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.Client_LastUsername = ((TextBox) sender).Text;
        }

        private void TextBoxServerAddress_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.Server_Address = ((TextBox)sender).Text;
        }

        private void CheckBoxServerSpectator_OnChecked(object sender, RoutedEventArgs e)
        {
            var isChecked = ((CheckBox)sender).IsChecked;
            if (isChecked != null)
                Properties.Settings.Default.Server_Spectator = isChecked.Value;
        }

        private void CheckBoxClientFullscreen_OnChecked(object sender, RoutedEventArgs e)
        {
            var isChecked = ((CheckBox)sender).IsChecked;
            if (isChecked != null)
                Properties.Settings.Default.Client_Fullscreen = isChecked.Value;
        }

        private void CheckBoxClientVSync_OnChecked(object sender, RoutedEventArgs e)
        {
            var isChecked = ((CheckBox)sender).IsChecked;
            if (isChecked != null)
                Properties.Settings.Default.Client_Vsync = isChecked.Value;
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void ButtonStart_OnClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            PlatHack.Game.Game.Start(TextBoxServerAddress.Text, TextBoxUsername.Text, TextBoxPassword.Password, new GameConfiguration
            {
                Title = "PlatHaK (PRE ALHPA) - " + TextBoxUsername.Text,
                DisableResizeButtons = true,
                Resizeable = false,
                WaitVerticalBlanking = Properties.Settings.Default.Client_Vsync,
                IsFullscreen = Properties.Settings.Default.Client_Fullscreen,
                Height = Properties.Settings.Default.Client_Fullscreen ? (int)SystemParameters.PrimaryScreenHeight : 768,
                Width = Properties.Settings.Default.Client_Fullscreen ? (int)SystemParameters.PrimaryScreenWidth : 1024,
            });
        }
    }
}
