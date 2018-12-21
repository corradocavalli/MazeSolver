#region Using

using System;
using Ruf.MazeClient.Entities;

#endregion

namespace Ruf.MazeSolver.Entities
{
    /// <summary>
    /// Represents a cross point side
    /// </summary>
    internal class Side
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

        /// <summary>
        /// Gets the side direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public Direction Direction { get; }

        /// <summary>
        /// Gets or sets the marks for this side.
        /// </summary>
        /// <value>
        /// The marks.
        /// </value>
        public int Marks { get; set; }
    }
}