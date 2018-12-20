#region Using

using System;
using Ruf.MazeClient.Entities;

#endregion

namespace MazeSolverClient.Entities
{
    public class Side
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Side" /> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public Side(Direction direction)
        {
            if (direction == Direction.Unknown) throw new ArgumentOutOfRangeException(nameof(direction), "Unknown is not a valid value for side direction.");
            this.Direction = direction;
        }

        public Direction Direction { get; }

        public int Marks { get; set; }
    }
}