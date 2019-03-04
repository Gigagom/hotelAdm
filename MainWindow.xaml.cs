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
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            User session_user = new User();
            session_user.SetLabels(NameLabel, PositionLabel);
            LoginGrid.Visibility = Visibility.Hidden;
            ContentGrid.Visibility = Visibility.Visible;
        }

        private void SBSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            DBSettings.Visibility = Visibility.Visible;
            Button btn = (Button)sender;
            if (btn.Background != Brushes.Aqua)
                btn.Background = Brushes.Aqua;
            else
                btn.Background = Brushes.White;
        }
    }
}
