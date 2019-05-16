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
using System.Diagnostics;

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
                case "StokWorker":
                    SetStokWorkerContent();
                    break;
                case "ReceptionWorker":
                    SetBasicContent();
                    break;
            }
        }
        private void SetBasicContent()
        {
            CurrrentUser.SetLabels(NameLabel, PositionLabel);
            NewOfferBtn.Visibility = Visibility.Visible;
            ApartsBtn.Visibility = Visibility.Visible;
            OffersBtn.Visibility = Visibility.Visible;
            GuestsBtn.Visibility = Visibility.Visible;
            //для номеров
            HotelApartments.TakeApartments();
            HotelApartments.SetApartmentsToGrid(ApartsDataGrid);
            ApartTypeCollection.TakeApart();
            CleaningTimeCollection.TakeTime();
            DrawButtons(ApartsBtnGrid, HotelApartments.ApartmentsCount, 4, ShowNumberInfo);
            //для клиентов
            ClientCollection.TakeClients();
            ClientCollection.ClientsToDG(ClientsDataGrid);
            //для заказов
            OrderCollection.TakeOrders();
            OrderCollection.OrdersToDG(OrderListDataGrid);
        }
        private void SetStokWorkerContent()
        {
            CurrrentUser.SetLabels(NameLabel, PositionLabel);
            ApartsBtn.Visibility = Visibility.Visible;
            OffersBtn.Visibility = Visibility.Visible;
            StokBtn.Visibility = Visibility.Visible;
            GuestsBtn.Visibility = Visibility.Visible;
            //для номеров
            HotelApartments.TakeApartments();
            HotelApartments.SetApartmentsToGrid(ApartsDataGrid);
            ApartTypeCollection.TakeApart();
            CleaningTimeCollection.TakeTime();
            DrawButtons(ApartsBtnGrid, HotelApartments.ApartmentsCount, 4, ShowNumberInfo);
            //для клиентов
            ClientCollection.TakeClients();
            ClientCollection.ClientsToDG(ClientsDataGrid);
            //для заказов
            OrderCollection.TakeOrders();
            OrderCollection.OrdersToDG(OrderListDataGrid);
            //для продуктов
            ProductCollection.TakeProducts();
            ProductCollection.ProductsToDG(ProductsDataGrid);
            //для поставщиков
            ProviderCollection.TakeProviders();
            ProviderCollection.ProvidersToDG(ProviderDataGrid);
            //для запросов к поставщикам
            RequestCollection.TakeRequests();
            RequestCollection.RequestSsToDG(RequestsDataGrid);
        }
        private void SetAdminContent()
        {
            //****Buttons****//
            NewOfferBtn.Visibility = Visibility.Visible;
            ApartsBtn.Visibility = Visibility.Visible;
            OffersBtn.Visibility = Visibility.Visible;
            StokBtn.Visibility = Visibility.Visible;
            GuestsBtn.Visibility = Visibility.Visible;
            SBSettingsBtn.Visibility = Visibility.Visible;
            //****Buttons****//
            //****Set Data*****//
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
            //для клиентов
            ClientCollection.TakeClients();
            ClientCollection.ClientsToDG(ClientsDataGrid);
            //для заказов
            OrderCollection.TakeOrders();
            OrderCollection.OrdersToDG(OrderListDataGrid);
            //для продуктов
            ProductCollection.TakeProducts();
            ProductCollection.ProductsToDG(ProductsDataGrid);
            //для поставщиков
            ProviderCollection.TakeProviders();
            ProviderCollection.ProvidersToDG(ProviderDataGrid);
            //для запросов к поставщикам
            RequestCollection.TakeRequests();
            RequestCollection.RequestSsToDG(RequestsDataGrid);
            //****Set Data*****//
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
                DBSettingsTab.SelectedIndex = 0;
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
                //перезапуск приложения
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
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

        public void Load(Task t)
        {
            Action action = () => LoadindGrid.Visibility = Visibility.Hidden;
            LoadindGrid.Dispatcher.Invoke(action);
            Thread.Sleep(3000);
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
                    newBtn.Height = 150;
                    newBtn.Style = (Style)FindResource("ForApartButtons");
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
            ApartInfoLabel.Content += tmp.id.ToString();
            ApartInfoRoomsTextBox.Text = tmp.countOfRooms.ToString();
            ApartInfoTypeTextBox.Text = tmp.roomType;
            ApartInfoTimeTextBox.Text = tmp.cleaningTime;
            ApartInfoCostTextBox.Text = tmp.price.ToString();
            ApartInfoGrid.Visibility = Visibility.Visible;
        }

        private void ApartInfoCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            ApartInfoGrid.Visibility = Visibility.Hidden;
            ApartInfoLabel.Content = "Информация о номере №";
            ApartInfoRoomsTextBox.Clear();
            ApartInfoTypeTextBox.Clear();
            ApartInfoTimeTextBox.Clear();
            ApartInfoCostTextBox.Clear();
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
                UserIdForUpdate.Text = u.id.ToString();
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
                            Users.DeleteUser(u.id);
                            //обновление данных в гриде
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
            CreateUserTypeTextBox.Text = "Выберите";
            CreateUserPositionTextBox.Text = "Выберите";
            UsersControlBtnGrid.Visibility = Visibility.Visible;
            CreateUserGrid.Visibility = Visibility.Hidden;
        }

        private void CreateUserSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Users.CreateUser(CreateUserLoginTextBox.Text, CreateUserPasswordTextBox.Text, CreateUserFIOTextBox.Text, CreateUserTypeTextBox.SelectedValue.ToString(), CreateUserPositionTextBox.SelectedValue.ToString());
                Users.TakeAllUsers(usersDataGrid);
                //скрываем и очищаем поля
                CreateUserLoginTextBox.Clear();
                CreateUserPasswordTextBox.Clear();
                CreateUserFIOTextBox.Clear();
                CreateUserTypeTextBox.Text = "Выберите";
                CreateUserPositionTextBox.Text = "Выберите";
                UsersControlBtnGrid.Visibility = Visibility.Visible;
                CreateUserGrid.Visibility = Visibility.Hidden;
                MessageBox.Show("Пользователь добавлен!","Готово!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }             
        }
        //Кнопки на панели редактирования пользователя
        private void UpdateUserCanselBtn_Click(object sender, RoutedEventArgs e)
        {
            UserIdForUpdate.Clear();
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
            try
            {
                Users.UpdateUser(Convert.ToInt32(UserIdForUpdate.Text), UpdateUserLoginTextBox.Text, UpdateUserPasswordTextBox.Text, UpdateUserFIOTextBox.Text, UpdateUserTypeTextBox.SelectedValue.ToString(), UpdateUserPositionTextBox.SelectedValue.ToString());
                Users.TakeAllUsers(usersDataGrid);
                //скрываем и очищаем поля
                UserIdForUpdate.Clear();
                UpdateUserLoginTextBox.Clear();
                UpdateUserPasswordTextBox.Clear();
                UpdateUserFIOTextBox.Clear();
                UpdateUserTypeTextBox.Text = "Выберите";
                UpdateUserPositionTextBox.Text = "Выберите";
                UsersControlBtnGrid.Visibility = Visibility.Visible;
                UpdateUserGrid.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateApartBtn_Click(object sender, RoutedEventArgs e)
        {
            Apartment a = (Apartment)ApartsDataGrid.SelectedItem;
            if (a != null)
            {
                UpdateApartId.Text = a.id.ToString();
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
            UpdateApartId.Clear();
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
            try
            {
                HotelApartments.UpdateApartsment(Convert.ToInt32(UpdateApartId.Text), Convert.ToInt32(UpdateApartRoomsTextBox.Text), UpdateApartTypeTextBox.SelectedValue.ToString(), UpdateApartTimeTextBox.SelectedValue.ToString(), Convert.ToDouble(UpdateApartCostTextBox.Text));
                HotelApartments.TakeApartments();
                HotelApartments.SetApartmentsToGrid(ApartsDataGrid);
                //
                UpdateApartId.Clear();
                UpdateApartLabel.Content = "Редактирование Номера №";
                UpdateApartRoomsTextBox.Clear();
                UpdateApartTypeTextBox.Items.Clear();
                UpdateApartTimeTextBox.Items.Clear();
                UpdateApartCostTextBox.Clear();
                ApartControlBtnGrid.Visibility = Visibility.Visible;
                UpdateApartGrid.Visibility = Visibility.Hidden;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            try
            {
                HotelApartments.CreateApartsment(Convert.ToInt32(CreateApartRoomsTextBox.Text), CreateApartTypeTextBox.SelectedValue.ToString(), CreateApartTimeTextBox.SelectedValue.ToString(), Convert.ToDouble(CreateApartCostTextBox.Text));
                HotelApartments.TakeApartments();
                HotelApartments.SetApartmentsToGrid(ApartsDataGrid);
                DrawButtons(ApartsBtnGrid, HotelApartments.ApartmentsCount, 4, ShowNumberInfo);
                //
                CreateApartRoomsTextBox.Clear();
                CreateApartTypeTextBox.Items.Clear();
                CreateApartTimeTextBox.Items.Clear();
                CreateApartCostTextBox.Clear();
                ApartControlBtnGrid.Visibility = Visibility.Visible;
                CreateApartGrid.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Magazine_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ActionLogCollection.TakeActionLog(LogDG);
        }

        private void SaveDBBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database.BackUp();
                MessageBox.Show("Резервное копироание успешно выполнено!","Готово!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GuestsBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (GuestsGrid.Visibility != Visibility.Visible)
            {
                SetAllMenuButtonsToDefault();
                HideAllGrids();
                ReverseBtnColor(btn);
                GuestsGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ReverseBtnColor(btn);
                HideGrid(GuestsGrid);
            }
        }

        private void ClientSearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            var filter = ClientCollection.ClientsList.Where(Client => Client.Passport_number.StartsWith(t.Text));
            ClientsDataGrid.Items.Clear();
            foreach (Client a in filter)
                ClientsDataGrid.Items.Add(a);
        }

        private void OffersBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (OfferListGrid.Visibility != Visibility.Visible)
            {
                SetAllMenuButtonsToDefault();
                HideAllGrids();
                ReverseBtnColor(btn);
                OfferListGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ReverseBtnColor(btn);
                HideGrid(OfferListGrid);
            }
        }

        private void OrderListDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Order a = (Order)OrderListDataGrid.SelectedItem;
            if (a != null)
            {
                try
                {
                    OrderInfoPasswordNumber.Text = ClientCollection.TakePassById(a.GuestID);
                    OrderInfoApartListNumber.Text = HotelApartments.GetApartsByOrder(a.Id);
                    OrderInfoLabel.Content += a.Id.ToString();
                    OrderInfoDays.Text = a.DaysCount;
                    OrderInfoPrice.Text = a.Price.ToString();
                    OrderInfoStartDay.Text = a.StartDay;
                    OrderInfoRegisterDateTime.Text = a.Date;
                    OfferInfoGrid.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ!");
            }
            OrderListDataGrid.SelectedItem = null;
        }

        private void OrderInfoCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            OrderInfoLabel.Content = "Информация о Заказе №";
            OrderInfoPasswordNumber.Clear();
            OrderInfoApartListNumber.Clear();
            OrderInfoDays.Clear();
            OrderInfoPrice.Clear();
            OrderInfoStartDay.Clear();
            OrderInfoRegisterDateTime.Clear();
            OfferInfoGrid.Visibility = Visibility.Hidden;
        }

        private void StokBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (StokGrid.Visibility != Visibility.Visible)
            {
                SetAllMenuButtonsToDefault();
                HideAllGrids();
                ReverseBtnColor(btn);
                StokTabs.SelectedIndex = 0;
                StokGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ReverseBtnColor(btn);
                HideGrid(StokGrid);
            }
        }
        //ПРОДУКТЫ
        //кнопки создания/удаления/редактирования продуктов
        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            UnitCollection.TakeUnits();
            UnitCollection.UnitsToBox(CreateProductUnitsComboBox);
            ProductsControlBtnGrid.Visibility = Visibility.Hidden;
            CreateProductGrid.Visibility = Visibility.Visible;
        }

        private void UpdateProductBtn_Click(object sender, RoutedEventArgs e)
        {
            Product a = (Product)ProductsDataGrid.SelectedItem;
            if (a != null)
            {
                ProductIdForUpdate.Text = a.Id.ToString();
                UpdateProductNameTextBox.Text = a.Name;
                UpdateProductCountTextBox.Text = a.Count.ToString();
                //
                UnitCollection.TakeUnits();
                UnitCollection.UnitsToBox(UpdateProductUnitsComboBox);
                UpdateProductUnitsComboBox.SelectedValue = a.Unit;
                //
                ProductsControlBtnGrid.Visibility = Visibility.Hidden;
                UpdateProductGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите продукт!");
            }
            ProductsDataGrid.SelectedItem = null;
        }

        private void DeleteProductBtn_Click(object sender, RoutedEventArgs e)
        {
            Product a = (Product)ProductsDataGrid.SelectedItem;
            if (a != null)
            {
                MessageBoxResult rt = MessageBox.Show($"Удалить продукт с id={a.Id.ToString()}?", "Удаление", MessageBoxButton.YesNo);
                switch (rt)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            Load(true);
                            ProductCollection.DeleteProduct(a.Id);
                            ProductCollection.TakeProducts();
                            ProductCollection.ProductsToDG(ProductsDataGrid);
                            Load(false);
                            MessageBox.Show("Продукт успешно удален!");
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
                MessageBox.Show("Выберите продукт!");
            }
            ProductsDataGrid.SelectedItem = null;
        }

        //кнопки сохранения/отмены добавления нового продукта
        private void CreateProductCanselBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateProductCountTextBox.Clear();
            CreateProductUnitsComboBox.Items.Clear();
            CreateProductNameTextBox.Clear();
            CreateProductUnitsComboBox.Text = "Выберите";
            ProductsControlBtnGrid.Visibility = Visibility.Visible;
            CreateProductGrid.Visibility = Visibility.Hidden;
        }

        private void CreateProductSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductCollection.CreateProduct(CreateProductNameTextBox.Text, Convert.ToInt32(CreateProductCountTextBox.Text),UnitCollection.NameToId(CreateProductUnitsComboBox.SelectedValue.ToString()));
                ProductCollection.TakeProducts();
                ProductCollection.ProductsToDG(ProductsDataGrid);
                //
                CreateProductCountTextBox.Clear();
                CreateProductUnitsComboBox.Items.Clear();
                CreateProductNameTextBox.Clear();
                ProductsControlBtnGrid.Visibility = Visibility.Visible;
                CreateProductGrid.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //кнопки сохранения/отмены редактирования продукта
        private void UpdateProductCanselBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductIdForUpdate.Clear();
            UpdateProductUnitsComboBox.Items.Clear();
            UpdateProductNameTextBox.Clear();
            UpdateProductCountTextBox.Clear();
            ProductsControlBtnGrid.Visibility = Visibility.Visible;
            UpdateProductGrid.Visibility = Visibility.Hidden;
        }

        private void UpdateProductSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductCollection.UpdateProduct(Convert.ToInt32(ProductIdForUpdate.Text), UpdateProductNameTextBox.Text, Convert.ToInt32(UpdateProductCountTextBox.Text), UnitCollection.NameToId(UpdateProductUnitsComboBox.SelectedValue.ToString()));
                ProductCollection.TakeProducts();
                ProductCollection.ProductsToDG(ProductsDataGrid);
                //
                ProductIdForUpdate.Clear();
                UpdateProductUnitsComboBox.Items.Clear();
                UpdateProductNameTextBox.Clear();
                UpdateProductCountTextBox.Clear();
                ProductsControlBtnGrid.Visibility = Visibility.Visible;
                UpdateProductGrid.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //ПОСТАВЩИКИ
        //кнопки создания/удаления/редактирования поставщиков
        private void AddProviderBtn_Click(object sender, RoutedEventArgs e)
        {
            ProviderControlBtnGrid.Visibility = Visibility.Hidden;
            CreateProviderGrid.Visibility = Visibility.Visible;
        }

        private void UpdateProviderBtn_Click(object sender, RoutedEventArgs e)
        {
            Provider a = (Provider)ProviderDataGrid.SelectedItem;
            if (a != null)
            {
                ProviderIdForUpdate.Text = a.Id.ToString();
                UpdateProviderNameTextBox.Text = a.Name;
                UpdateProviderTelTextBox.Text = a.Phone;
                //
                ProviderControlBtnGrid.Visibility = Visibility.Hidden;
                UpdateProviderGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите поставщика!");
            }
            ProviderDataGrid.SelectedItem = null;
        }

        private void DeleteProviderBtn_Click(object sender, RoutedEventArgs e)
        {
            Provider a = (Provider)ProviderDataGrid.SelectedItem;
            if (a != null)
            {
                MessageBoxResult rt = MessageBox.Show($"Удалить поставщика с id={a.Id.ToString()}?", "Удаление", MessageBoxButton.YesNo);
                switch (rt)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            Load(true);
                            ProviderCollection.DeleteProvider(a.Id);
                            ProviderCollection.TakeProviders();
                            ProviderCollection.ProvidersToDG(ProviderDataGrid);
                            Load(false);
                            MessageBox.Show("Поставщик успешно удален!");
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
                MessageBox.Show("Выберите поставщика!");
            }
            ProviderDataGrid.SelectedItem = null;
        }
        //кнопки сохранения/отмены добавления нового поставщика
        private void CreateProviderCanselBtn_Click(object sender, RoutedEventArgs e)
        {
            ProviderControlBtnGrid.Visibility = Visibility.Visible;
            CreateProviderGrid.Visibility = Visibility.Hidden;
            CreateProviderNameTextBox.Clear();
            CreateProviderTelTextBox.Clear();
        }

        private void CreateProviderSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProviderCollection.CreateProvider(CreateProviderNameTextBox.Text, CreateProviderTelTextBox.Text);
                ProviderCollection.TakeProviders();
                ProviderCollection.ProvidersToDG(ProviderDataGrid);
                //
                ProviderControlBtnGrid.Visibility = Visibility.Visible;
                CreateProviderGrid.Visibility = Visibility.Hidden;
                CreateProviderNameTextBox.Clear();
                CreateProviderTelTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //кнопки сохранения/отмены редактирования поставщика
        private void UpdateProviderCanselBtn_Click(object sender, RoutedEventArgs e)
        {
            ProviderIdForUpdate.Clear();
            UpdateProviderNameTextBox.Clear();
            UpdateProviderTelTextBox.Clear();
            ProviderControlBtnGrid.Visibility = Visibility.Visible;
            UpdateProviderGrid.Visibility = Visibility.Hidden;
        }

        private void UpdateProviderSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProviderCollection.UpdateProvider(Convert.ToInt32(ProviderIdForUpdate.Text), UpdateProviderNameTextBox.Text, UpdateProviderTelTextBox.Text);
                ProviderCollection.TakeProviders();
                ProviderCollection.ProvidersToDG(ProviderDataGrid);
                //
                ProviderIdForUpdate.Clear();
                UpdateProviderNameTextBox.Clear();
                UpdateProviderTelTextBox.Clear();
                ProviderControlBtnGrid.Visibility = Visibility.Visible;
                UpdateProviderGrid.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //кнопки создания/завешения/удаления запросов к поставщикам
        private void AddRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductCollection.ProductsToCB(CreateRequestProductComboBox);
            ProviderCollection.ProvidersToCB(CreateRequestProviderComboBox);
            CreateRequestGrid.Visibility = Visibility.Visible;
            RequestsControlBtnGrid.Visibility = Visibility.Hidden;
        }

        private void UpdateRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            Request a = (Request)RequestsDataGrid.SelectedItem;
            if (a != null)
            {
                MessageBoxResult rt = MessageBox.Show($"Закрыть запрос с id={a.Id.ToString()}?", "Закрытие", MessageBoxButton.YesNo);
                switch (rt)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            Load(true);
                            RequestCollection.CloseRequest(a.Id, ProductCollection.NameToId(a.ProdName), a.Count);
                            ProductCollection.TakeProducts();
                            ProductCollection.ProductsToDG(ProductsDataGrid);
                            RequestCollection.TakeRequests();
                            RequestCollection.RequestSsToDG(RequestsDataGrid);
                            Load(false);
                            MessageBox.Show("Запрос успешно закрыт!");
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
                MessageBox.Show("Выберите запрос!");
            }
            RequestsDataGrid.SelectedItem = null;
        }

        private void DeleteRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            Request a = (Request)RequestsDataGrid.SelectedItem;
            if (a != null)
            {
                MessageBoxResult rt = MessageBox.Show($"Удалить запрос с id={a.Id.ToString()}?", "Удаление", MessageBoxButton.YesNo);
                switch (rt)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            Load(true);
                            RequestCollection.DeleteRequest(a.Id);
                            RequestCollection.TakeRequests();
                            RequestCollection.RequestSsToDG(RequestsDataGrid);
                            Load(false);
                            MessageBox.Show("Запрос успешно удален!");
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
                MessageBox.Show("Выберите запрос!");
            }
            RequestsDataGrid.SelectedItem = null;
        }
        //кнопки отмены/сохранения нового запроса
        private void CreateRequestCanselBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateRequestProductComboBox.Text = "Выберите";
            CreateRequestProviderComboBox.Text = "Выберите";
            CreateRequestGrid.Visibility = Visibility.Hidden;
            RequestsControlBtnGrid.Visibility = Visibility.Visible;
        }

        private void CreateRequestSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int prod = ProductCollection.NameToId(CreateRequestProductComboBox.SelectedValue.ToString());
                int provider = ProviderCollection.NameToId(CreateRequestProviderComboBox.SelectedValue.ToString());
                RequestCollection.CreateRequest(prod, Convert.ToInt32(CreateRequestCountTextBox.Text), provider);
                RequestCollection.TakeRequests();
                RequestCollection.RequestSsToDG(RequestsDataGrid);
                //
                CreateRequestProductComboBox.Text = "Выберите";
                CreateRequestProviderComboBox.Text = "Выберите";
                CreateRequestGrid.Visibility = Visibility.Hidden;
                RequestsControlBtnGrid.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        private void NewOfferBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (NewOfferGrid.Visibility != Visibility.Visible)
            {
                SetAllMenuButtonsToDefault();
                HideAllGrids();
                ReverseBtnColor(btn);
                NewOfferGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ReverseBtnColor(btn);
                HideGrid(NewOfferGrid);
            }
        }

        private void ClientForSearchOKBtn_Click(object sender, RoutedEventArgs e)
        {
            NewOrder.Clear();
            if (ClientForSearchTextBox.Text != "")
            {
                NewOrder.Client = ClientCollection.TakeClientByPass(ClientForSearchTextBox.Text);
                if (NewOrder.Client != null)
                {
                    SearchClientForNewOrderGrid.Visibility = Visibility.Hidden;
                    ChooseApartsForNewOrderGrid.Visibility = Visibility.Visible;
                    DrawButtons(ForAparts, HotelApartments.ApartmentsCount, 2, AddApartInList);
                }
                else
                {
                    MessageBoxResult rt = MessageBox.Show($"Клиент с введенным номером паспорта не найден. Создать нового?", "Новый клиент?", MessageBoxButton.YesNo);
                    switch (rt)
                    {
                        case MessageBoxResult.Yes:
                            PassNewClientNewOrder.Text = ClientForSearchTextBox.Text;
                            NewClientForNewOrderGrid.Visibility = Visibility.Visible;
                            SearchClientForNewOrderGrid.Visibility = Visibility.Hidden;
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Введите номер паспорта");
            }
        }

        private void CancelAddingNewOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchClientForNewOrderGrid.Visibility = Visibility.Visible;
            NewClientForNewOrderGrid.Visibility = Visibility.Hidden;
            ChooseApartsForNewOrderGrid.Visibility = Visibility.Hidden;
            ChooseDatesForNewOrderGrid.Visibility = Visibility.Hidden;
            ConfirmNewOrderGrid.Visibility = Visibility.Hidden;
            FIONewClientNewOrder.Clear();
            PassNewClientNewOrder.Clear();
            TelNewClientNewOrder.Clear();
            ClientForSearchTextBox.Clear();
            StartDayNewOrder.SelectedDate = null;
            DaysCountNewOrder.Clear();
            ConfirmFIONewOrder.Clear();
            ConfirmPassNewOrder.Clear();
            ConfirmTelNewOrder.Clear();
            ConfirmApartsNewOrder.Clear();
            ConfirmCountNewOrder.Clear();
            ConfirmDataNewOrder.Clear();
            ConfirmPriceNewOrder.Clear();
            NewOrder.Clear();
        }

        private void SaveNewClientBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClientCollection.AddClient(FIONewClientNewOrder.Text, PassNewClientNewOrder.Text, TelNewClientNewOrder.Text);
                ClientCollection.TakeClients();
                ClientCollection.ClientsToDG(ClientsDataGrid);
                NewOrder.Client = ClientCollection.TakeClientByPass(PassNewClientNewOrder.Text);

                FIONewClientNewOrder.Clear();
                PassNewClientNewOrder.Clear();
                TelNewClientNewOrder.Clear();

                HotelApartments.TakeApartments();
                HotelApartments.SetApartmentsToGrid(ApartsDataGrid);
                ApartTypeCollection.TakeApart();
                CleaningTimeCollection.TakeTime();
                DrawButtons(ApartsBtnGrid, HotelApartments.ApartmentsCount, 4, ShowNumberInfo);

                NewClientForNewOrderGrid.Visibility = Visibility.Hidden;
                ChooseApartsForNewOrderGrid.Visibility = Visibility.Visible;
                DrawButtons(ForAparts, HotelApartments.ApartmentsCount, 2, AddApartInList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void AddApartInList(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Style == (Style)FindResource("ForApartButtons"))
            {
                btn.Style = (Style)FindResource("ForAddingButtons");
                NewOrder.Aparts.Add(HotelApartments.GetApartById(Int32.Parse(btn.Content.ToString())));
            }
            else
            {
                var item = NewOrder.Aparts.SingleOrDefault(x => x.id == Int32.Parse(btn.Content.ToString()));
                if (item != null)
                    NewOrder.Aparts.Remove(item);
                btn.Style = (Style)FindResource("ForApartButtons");
            }
        }
        private void SaveApartsListBtn_Click(object sender, RoutedEventArgs e)
        {            
            if (NewOrder.Aparts.Count > 0)
            {
                MessageBoxResult rt = MessageBox.Show($"Закончить выбор номеров?", "Закончить?", MessageBoxButton.YesNo);
                switch (rt)
                {
                    case MessageBoxResult.Yes:
                        ChooseApartsForNewOrderGrid.Visibility = Visibility.Hidden;
                        ChooseDatesForNewOrderGrid.Visibility = Visibility.Visible;
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите хотя бы 1 номер!");
            }
        }

        private void SaveDateInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            NewOrder.StartDay = StartDayNewOrder.Text.ToString();
            NewOrder.DaysCount = Int32.Parse(DaysCountNewOrder.Text);
            NewOrder.SetPrice();
            ConfirmFIONewOrder.Text = NewOrder.Client.Name;
            ConfirmPassNewOrder.Text = NewOrder.Client.Passport_number;
            ConfirmTelNewOrder.Text = NewOrder.Client.Phone;
            ConfirmApartsNewOrder.Text = NewOrder.GetApartList();
            ConfirmCountNewOrder.Text = NewOrder.DaysCount.ToString();
            ConfirmDataNewOrder.Text = NewOrder.StartDay;
            ConfirmPriceNewOrder.Text = NewOrder.Price.ToString();
            ChooseDatesForNewOrderGrid.Visibility = Visibility.Hidden;
            ConfirmNewOrderGrid.Visibility = Visibility.Visible;
        }

        private void SaveNewClientBtn1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewOrder.Save();
                OrderCollection.TakeOrders();
                OrderCollection.OrdersToDG(OrderListDataGrid);
                //для продуктов
                ProductCollection.TakeProducts();
                ProductCollection.ProductsToDG(ProductsDataGrid);
                FIONewClientNewOrder.Clear();
                PassNewClientNewOrder.Clear();
                TelNewClientNewOrder.Clear();
                ClientForSearchTextBox.Clear();
                StartDayNewOrder.SelectedDate = null;
                DaysCountNewOrder.Clear();
                ConfirmFIONewOrder.Clear();
                ConfirmPassNewOrder.Clear();
                ConfirmTelNewOrder.Clear();
                ConfirmApartsNewOrder.Clear();
                ConfirmCountNewOrder.Clear();
                ConfirmDataNewOrder.Clear();
                ConfirmPriceNewOrder.Clear();
                NewOrder.Clear();
                ConfirmNewOrderGrid.Visibility = Visibility.Hidden;
                SearchClientForNewOrderGrid.Visibility = Visibility.Visible;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AnyQueryBtn_Click(object sender, RoutedEventArgs e)
        {
            string a = AppDomain.CurrentDomain.BaseDirectory;
            a += @"AnyQuery\AnyQuery.exe";
            Process p = new Process();
            p.StartInfo.FileName = a;
            p.Start();
            Application.Current.Shutdown();
        }

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExcelReport.Create();
                MessageBox.Show("Отчет сформирован!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
