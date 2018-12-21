#region Using

using System.Windows;
using Ruf.MazeClient;
using Ruf.MazeClient.Entities;
using Ruf.MazeSolver;
using Ruf.MazeSolver.Entities;

#endregion

namespace MazeSolverClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MazeSolverFactory factory;
        private MazeSolver solver;


        public MainWindow()
        {
            this.InitializeComponent();

            //Initializes communication client and factory
            MazeClient client = new MazeClient("http://localhost:3000");
            this.factory = new MazeSolverFactory(client);
        }

        /// <summary>
        /// Called when progress event is raised.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SolvingEventArgs"/> instance containing the event data.</param>
        private void OnProgress(object sender, SolvingEventArgs e)
        {
            this.ProgressTextBlock.Text = e.ProgressState == StateValue.OnTheWay ? $"Moving to X:{e.Position.X} Y:{e.Position.Y}" : $"Target reached at X:{e.Position.X} Y:{e.Position.Y}";
            this.MovesTextBlock.Text = e.Moves.ToString();
        }


        /// <summary>
        /// Called when solve button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void OnSolve(object sender, RoutedEventArgs e)
        {
            this.MovesTextBlock.Text = null;
            this.SolverButton.IsEnabled = false;

            this.solver = this.factory.CreateSolver();
            this.solver.Progress += this.OnProgress;
            await this.solver.SolveAsync();

            this.solver.Progress -= this.OnProgress;
            this.SolverButton.IsEnabled = true;
        }
    }
}