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
using Ruf.MazeClient;
using Ruf.MazeClient.Entities;

namespace MazeSolverClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MazeClient client;

        public MainWindow()
        {
            InitializeComponent();
            this.client = new MazeClient("http://localhost:3000");
        }

        private async void OnGetState(object sender, RoutedEventArgs e)
        {
            var response = await this.client.GetStateAsync();
        }

        private async void OnMove(object sender, RoutedEventArgs e)
        {
            var response = await this.client.MoveAsync(Direction.East);
        }

        private void OnReset(object sender, RoutedEventArgs e)
        {
            var response= this.client.Reset();
        }

        private async void OnDirections(object sender, RoutedEventArgs e)
        {
            var response = await this.client.GetDirectionsAsync();
        }

        private async void OnPositions(object sender, RoutedEventArgs e)
        {
            var response = await this.client.GetPositionAsync();
        }
    }
}
