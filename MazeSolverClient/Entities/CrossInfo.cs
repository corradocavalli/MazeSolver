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
    public partial class MazeSolverEngine
    {
        public class CrossPoint
        {
            private readonly List<Side> sides;
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
                if (availableDirections.North) this.AvailableDirections.Add(Direction.North);
                if (availableDirections.South) this.AvailableDirections.Add(Direction.South);
                if (availableDirections.East) this.AvailableDirections.Add(Direction.East);
                if (availableDirections.West) this.AvailableDirections.Add(Direction.West);

                //Creates default sides
                this.sides = new List<Side>()
                {
                    new Side(Direction.North),
                    new Side(Direction.East),
                    new Side(Direction.South),
                    new Side(Direction.West)
                };

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

            public bool CanGo(Direction directionToFollow)
            {
                switch (directionToFollow)
                {
                    case Direction.North:
                        return this.NorthMarks < 2;
                    case Direction.East:
                        return this.EastMarks < 2;
                    case Direction.South:
                        return this.SouthMarks < 2;
                    case Direction.West:
                        return this.WestMarks < 2;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(directionToFollow), directionToFollow, null);
                }
            }

            private void Mark(Direction direction)
            {
                switch (direction)
                {
                    case Direction.North:
                        this.sides[(int)Direction.North].Marks++;
                        break;
                    case Direction.East:
                        this.sides[(int)Direction.East].Marks++;
                        break;
                    case Direction.South:
                        this.sides[(int)Direction.South].Marks++;
                        break;
                    case Direction.West:
                        this.sides[(int)Direction.West].Marks++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
            }

            public Direction ChooseCrossDirection(Direction from)
            {
                //Never visited cross, choose random one.. (case 1)
                if (this.sides.Sum(side => side.Marks) == 0)
                {
                    int index = this.rnd.Next(0, this.AvailableDirections.Count);
                    Direction leave = this.AvailableDirections[index];
                    this.Mark(from);
                    this.Mark(leave);
                    return leave;
                }

                //Visited 
                if (this.sides.Sum(side => side.Marks) >= 1)
                {
                    var enteringSide = this.sides[(int)from];

                    //Case 2
                    if (enteringSide.Marks == 0)
                    {
                        var leave = from.Reverse();
                        this.Mark(from);
                        this.Mark(leave);
                        return leave;
                    }
                    else
                    {
                        var allowedExits = this.sides.Where(s => s.Direction != from && s.Marks == 0).ToList();
                        if (allowedExits.Any())
                        {
                            //Choose a random one from available unvisited exits (case 3)
                            int index = this.rnd.Next(0, allowedExits.Count());
                            Direction leave = allowedExits[index].Direction;
                            this.Mark(from);
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
                                this.Mark(from);
                                this.Mark(leave);
                                return leave;
                            }
                        }
                    }



                }

                //

            }
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