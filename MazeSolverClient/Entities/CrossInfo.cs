#region Using

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MazeSolverClient.Helpers;
using Ruf.MazeClient.Entities;

#endregion

namespace MazeSolverClient.Entities
{

    public class CrossPoint
    {
        private readonly List<Side> sides=new List<Side>();
        private readonly Random rnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossPoint"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        public CrossPoint(Point position, Directions availableDirections)
        {
            this.Position = position;
            this.AvailableDirections = new List<Direction>();

            //Let's make directions a bit friendly
            if (availableDirections.North)
            {
                this.AvailableDirections.Add(Direction.North);
                this.sides.Add(new Side(Direction.North));
            }

            if (availableDirections.South)
            {
                this.AvailableDirections.Add(Direction.South);
                this.sides.Add(new Side(Direction.South));
            }

            if (availableDirections.East)
            {
                this.AvailableDirections.Add(Direction.East);
                this.sides.Add(new Side(Direction.East));
            }

            if (availableDirections.West)
            {
                this.AvailableDirections.Add(Direction.West);
                this.sides.Add(new Side(Direction.West));
            }

            this.rnd = new Random();
        }

        public List<Direction> AvailableDirections { get; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Point Position { get; }


        private void Mark(Direction direction)
        {
            if(direction != Direction.Unknown)
            {
                this.sides.First(s => s.Direction == direction).Marks++;
            }
        }

        public Direction ChooseCrossDirection(Direction from, bool isBackward)
        {
            var enter = from.Reverse();

            //Never visited cross, choose random one.. (case 1)
            if (this.sides.Sum(side => side.Marks) == 0)
            {
                int index = this.rnd.Next(0, this.sides.Count - 1);
                Direction leave = this.sides[index].Direction;
                this.Mark(enter);

                this.Mark(leave);
                return leave;
            }

            //Visited 
            if (this.sides.Sum(side => side.Marks) >= 1)
            {
                var enteringSide = this.sides.First(s => s.Direction == enter);

                //Case 2
                if (enteringSide.Marks == 0)
                {
                    var leave = from.Reverse();
                    this.Mark(enter);
                    this.Mark(leave);
                    return leave;
                }
                else
                {
                    var allowedExits = this.sides.Where(s => s.Direction != from && s.Marks == 0).ToList();
                    if (allowedExits.Any())
                    {
                        //Choose a random one from available unvisited exits (case 3)
                        int index = this.rnd.Next(0, allowedExits.Count() - 1);
                        Direction leave = allowedExits[index].Direction;
                        this.Mark(enter);
                        this.Mark(leave);
                        return leave;
                    }
                    else
                    {
                        allowedExits = this.sides.Where(s => s.Direction != from && s.Marks == 1).ToList();
                        if (allowedExits.Any())
                        {
                            //Choose a random one from available unvisited exits (case 4)
                            int index = this.rnd.Next(0, allowedExits.Count());
                            Direction leave = allowedExits[index].Direction;
                            this.Mark(enter);
                            this.Mark(leave);
                            return leave;
                        }
                    }
                }
            }

            return Direction.Unknown;
        }
        private class Side
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

            public int Marks { get; set; }
            public Direction Direction { get; }
        }
    }
}
