using TeamChatClient.Properties;
using TeamChatInterfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeamChatClient
{
    public partial class MainWindow : Window
    {
        public static ITeamChatService Server;

        private static DuplexChannelFactory<ITeamChatService> _channelFactory;

        private Paragraph paragraph;

        Hashtable emoticons;

        public MainWindow()
        {
            InitializeComponent();
            passwordBox.MaxLength = 10;

            SetLanguageDictionary();
            CreateEmoticons();

            

            this.paragraph = new Paragraph();
            TextDisplay.Document = new FlowDocument(paragraph);
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri("..\\Resources\\string_eneu.xaml",
                                  UriKind.Relative);
                    break;
                case "de-DE":
                    dict.Source = new Uri("..\\Resources\\string_de.xaml",
                                       UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\Resources\\string_eneu.xaml",
                                      UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);

        }

        public void TakeMessage(string message, string userName)
        {
            if (userName == UserNameTextBox.Text)
            {
                var from = userName;
                var text = message;

                if (lastOne.Text != userName)
                {
                    paragraph.Inlines.Add(new Bold(new Run("[" + from + "]:\n"))
                    {
                        Foreground = Brushes.Red
                    });
                }
                paragraph.Inlines.Add(text);
                paragraph.Inlines.Add(new LineBreak());
                lastOne.Text = userName;
                
                TextDisplay.ScrollToEnd();
            }
            else if (userName == "SERVER")
            {
                var from = userName;
                var text = message;
                if (lastOne.Text != userName)
                {
                    paragraph.Inlines.Add(new Bold(new Italic(new Run("[" + from + "]: " + text)))
                    {
                        Foreground = Brushes.MediumPurple
                    });
                }
                paragraph.Inlines.Add(new LineBreak());
                lastOne.Text = userName;
                TextDisplay.ScrollToEnd();
            }
            else
            {
                var from = userName;
                var text = message;
                if (lastOne.Text != userName)
                {
                    paragraph.Inlines.Add(new Bold(new Run("[" + from + "]:\n"))
                    {
                        Foreground = Brushes.Yellow
                    });
                }
                paragraph.Inlines.Add(text);
                paragraph.Inlines.Add(new LineBreak());
                lastOne.Text = userName;
                TextDisplay.ScrollToEnd();
            }
        }

        void CreateEmoticons()
        {
            emoticons = new Hashtable(21);
            emoticons.Add(":E", Properties.Resources._1);
            emoticons.Add(":höhö", Properties.Resources._2);
            emoticons.Add(":B", Properties.Resources._3);
            emoticons.Add("--)", Properties.Resources._4);
            emoticons.Add("^-)", Properties.Resources._5);
            emoticons.Add(">.<", Properties.Resources._6);
            emoticons.Add("D:", Properties.Resources._7);
            emoticons.Add(":O", Properties.Resources._8);
            emoticons.Add("oO", Properties.Resources._9);
            emoticons.Add(":C", Properties.Resources._10);
            emoticons.Add(":/", Properties.Resources._11);
            emoticons.Add("-.-", Properties.Resources._12);
            emoticons.Add(":)", Properties.Resources._13);
            emoticons.Add(":P", Properties.Resources._14);
            emoticons.Add("oÔ", Properties.Resources._15);
            emoticons.Add(":F", Properties.Resources._16);
            emoticons.Add("^.^", Properties.Resources._17);
            emoticons.Add(";/", Properties.Resources._18);
            emoticons.Add("._.", Properties.Resources._19);
            emoticons.Add("8D", Properties.Resources._20);
            emoticons.Add(":D", Properties.Resources._21);
        }
        
        internal void TakeWhisper(string message, string reciever, string sender)
        {
            if (UserNameTextBox.Text.ToLower() == reciever.ToLower())
            {
                var from = sender;
                var to = reciever;
                var text = message;

                string whis1 =  (string)Resources["whisperGet"];

                paragraph.Inlines.Add(new Italic(new Run(from + " " + whis1 + ": " + text))
                {
                    Foreground = Brushes.MediumPurple
                });
                paragraph.Inlines.Add(new LineBreak());
                TextDisplay.ScrollToEnd();
            }
            else if (UserNameTextBox.Text == sender)
            {
                var from = sender;
                var to = reciever;
                var text = message;

                string whis2 = (string)Resources["whisperTo"];

                paragraph.Inlines.Add(new Italic(new Run(whis2 +" " + reciever +": " + text))
                {
                    Foreground = Brushes.MediumPurple
                });
                paragraph.Inlines.Add(new LineBreak());
                TextDisplay.ScrollToEnd();
            }
        }

        public void TakeOnOff(string userName, int status)
        {
            if (status == 1)   // login
            {
                var from = userName;

                string enter = (string)Resources["enter"];

                paragraph.Inlines.Add(new Bold(new Italic(new Run(from + " " + enter)))
                {
                    Foreground = Brushes.Orange
                });
                paragraph.Inlines.Add(new LineBreak());
                TextDisplay.ScrollToEnd();
            }
            else if (status == 0)  // logout
            {
                var from = userName;

                string leave = (string)Resources["leave"];

                paragraph.Inlines.Add(new Bold(new Italic(new Run(from + " " + leave)))
                {
                    Foreground = Brushes.Orange
                });
                paragraph.Inlines.Add(new LineBreak());
                TextDisplay.ScrollToEnd();

            }
            else if (status == 500)  // POKE
            {
                var from = userName;

                string shake = (string)Resources["poke"];

                paragraph.Inlines.Add(new Bold(new Italic(new Run("@@@: " + from + " " + shake)))
                {
                    Foreground = Brushes.Orange
                });
                paragraph.Inlines.Add(new LineBreak());
                TextDisplay.ScrollToEnd();
            }
        }
        public void TakeType(string userName, int type)
        {
            if (type == 1)  // start typing
            {
                TypeLabel.Text = userName + " is typing...";
            }
            else if (type == 0)  // stop typing
            {
                TypeLabel.Text = "";
            }
        }
  
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            loginFrameBorder.Visibility = Visibility.Visible;
            UserNameTextBox.Visibility = Visibility.Visible;
            passwordBox.Visibility = Visibility.Visible;
            loginFrameButtonGo.Visibility = Visibility.Visible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (LogOutButton.Visibility != Visibility.Hidden)
            {
                try
                {
                    Server.SendOnOff(UserNameTextBox.Text, 0);
                    Server.Logout();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
        }
        public void AddUserToList(string userName)
        {
            if (UserListBox.Items.Contains(userName))
            {
                return;
            }
            UserListBox.Items.Add(userName);
        }
        public void RemoveUserFromList(string userName)
        {
            if (UserListBox.Items.Contains(userName))
            {
                UserListBox.Items.Remove(userName);
            }
        }
        private void LoaduserList(List<string> users)
        {
            foreach(var user in users)
            {
                AddUserToList(user);
            }
        }

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordBox.Text = "";
            passBoxLabel.Visibility = Visibility.Hidden;
        }

        private void UserNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UserNameTextBox.Text = "";
            userBoxLabel.Visibility = Visibility.Hidden;
        }

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _login();
            }
        }

        private void UserNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _login();
            }
        }

        private void LogOutButton_MouseEnter(object sender, MouseEventArgs e)
        {
           
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            _logout();
        }


        private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // try
            // {
            // if (MessageTextBox.Text.Length == 1)
            // {
            // Server.IsTyping(UserNameTextBox.Text, 1);
            // }
            // else if (MessageTextBox.Text.Length > 1)
            // {
            // }
            // else if (MessageTextBox.Text.Length == 0)
            // {
            // Server.IsTyping(UserNameTextBox.Text, 0);
            // }
            //   }
            // catch
            // {
            //   MessageBox.Show("Connection lost. The server is not responding anymore.");
            //}
        }

        private void MessageTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var tb = (sender as TextBox);

            if (Keyboard.Modifiers == ModifierKeys.Control && Keyboard.IsKeyDown(Key.Enter))
            {
                tb.Text += "\r\n";
                tb.SelectionStart = tb.Text.Length;

                e.Handled = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift && Keyboard.IsKeyDown(Key.Enter))
            {
                tb.Text += "\r\n";
                tb.SelectionStart = tb.Text.Length;

                e.Handled = true;
            }
            else if (Keyboard.IsKeyDown(Key.Enter))
            {
                var textBinding = BindingOperations.GetBindingExpression(
                    tb, TextBox.TextProperty);

                if (textBinding != null)
                    textBinding.UpdateSource();

                if (MessageTextBox.Text.Length > 0 && MessageTextBox.Text != "")
                {
                    string _msg = MessageTextBox.Text;
                    string[] _explodedMsg = _msg.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    e.Handled = true;
                    if (_explodedMsg[0] == "/w")
                    {

                        try
                        {
                            string _cmd = _explodedMsg[0];
                            string _rec = _explodedMsg[1];

                            MessageTextBox.Visibility = Visibility.Hidden;
                            whispLabel.Visibility = Visibility.Visible;
                            whispBox1.Visibility = Visibility.Visible;
                            whispBox2.Visibility = Visibility.Visible;
                            MessageTextBox.Text = "";
                            Keyboard.Focus(whispBox2);
                            whispBox1.Text = _rec;
                        }
                        catch
                        {
                            startWhispFromChat();
                        }
                    }
                    else
                    {
                        _sendMsg();
                    }
                }
            }
            else if (e.Key == Key.Space)
            {
                if (MessageTextBox.Text.Length > 0 && MessageTextBox.Text != "")
                {
                    string _msg = MessageTextBox.Text;
                    string[] _explodedMsg = _msg.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (_explodedMsg[0] == "/w")
                    {

                        try
                        {
                            string _cmd = _explodedMsg[0];
                            string _rec = _explodedMsg[1];
                            if (_rec != "")
                            {
                                MessageTextBox.Visibility = Visibility.Hidden;
                                whispLabel.Visibility = Visibility.Visible;
                                whispBox1.Visibility = Visibility.Visible;
                                whispBox2.Visibility = Visibility.Visible;
                                MessageTextBox.Text = "";
                                Keyboard.Focus(whispBox2);
                                whispBox1.Text = _rec;
                            }
                        }
                        catch
                        {
                            //nothing
                        }
                    }
                }
            }
        }

        private void whispBox2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var tb = (sender as TextBox);

            if (Keyboard.Modifiers == ModifierKeys.Control && Keyboard.IsKeyDown(Key.Enter))
            {
                tb.Text += "\r\n";
                tb.SelectionStart = tb.Text.Length;

                e.Handled = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift && Keyboard.IsKeyDown(Key.Enter))
            {
                tb.Text += "\r\n";
                tb.SelectionStart = tb.Text.Length;

                e.Handled = true;
            }
            else if (Keyboard.IsKeyDown(Key.Enter))
            {
                var textBinding = BindingOperations.GetBindingExpression(
                    tb, TextBox.TextProperty);

                if (textBinding != null)
                    textBinding.UpdateSource();

                e.Handled = true;
                _sendWhisp();
            }
            else if (Keyboard.IsKeyDown(Key.Escape))
            {
                var textBinding = BindingOperations.GetBindingExpression(
                    tb, TextBox.TextProperty);

                if (textBinding != null)
                    textBinding.UpdateSource();

                e.Handled = true;
                stopWhisp();
            }

        }

        private void _login()
        {
            InitializeComponent();
            _channelFactory = new DuplexChannelFactory<ITeamChatService>(new ClientCallBack(), "ChatServiceEndPoint");
            Server = _channelFactory.CreateChannel();

            try
            {

                int returnValue = Server.Login(UserNameTextBox.Text, passwordBox.Text);
                if (returnValue == 1)
                {
                    string loggedAlready = (string)Resources["loggedAlready"];
                    MessageBox.Show(loggedAlready);
                }
                else if (returnValue == 2)
                {
                    string wrongPW = (string)Resources["wrongPW"];
                    MessageBox.Show(wrongPW);
                }
                else if (returnValue == 3)
                {
                    string wrongPW = (string)Resources["wrongPW"];
                    MessageBox.Show(wrongPW);
                }
                else if (returnValue == 0)
                {
                    // MessageBox.Show("Logged in!");
                    UserNameTextBox.IsEnabled = false;
                    passwordBox.IsEnabled = false;
                    DisableBorder.Visibility = Visibility.Hidden;
                    savePassBox.Visibility = Visibility.Hidden;
                    savePassLabel.Visibility = Visibility.Hidden;
                    userBoxLabel.Visibility = Visibility.Hidden;
                    passBoxLabel.Visibility = Visibility.Hidden;

                    UserNameTextBox.Visibility = Visibility.Hidden;
                    passwordBox.Visibility = Visibility.Hidden;
                    loginFrameBorder.Visibility = Visibility.Hidden;
                    loginFrameButtonGo.Visibility = Visibility.Hidden;
                    TextDisplay.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xA8, 0xCC, 0xFF));
                    
                    LogOutButton.Visibility = Visibility.Visible;

                    Server.SendOnOff(UserNameTextBox.Text, 1);
                    TakeOnOff(UserNameTextBox.Text, 1);

                    LoaduserList(Server.getCurrentUsers());
                    Keyboard.Focus(MessageTextBox);

                    if (savePassBox.IsChecked == true)
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        Properties.Settings.Default.User = UserNameTextBox.Text;
                        Properties.Settings.Default.Password = passwordBox.Text;
                        Properties.Settings.Default.Save();
                        ConfigurationManager.RefreshSection("userSettings");
                    }
                }
            }
            catch
            {
                string noCon = (string)Resources["noCon"];
                MessageBox.Show(noCon);
            }
        }

        private void _logout()
        {
            string loggedOut = (string)Resources["loggedOut"];
            try
            {
                UserNameTextBox.IsEnabled = true;
                passwordBox.IsEnabled = true;
                LoginButton.IsEnabled = true;
                DisableBorder.Visibility = Visibility.Visible;
                savePassBox.Visibility = Visibility.Visible;
                savePassLabel.Visibility = Visibility.Visible;
                userBoxLabel.Visibility = Visibility.Visible;
                passBoxLabel.Visibility = Visibility.Visible;

                UserNameTextBox.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Visible;
                loginFrameBorder.Visibility = Visibility.Visible;
                loginFrameButtonGo.Visibility = Visibility.Visible;
                TextDisplay.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x74, 0x74, 0x74));

                LogOutButton.Visibility = Visibility.Hidden;

                Server.SendOnOff(UserNameTextBox.Text, 0);
                Server.Logout();

                FadeInBox.Text = loggedOut;
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 0.0;
                animation.To = 0.8;
                animation.Duration = TimeSpan.FromSeconds(5);
                animation.AutoReverse = true;
                FadeInBox.BeginAnimation(UIElement.OpacityProperty, animation);

                UserListBox.Items.Clear();

                if (savePassBox.IsChecked == false)
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    Properties.Settings.Default.User = "";
                    Properties.Settings.Default.Password = "";
                    Properties.Settings.Default.Save();
                    ConfigurationManager.RefreshSection("userSettings");
                }
            }
            catch
            {
                UserNameTextBox.IsEnabled = true;
                passwordBox.IsEnabled = true;
                LoginButton.IsEnabled = true;

                UserNameTextBox.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Visible;
                loginFrameBorder.Visibility = Visibility.Visible;
                loginFrameButtonGo.Visibility = Visibility.Visible;
                savePassBox.Visibility = Visibility.Visible;
                savePassLabel.Visibility = Visibility.Visible;
                userBoxLabel.Visibility = Visibility.Visible;
                passBoxLabel.Visibility = Visibility.Visible;

                TextDisplay.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x74, 0x74, 0x74));

                LogOutButton.Visibility = Visibility.Hidden;

                FadeInBox.Text = loggedOut;
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 0.0;
                animation.To = 0.8;
                animation.Duration = TimeSpan.FromSeconds(5);
                animation.AutoReverse = true;
                FadeInBox.BeginAnimation(UIElement.OpacityProperty, animation);

                UserListBox.Items.Clear();
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        private void loginFrameButtonGo_Click(object sender, RoutedEventArgs e)
        {
            _login();
        }

        private void _sendMsg()
        {
            if (MessageTextBox.Text.Length == 0)
            {
                return;
            }
            if (UserNameTextBox.IsEnabled == true)
            {
                return;
            }
            try
            {
                if (string.IsNullOrWhiteSpace(MessageTextBox.Text))
                {
                    string uMad = (string)Resources["uMad"];
                    MessageBox.Show(uMad);
                    MessageTextBox.Text = "";
                }
                else
                {
                    MessageTextBox.Text = Regex.Replace(MessageTextBox.Text, "\\s+\r\n", "\r\n");
                    MessageTextBox.Text = Regex.Replace(MessageTextBox.Text, "\r\n\r\n", "\r\n");
                    Server.SendMessageToALL(MessageTextBox.Text, UserNameTextBox.Text);
                    TakeMessage(MessageTextBox.Text, UserNameTextBox.Text);
                    MessageTextBox.Text = "";
                }
            }
            catch
            {
                _logout();
            }
        }

        private void _sendWhisp()
        {

            if (whispBox2.Text.Length == 0)
            {
                return;
            }
            if (UserNameTextBox.IsEnabled == true)
            {
                return;
            }
            if (whispBox1.Text == UserNameTextBox.Text || whispBox1.Text.ToLower() == UserNameTextBox.Text.ToLower() )
            {
                _errorMsg(1);
            }
            else
            {
                bool contains = false;

                for (int i = 0; i < UserListBox.Items.Count; i++)
                {
                    if (UserListBox.Items[i].ToString().ToLower() == whispBox1.Text.ToLower())
                    {
                        contains = true;
                    }

                }

                if (!contains)
                {
                    _errorMsg(2);
                }
                else
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(whispBox2.Text))
                        {
                            string uMad = (string)Resources["uMad"];
                            MessageBox.Show(uMad);
                            whispBox2.Text = "";
                        }
                        else
                        {
                            whispBox2.Text = Regex.Replace(whispBox2.Text, "\\s+\r\n", "\r\n");
                            whispBox2.Text = Regex.Replace(whispBox2.Text, "\r\n\r\n", "\r\n");
                            Server.WhisperToUser(whispBox2.Text, whispBox1.Text, UserNameTextBox.Text);
                            TakeWhisper(whispBox2.Text, whispBox1.Text, UserNameTextBox.Text);
                            whispBox2.Text = "";
                        }                      
                    }
                    catch
                    {
                        _logout();
                    }
                }
            }
        }

        private void pokeMe(object sender, RoutedEventArgs e)
        {
            if (UserListBox.SelectedIndex == -1)
            {
                return;
            }
            string reciever = UserListBox.Items[UserListBox.SelectedIndex].ToString();
            string ownUser = UserNameTextBox.Text;

            if (ownUser != reciever)
            {
                Server.SendPoke(UserNameTextBox.Text, 500, reciever);

                var from = reciever;
                string uPoke = (string)Resources["uPoke"];
                paragraph.Inlines.Add(new Bold(new Italic(new Run("@@@:" + uPoke +": "+ from + "!")))
                {
                    Foreground = Brushes.Orange
                });
                paragraph.Inlines.Add(new LineBreak());
                TextDisplay.ScrollToEnd();
            }
            else
            {
                //return;
                _errorMsg(3);
            }
        }

        private void startWhisp(object sender, RoutedEventArgs e)
        {
            if (UserListBox.SelectedIndex != -1)
            {
                string reciever = UserListBox.Items[UserListBox.SelectedIndex].ToString();
                if (reciever != UserNameTextBox.Text)
                {
                    whispBox1.Text = reciever;
                    MessageTextBox.Visibility = Visibility.Hidden;
                    whispLabel.Visibility = Visibility.Visible;
                    whispBox1.Visibility = Visibility.Visible;
                    whispBox2.Visibility = Visibility.Visible;
                    whispBox2.Text = MessageTextBox.Text;
                    Keyboard.Focus(whispBox2);
                }
                else
                {
                    _errorMsg(1);
                }
            }
            else
            {
                whispBox1.Text = "???";
            }
 
        }

        private void startWhispFromChat()
        {
            MessageTextBox.Visibility = Visibility.Hidden;
            whispLabel.Visibility = Visibility.Visible;
            whispBox1.Visibility = Visibility.Visible;
            whispBox2.Visibility = Visibility.Visible;
            MessageTextBox.Text = "";
            Keyboard.Focus(whispBox1);
            whispBox1.Text = "";
        }
        private void stopWhisp()
        {
            MessageTextBox.Visibility = Visibility.Visible;
            whispLabel.Visibility = Visibility.Hidden;
            whispBox1.Visibility = Visibility.Hidden;
            whispBox1.Text = "";
            whispBox2.Visibility = Visibility.Hidden;
            whispBox2.Text = "";
            MessageTextBox.Text = "";
            Keyboard.Focus(MessageTextBox);
        }

        private void _errorMsg(int value)
        {
            if (value == 1)
            {
                string notUwhisp = (string)Resources["notUwhisp"];
                paragraph.Inlines.Add(new Italic(new Run(notUwhisp))
                {
                    Foreground = Brushes.Red
                });
                paragraph.Inlines.Add(new LineBreak());
                TextDisplay.ScrollToEnd();
                whispBox2.Text = "";
            }
            else if (value == 2)
            {
                string notOnline = (string)Resources["notOnline"];
                paragraph.Inlines.Add(new Italic(new Run(notOnline))
                {
                    Foreground = Brushes.Red
                });
                paragraph.Inlines.Add(new LineBreak());
                TextDisplay.ScrollToEnd();
                whispBox2.Text = "";
                whispBox2.Text = "";
            }
            else if (value == 3)
            {
                string notUpoke = (string)Resources["notUpoke"];
                paragraph.Inlines.Add(new Italic(new Run(notUpoke))
                {
                    Foreground = Brushes.Red
                });
                paragraph.Inlines.Add(new LineBreak());
                TextDisplay.ScrollToEnd();
            }
            else if (value == 4)
            {

            }
        }

        private void whispBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.Focus(whispBox2);
            }
        }

        private void UserNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UserNameTextBox.Text.Length == 0)
            {
                userBoxLabel.Visibility = Visibility.Visible;
            }
        }

        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Text.Length == 0)
            {
                passBoxLabel.Visibility = Visibility.Visible;
            }
        }

    }
}
