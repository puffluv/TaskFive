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
using ChatClient.ServiceChat;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ChatClient.ServiceChat.IServiceChatCallback
    {
        bool isConnected = false;
        ServiceChatClient client;
        int id;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
        }

        void ConnectUser()
        {
            if (!isConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                id =  client.Connect(tbUserName.Text);
                tbMessage.IsEnabled = true;
                tbUserName.IsEnabled = false;
                Welcome.Text = "Добро пожаловать в чат!";
                bConnDiscon.Content = "Disconnect";
                isConnected = true;
                bSend.IsEnabled = true;
            }
        }

        void DisconnectUser()
        {
            client.Disconnect(id);
            client = null;
            tbMessage.IsEnabled = false;
            tbUserName.IsEnabled = true;
            Welcome.Text = "Присоединитесь к чату";
            bSend.IsEnabled = false;
            bConnDiscon.Content = "Connect";
            isConnected = false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        public void MessageCallback(string message)
        {
            lbChat.Items.Add(message);
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count-1]);
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DisconnectUser();
        }
        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (client != null)
                {
                    if (IsMessageValid())
                    {
                        sndMsg();
                    }
                }
            }
        }

        private void bSend_Click(object sender, RoutedEventArgs e)
        {
            if (IsMessageValid())
            {
                sndMsg();
            }
        }
        private void sndMsg()
        {
            client.SendMessage(tbMessage.Text, id);
            tbMessage.Text = string.Empty;
        }
        private bool IsMessageValid()
        {
            return !string.IsNullOrWhiteSpace(tbMessage.Text) && tbMessage.Text != "Введите текст";
        }
        private void tbMessage_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbMessage.Text == "Введите текст")
            {
                tbMessage.Text = "";
                tbMessage.Foreground = Brushes.White;
            }
        }

        private void tbMessage_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbMessage.Text == "Введите текст")
            {
                // Если поле ввода содержит текст "Введите текст", просто вернем его
                return;
            }

            // В противном случае, если поле ввода пустое, восстановим текст "Введите текст"
            if (string.IsNullOrWhiteSpace(tbMessage.Text))
            {
                tbMessage.Text = "Введите текст";
                tbMessage.Foreground = Brushes.LightGray;
            }
        }
        private void tbUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbUserName.Text == "Nickname")
            {
                tbUserName.Text = "";
                tbUserName.Foreground = Brushes.White;
            }
        }

        private void tbUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbUserName.Text))
            {
                tbUserName.Text = "Nickname";
                tbUserName.Foreground = Brushes.LightGray;
            }
        }

    }
}
