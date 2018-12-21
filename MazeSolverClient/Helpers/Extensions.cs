#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using MazeSolverClient.Entities;
using Ruf.MazeClient.Entities;

#endregion

namespace MazeSolverClient.Helpers
{
    /// <summary>
    /// Exposes all required extensions methods
    /// </summary>
    public static class Extensions
    {
        private static readonly Random Rnd = new Random();

        public static Direction GetRandomSide(this List<Side> sides, Direction from)
        {
            sides = sides.Where(s => s.Direction != from).ToList();
            var side = sides.ElementAt(Rnd.Next(0, sides.Count() - 1));
            return side.Direction;
        }

        /// <summary>
        /// Return true if we are at a dead end
        /// </summary>
        /// <param name="directions">The directions.</param>
        /// <param name="from">From.</param>
        /// <returns>True when we are on a dead end, otherwise false</returns>
        public static bool HitEnd(this Directions directions, Direction from)
        {
            return from == Direction.South && !directions.South ||
                   from == Direction.North && !directions.North ||
                   from == Direction.East && !directions.East ||
                   from == Direction.West && !directions.West;
        }

        /// <summary>
        /// Determines whether directions represent a cross point.
        /// </summary>
        /// <param name="directions">The available directions.</param>
        /// <returns>
        /// <c>true</c> if we are on a cross point, otherwise false</c>.
        /// </returns>
        public static bool IsCrossPoint(this Directions directions)
        {
            return (directions.North || directions.South) && (directions.East || directions.West) ||
                   (directions.East || directions.West) && (directions.North || directions.South);
        }

        /// <summary>
        /// Gets the reverse direction of provided one
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns>Reversed direction</returns>
        public static Direction Reverse(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.South;
                case Direction.East:
                    return Direction.West;
                case Direction.South:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                default:
                    return Direction.Unknown;
            }
        }

        /// <summary>
        /// To the directions.
        /// </summary>
        /// <param name="directions">The directions.</param>
        /// <returns>List of directions</returns>
        public static List<Direction> ToDirections(this Directions directions)
        {
            List<Direction> directionList = new List<Direction>();
            if (directions.North) directionList.Add(Direction.North);
            if (directions.South) directionList.Add(Direction.South);
            if (directions.East) directionList.Add(Direction.East);
            if (directions.West) directionList.Add(Direction.West);
            return directionList;
        }
    }
}