#region Using

using System;
using System.Threading.Tasks;
using Ruf.MazeSolver.Entities;

#endregion

namespace Ruf.MazeSolver
{
    /// <summary>
    /// Interface implemented by all maze solving engines
    /// </summary>
    public interface IMazeSolver
    {
        /// <summary>
        /// Report maze solving progress
        /// </summary>
        event EventHandler<SolvingEventArgs> Progress;

        /// <summary>
        /// Solves the maze
        /// </summary>
        Task SolveAsync();
    }
}