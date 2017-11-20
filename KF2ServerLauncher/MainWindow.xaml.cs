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
        public string mConfigFilePath;

        public MainWindow()
        {
            InitializeComponent();

            string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string configFolderPath = Path.Combine(appDataFolderPath, "KF2Server");
            mConfigFilePath = Path.Combine(configFolderPath, "KF2Server.xml");

            LoadConfig();
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
            SaveConfig();

            Close();
        }

        private void SaveConfig()
        {
            string configFolderPath = Path.GetDirectoryName(mConfigFilePath);
            if (!Directory.Exists(configFolderPath))
            {
                System.IO.Directory.CreateDirectory(configFolderPath);
            }

            FileStream writer = new FileStream(mConfigFilePath, FileMode.OpenOrCreate);

            using (XmlWriter xmlWriter = XmlWriter.Create(writer))
            {
                xmlWriter.WriteStartElement("Config");
                xmlWriter.WriteElementString("Map", (string)((ComboBoxItem)mapComboBox.SelectedItem).Content);
                xmlWriter.WriteElementString("Difficulty", (string)((ComboBoxItem)difficultyComboBox.SelectedItem).Content);
                xmlWriter.WriteElementString("MaxPlayers", (string)((ComboBoxItem)maxPlayersComboBox.SelectedItem).Content);
                xmlWriter.WriteElementString("IsLanMatch", (isLanMatchCheckBox.IsChecked.HasValue ? true : false).ToString());

                xmlWriter.WriteEndDocument();

                writer.Flush();
            }
        }

        private void LoadConfig()
        {
            // If config file exists, load it
            if (File.Exists(mConfigFilePath))
            {
                FileStream reader = new FileStream(mConfigFilePath, FileMode.Open);

                using (XmlReader xmlReader = XmlReader.Create(reader))
                {
                    if (xmlReader.ReadToFollowing("Map"))
                    {
                        string map = xmlReader.ReadElementContentAsString();
                        foreach (ComboBoxItem item in mapComboBox.Items)
                        {
                            if ((string)item.Content == map)
                            {
                                mapComboBox.SelectedItem = item;

                                break;
                            }
                        }
                    }
                    if (xmlReader.ReadToFollowing("Difficulty"))
                    {
                        string difficulty = xmlReader.ReadElementContentAsString();
                        foreach (ComboBoxItem item in difficultyComboBox.Items)
                        {
                            if ((string)item.Content == difficulty)
                            {
                                difficultyComboBox.SelectedItem = item;

                                break;
                            }
                        }
                    }
                    if (xmlReader.ReadToFollowing("MaxPlayers"))
                    {
                        string maxPlayers = xmlReader.ReadElementContentAsString();
                        foreach (ComboBoxItem item in maxPlayersComboBox.Items)
                        {
                            if ((string)item.Content == maxPlayers)
                            {
                                maxPlayersComboBox.SelectedItem = item;

                                break;
                            }
                        }
                    }
                    if (xmlReader.ReadToFollowing("IsLanMatch"))
                    {
                        bool isLanMatch = xmlReader.ReadElementContentAsBoolean();
                        isLanMatchCheckBox.IsChecked = isLanMatch;
                    }
                }
            }
        }
    }
}
