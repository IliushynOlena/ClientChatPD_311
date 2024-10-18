using System.Collections.ObjectModel;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPEndPoint serverEndPoint;
        UdpClient client ;
        //const string serverAddress = "127.0.0.1";   
        //const short serverPort = 4040;   
        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();   
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = messages; 
            client = new UdpClient();
            string address = ConfigurationManager.AppSettings["ServerAddress"]!;
            short port = short.Parse( ConfigurationManager.AppSettings["ServerPort"]!);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
   
        }

        private void Leave_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Join_Button_Click(object sender, RoutedEventArgs e)
        {
            string message = "$<join>";
            SendMessage(message);
            Listen();
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {           
            string message = msgTextBox.Text;
            SendMessage(message);
        }
        private async void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(data, data.Length, serverEndPoint);
        }
        private async void Listen()
        {
            while (true)
            {
                var data = await client.ReceiveAsync();
                string message = Encoding.UTF8.GetString(data.Buffer);
                messages.Add(new MessageInfo(message, DateTime.Now));
            }
        }
    }
}