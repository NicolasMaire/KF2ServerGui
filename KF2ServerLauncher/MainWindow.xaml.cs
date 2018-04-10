using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace KF2ServerLauncher
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string serverExecutablePath = @".\Binaries\Win64\KFServer.exe";

            if (System.IO.File.Exists(serverExecutablePath))
            {
                string map = (string)((ComboBoxItem)mapComboBox.SelectedItem).Content + ".kfm";
                string difficulty = (string)((ComboBoxItem)difficultyComboBox.SelectedItem).Content;
                string maxPlayers = (string)((ComboBoxItem)maxPlayersComboBox.SelectedItem).Content;
                string isLanMatch = isLanMatchCheckBox.IsChecked.HasValue ? (isLanMatchCheckBox.IsChecked.Value ? "True" : "False") : "False";

                string cmdText = "/C start " + serverExecutablePath + " " + map + "?adminpassword=123?Difficulty=" + difficulty + "?MaxPlayers=" + maxPlayers + "?bIsLanMatch=" + isLanMatch;

                System.Diagnostics.Process.Start("cmd.exe", cmdText);
            }
            else
            {
                System.Windows.MessageBox.Show("Server executable " + serverExecutablePath + " not found");
            }
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
