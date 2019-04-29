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
using System.Text.RegularExpressions;

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
            //для номеров
            HotelApartments.TakeApartments();
            HotelApartments.SetApartmentsToGrid(ApartsDataGrid);
            ApartTypeCollection.TakeApart();
            CleaningTimeCollection.TakeTime();
            DrawButtons(ApartsBtnGrid, HotelApartments.ApartmentsCount, 4, ShowNumberInfo);
            //для пользователей
            Users.TakeAllUsers(usersDataGrid);
            CurrrentUser.SetLabels(NameLabel, PositionLabel);
            UserTypeCollection.TakeUserType();
            PositionCollection.TakePositions();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
                string enteredLogin = LoginTextbox.Text;
                string enteredPassword = PasswordTextbox.Password.ToString();
                Task task = Task.Factory.StartNew(() => Load(true));
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
                Task task = Task.Factory.StartNew(() => Load(false));
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
        //Только цифры в textbox
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
            if (state)
            {
                Action action = () => LoadindGrid.Visibility = Visibility.Visible;
                LoadindGrid.Dispatcher.Invoke(action);
            }
            else
            {
                Action action = () => LoadindGrid.Visibility = Visibility.Hidden;
                LoadindGrid.Dispatcher.Invoke(action);
            }                
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
            MessageBox.Show($"{tmp.id}, {tmp.countOfRooms}, {tmp.roomType}, {tmp.cleaningTime}, {tmp.price}","Информация о номере");
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
            User u = (User)usersDataGrid.SelectedItem;
            if (u != null)
            {
                UpdateUserLoginTextBox.Text = u.login;
                UpdateUserPasswordTextBox.Text = u.password;
                UpdateUserFIOTextBox.Text = u.FIO;
                //Добавление в комбобоксы
                UserTypeCollection.UserTypeToBox(UpdateUserTypeTextBox);
                UpdateUserTypeTextBox.SelectedValue = u.type;
                PositionCollection.PositionsToBox(UpdateUserPositionTextBox);
                UpdateUserPositionTextBox.SelectedValue = u.position_name;
                //
                UsersControlBtnGrid.Visibility = Visibility.Hidden;
                UpdateUserGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите пользователя!");
            }
            usersDataGrid.SelectedItem = null;
        }
        //Удаление пользвателя
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
                            Task task = Task.Factory.StartNew(() => Load(true));
                            usersDataGrid.Items.Clear();
                            Users.DeleteUser(u.id);
                            Users.TakeAllUsers(usersDataGrid);
                            task = Task.Factory.StartNew(() => Load(false));
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
        //Удаление номера
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
                            Task task = Task.Factory.StartNew(() => Load(true));
                            ApartsDataGrid.Items.Clear();
                            HotelApartments.DeleteApartsment(a.id);
                            HotelApartments.TakeApartments();
                            HotelApartments.SetApartmentsToGrid(ApartsDataGrid);
                            DrawButtons(ApartsBtnGrid, HotelApartments.ApartmentsCount, 4, ShowNumberInfo);
                            task = Task.Factory.StartNew(() => Load(false));
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

        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            UserTypeCollection.UserTypeToBox(CreateUserTypeTextBox);
            PositionCollection.PositionsToBox(CreateUserPositionTextBox);
            UsersControlBtnGrid.Visibility = Visibility.Hidden;
            CreateUserGrid.Visibility = Visibility.Visible;
        }
        //Кнопки на панели создания нового пользователя
        private void CreateUserCanselBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateUserLoginTextBox.Clear();
            CreateUserPasswordTextBox.Clear();
            CreateUserFIOTextBox.Clear();
            CreateUserTypeTextBox.Items.Clear();
            CreateUserPositionTextBox.Items.Clear();
            UsersControlBtnGrid.Visibility = Visibility.Visible;
            CreateUserGrid.Visibility = Visibility.Hidden;
        }

        private void CreateUserSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{CreateUserLoginTextBox.Text} :: {CreateUserPasswordTextBox.Text} :: {CreateUserFIOTextBox.Text} :: {CreateUserTypeTextBox.SelectedValue} :: {CreateUserPositionTextBox.SelectedValue}");
        }
        //Кнопки на панели редактирования пользователя
        private void UpdateUserCanselBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateUserLoginTextBox.Clear();
            UpdateUserPasswordTextBox.Clear();
            UpdateUserFIOTextBox.Clear();
            UpdateUserTypeTextBox.Items.Clear();
            UpdateUserPositionTextBox.Items.Clear();
            UsersControlBtnGrid.Visibility = Visibility.Visible;
            UpdateUserGrid.Visibility = Visibility.Hidden;
        }

        private void UpdateUserSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{UpdateUserLoginTextBox.Text} :: {UpdateUserPasswordTextBox.Text} :: {UpdateUserFIOTextBox.Text} :: {UpdateUserTypeTextBox.SelectedValue} :: {UpdateUserPositionTextBox.SelectedValue}");
        }

        private void UpdateApartBtn_Click(object sender, RoutedEventArgs e)
        {
            Apartment a = (Apartment)ApartsDataGrid.SelectedItem;
            if (a != null)
            {
                UpdateApartLabel.Content += a.id.ToString();
                UpdateApartRoomsTextBox.Text = a.countOfRooms.ToString();
                UpdateApartCostTextBox.Text = a.price.ToString();
                //
                ApartTypeCollection.ApartToBox(UpdateApartTypeTextBox);
                UpdateApartTypeTextBox.SelectedValue = a.roomType;
                CleaningTimeCollection.TimeToBox(UpdateApartTimeTextBox);
                UpdateApartTimeTextBox.SelectedValue = a.cleaningTime;
                //
                ApartControlBtnGrid.Visibility = Visibility.Hidden;
                UpdateApartGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите номер!");
            }
            ApartsDataGrid.SelectedItem = null;
        }
        //Кнопки на панели редактирования номера
        private void UpdateApartCanselBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateApartLabel.Content = "Редактирование Номера №";
            UpdateApartRoomsTextBox.Clear();
            UpdateApartTypeTextBox.Items.Clear();
            UpdateApartTimeTextBox.Items.Clear();
            UpdateApartCostTextBox.Clear();
            ApartControlBtnGrid.Visibility = Visibility.Visible;
            UpdateApartGrid.Visibility = Visibility.Hidden;
        }

        private void UpdateApartSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{UpdateApartRoomsTextBox.Text} :: {UpdateApartTypeTextBox.SelectedValue} :: {UpdateApartTimeTextBox.SelectedValue} :: {UpdateApartCostTextBox.Text}");
        }

        private void AddApartBtn_Click(object sender, RoutedEventArgs e)
        {
            ApartTypeCollection.ApartToBox(CreateApartTypeTextBox);
            CleaningTimeCollection.TimeToBox(CreateApartTimeTextBox);
            ApartControlBtnGrid.Visibility = Visibility.Hidden;
            CreateApartGrid.Visibility = Visibility.Visible;
        }
        //Кнопки на панели создания нового номера
        private void CreateApartCanselBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateApartRoomsTextBox.Clear();
            CreateApartTypeTextBox.Items.Clear();
            CreateApartTimeTextBox.Items.Clear();
            CreateApartCostTextBox.Clear();
            ApartControlBtnGrid.Visibility = Visibility.Visible;
            CreateApartGrid.Visibility = Visibility.Hidden;
        }

        private void CreateApartSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{CreateApartRoomsTextBox.Text} :: {CreateApartTypeTextBox.SelectedValue} :: {CreateApartTimeTextBox.SelectedValue} :: {CreateApartCostTextBox.Text}");
        }
    }
}
