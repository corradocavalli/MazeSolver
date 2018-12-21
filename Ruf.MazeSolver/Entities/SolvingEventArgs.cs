#region Using

using System;
using System.Drawing;
using Ruf.MazeClient.Entities;

#endregion

namespace Ruf.MazeSolver.Entities
{
    /// <summary>
    /// Contains information relative to maze solving process
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public sealed class SolvingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolvingEventArgs"/> class.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="position">The position.</param>
        internal SolvingEventArgs(StateValue state, Point position, int tentative)
        {
            this.Position = position;
            this.Moves = tentative;
            this.ProgressState = state;
        }
        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Point Position { get; }

        /// <summary>
        /// Gets the moves count.
        /// </summary>
        /// <value>
        /// The tentative.
        /// </value>
        public int Moves { get; }

        /// <summary>
        /// Gets the state of the progress.
        /// </summary>
        /// <value>
        /// The state of the progress.
        /// </value>
        public StateValue ProgressState { get; }
    }
}