using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruf.MazeClient.Entities;

namespace MazeSolverClient.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Determines whether directions represent a cross point.
        /// </summary>
        /// <param name="directions">The directions.</param>
        /// <returns>
        ///   <c>true</c> if [has cross points] [the specified directions]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCrossPoint(this Directions directions)
        {
            return (directions.North || directions.South) && (directions.East || directions.West) || 
                   (directions.East || directions.West) && (directions.North || directions.South);

        }

        public static bool HitEnd(this Directions directions, Direction from)
        {
            return from == Direction.South && !directions.South ||
                   from == Direction.North && !directions.North ||
                   from == Direction.East && !directions.East ||
                   from == Direction.West && !directions.West;
        }

        public static List<Direction> ToDirections(this Directions directions)
        {
            List<Direction> directionList = new List<Direction>();
            if (directions.North) directionList.Add(Direction.North);
            if (directions.South) directionList.Add(Direction.South);
            if (directions.East) directionList.Add(Direction.East);
            if (directions.West) directionList.Add(Direction.West);
            return directionList;
        }

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
    }
}
