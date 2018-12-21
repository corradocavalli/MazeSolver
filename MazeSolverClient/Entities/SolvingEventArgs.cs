#region Using

using System;
using System.Drawing;
using Ruf.MazeClient.Entities;

#endregion

namespace MazeSolverClient.Entities
{
    public sealed class SolvingEventArgs : EventArgs
    {
        internal SolvingEventArgs(StateValue state, Point position)
        {
            this.Position = position;
            this.ProgressState = state;
        }
        public Point Position { get; }
        public StateValue ProgressState { get; }
    }
}