#region Using

using System;

#endregion

namespace Ruf.MazeSolver
{
    /// <summary>
    /// Provides a maze solver instance
    /// </summary>
    public class MazeSolverFactory
    {
        private readonly MazeClient.MazeClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="MazeSolverFactory"/> class.
        /// </summary>
        /// <param name="client">The communication client.</param>
        /// <exception cref="System.ArgumentNullException">client</exception>
        public MazeSolverFactory(MazeClient.MazeClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Creates the maze solver.
        /// </summary>
        /// <returns>Maze solver instance</returns>
        public MazeSolver CreateSolver()
        {
            //We can return specific or better maze solving implementations in future...
            return new TremauxMazeSolver(this.client);
        }
    }
}