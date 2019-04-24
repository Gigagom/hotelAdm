using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using MySql.Data;
using MySql.Data.MySqlClient;

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
            CurrrentUser.NewUser();
            Database.SetDBSettings();
            Database.SetToTextboxes(DBServerSettingsTextbox, 
                                    DBPortSettingsTextbox, 
                                    DBNameSettingsTextbox, 
                                    DBUserSettingsTextbox, 
                                    DBPassSettingsTextbox);
            LoginTextbox.Focus();
        }

        private void SetContent()
        {
            switch (CurrrentUser.Type)
            {
                case "Admin":
                    SetAdminContent();
                    break;
            }
        }

        private void SetAdminContent()
        {
            HotelApartments.TakeApartments();
            HotelApartments.SetApartmentsToGrid(ApartsDataGrid);
            DrawButtons(ApartsBtnGrid, HotelApartments.ApartmentsCount, 4, ShowNumberInfo);
            Users.TakeAllUsers(usersDataGrid);
            CurrrentUser.SetLabels(NameLabel, PositionLabel);
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
                string enteredLogin = LoginTextbox.Text;
                string enteredPassword = PasswordTextbox.Password.ToString();
                Load(true);
                if (CurrrentUser.Authorization(enteredLogin, enteredPassword))
                {
                    SetContent();
                    LoginGrid.Visibility = Visibility.Hidden;
                    ContentGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Данные введены неверно!");
                }          
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Ошибка!");
            }
            finally
            {
                Load(false);
            }
        }

        private void SBSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (SettingsGrid.Visibility != Visibility.Visible)
            {
                SetAllMenuButtonsToDefault();
                HideAllGrids();
                ReverseBtnColor(btn);
                SettingsGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ReverseBtnColor(btn);
                HideGrid(SettingsGrid);
            }     
        }

        public void ReverseBtnColor(Button btn)
        {
            Brush TextColor = btn.Background;
            btn.Background = btn.Foreground;
            btn.Foreground = TextColor;
        }

        public void ReverseBtnsColor(Button[] btns)
        {
            foreach (var btn in btns)
            {
                Brush TextColor = btn.Background;
                btn.Background = btn.Foreground;
                btn.Foreground = TextColor;
            }          
        }

        private void SaveSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfigChange.AddUpdateAppSettings("DBServer", DBServerSettingsTextbox.Text);
                ConfigChange.AddUpdateAppSettings("DBPort", DBPortSettingsTextbox.Text);
                ConfigChange.AddUpdateAppSettings("DBName", DBNameSettingsTextbox.Text);
                ConfigChange.AddUpdateAppSettings("DBUser", DBUserSettingsTextbox.Text);
                ConfigChange.AddUpdateAppSettings("DBPassword", DBPassSettingsTextbox.Text);
            }
            catch(ConfigChangeException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Database.SetDBSettings();
                Database.SetToTextboxes(DBServerSettingsTextbox, 
                                        DBPortSettingsTextbox, 
                                        DBNameSettingsTextbox, 
                                        DBUserSettingsTextbox, 
                                        DBPassSettingsTextbox);
                MessageBox.Show("Данные обновлены!");
                if (Database.CheckConnection())
                {
                    MessageBox.Show("База данных подключена!");
                }
                else
                {
                    MessageBox.Show("Отсутствует подключение к БД!");
                }
            }
        }
        public void Load(bool state)
        {
            if(state)
                LoadindGrid.Visibility = Visibility.Visible;
            else
                LoadindGrid.Visibility = Visibility.Hidden;
        }

        public void DrawButtons(Grid Target, double count, double columns, RoutedEventHandler hadler)
        {
            Target.Children.Clear();
            Target.ColumnDefinitions.Clear();
            int number = 1;
            double c = count / columns;
            double rows = Math.Round(c, 0, MidpointRounding.AwayFromZero);

            StackPanel[] panels = new StackPanel[Convert.ToInt32(columns)];

            
            for (int i = 0; i < columns; i++)
            {
                panels[i] = new StackPanel
                {
                    Name = "ApartPanel" + i,
                    Orientation = Orientation.Vertical
                };
                Target.ColumnDefinitions.Add(new ColumnDefinition());
                Target.Children.Add(panels[i]);
                Grid.SetColumn(panels[i], i);
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Button newBtn = new Button();
                    newBtn.Content = number.ToString();
                    newBtn.Name = "Button" + i.ToString() + j.ToString();
                    newBtn.Height = 100;
                    newBtn.FontSize = 50;
                    newBtn.Margin = new Thickness(20, 30, 20, 0);
                    newBtn.Click += new RoutedEventHandler(hadler);

                    panels[j].Children.Add(newBtn);
                    
                    count--;
                    number++;
                    if (count < 1) break;
                }
            }
        }

        private void ShowNumberInfo(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int num = Int32.Parse(btn.Content.ToString());
            Apartment tmp = HotelApartments.Apartsments[num];
            MessageBox.Show($"{tmp.id}, {tmp.countOfRooms}, {tmp.roomType}, {tmp.cleaningTime}, {tmp.price}, {tmp.isBusy}","Информация о номере");
        }

        private void ApartsBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (ApartsGrid.Visibility != Visibility.Visible)
            {
                SetAllMenuButtonsToDefault();
                HideAllGrids();
                ReverseBtnColor(btn);
                ApartsGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ReverseBtnColor(btn);
                HideGrid(ApartsGrid);
            }
        }

        private void HideAllGrids()
        {
            foreach (object obj in DataGrid.Children)
            {
                if(obj is Grid)
                {
                    var tmp = Visibility.Hidden;
                    Grid gr = (Grid)obj;
                    gr.Visibility = tmp;
                }                
            }            
        }

        private void HideGrid(Grid grid)
        {
            if (grid.Visibility == Visibility.Visible)
                grid.Visibility = Visibility.Hidden;
        }

        private void SetAllMenuButtonsToDefault()
        {
            foreach(object obj in MenuButtons.Children)
            {
                Button btn = (Button)obj;
                btn.Background = Brushes.White;
                btn.Foreground = Brushes.Black;
            }
        }

        private void UpdateUserBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateUserGrid.Visibility = Visibility.Visible;
        }

        private void DeleteUserBtn_Click(object sender, RoutedEventArgs e)
        {
            User u = (User)usersDataGrid.SelectedItem;
            if (u!=null)
            {
                MessageBoxResult rt = MessageBox.Show($"Удалить пользователя с id={u.id.ToString()}?", "Удаление", MessageBoxButton.YesNo);
                switch (rt)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            Load(true);
                            usersDataGrid.Items.Clear();
                            Users.DeleteUser(u.id);
                            Users.TakeAllUsers(usersDataGrid);
                            Load(false);
                            MessageBox.Show("Пользователь успешно удален");
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message,"Ошибка");
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя!");
            }
            usersDataGrid.SelectedItem = null;
        }

        private void DeleteApartBtn_Click(object sender, RoutedEventArgs e)
        {
            Apartment a = (Apartment)ApartsDataGrid.SelectedItem;
            if (a != null)
            {
                MessageBoxResult rt = MessageBox.Show($"Удалить номер с id={a.id.ToString()}?", "Удаление", MessageBoxButton.YesNo);
                switch (rt)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            Load(true);
                            ApartsDataGrid.Items.Clear();
                            HotelApartments.DeleteApartsment(a.id);
                            HotelApartments.TakeApartments();
                            HotelApartments.SetApartmentsToGrid(ApartsDataGrid);
                            DrawButtons(ApartsBtnGrid, HotelApartments.ApartmentsCount, 4, ShowNumberInfo);
                            Load(false);
                            MessageBox.Show("Номер успешно удален");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка");
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите номер!");
            }
            ApartsDataGrid.SelectedItem = null;
        }
    }
}
