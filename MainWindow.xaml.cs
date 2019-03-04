using System;
using System.Collections.Generic;
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
using System.Configuration;

namespace hotelAdm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoginGrid.Visibility = Visibility.Visible;
            User.NewUser();
            Database.NewDB();
            Database.SetToTextboxes(DBServerSettingsTextbox, DBNameSettingsTextbox, DBUserSettingsTextbox, DBPassSettingsTextbox);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            User.SetLabels(NameLabel, PositionLabel);
            LoginGrid.Visibility = Visibility.Hidden;
            ContentGrid.Visibility = Visibility.Visible;
        }

        private void SBSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (DBSettings.Visibility != Visibility.Visible)
            {
                btn.Background = Brushes.Aqua;
                DBSettings.Visibility = Visibility.Visible;
            }
            else
            {
                btn.Background = Brushes.White;
                DBSettings.Visibility = Visibility.Hidden;
            }     
        }
    }
}
