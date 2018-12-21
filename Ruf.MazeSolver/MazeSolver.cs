using System;
using System.Threading.Tasks;
using Ruf.MazeSolver.Entities;

namespace Ruf.MazeSolver
{
    /// <summary>
    /// Represents a generic maze solver
    /// </summary>
    /// <seealso cref="Ruf.MazeSolver.IMazeSolver" />
    public abstract class MazeSolver : IMazeSolver
    {
        /// <summary>
        /// Report maze solving progress
        /// </summary>
        public event EventHandler<SolvingEventArgs> Progress;

        /// <summary>
        /// Solves the maze
        /// </summary>
        /// <returns></returns>
        public abstract Task SolveAsync();

        /// <summary>
        /// Raises the <see cref="E:Progress" /> event.
        /// </summary>
        /// <param name="e">The <see cref="SolvingEventArgs"/> instance containing the event data.</param>
        protected virtual void OnEngineStatus(SolvingEventArgs e)
        {
            this.Progress?.Invoke(this, e);
        }
    }
}