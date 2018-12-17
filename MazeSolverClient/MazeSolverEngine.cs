using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Security;
using System.Windows;
using MazeSolverClient.Helpers;
using Ruf.MazeClient;
using Ruf.MazeClient.Entities;
using Point = System.Drawing.Point;

namespace MazeSolverClient
{
    public class MazeSolverEngine
    {
        private readonly MazeClient client;
        private List<CrossInfo> crossPoints = new List<CrossInfo>();
        private CurrentPosition position;

        /// <summary>
        /// Initializes a new instance of the <see cref="MazeSolverEngine"/> class.
        /// </summary>
        /// <param name="mazeClient">The maze client.</param>
        /// <exception cref="ArgumentNullException">client</exception>
        public MazeSolverEngine(MazeClient mazeClient)
        {
            this.client = mazeClient ?? throw new ArgumentNullException(nameof(mazeClient));
        }

        public async Task<bool> SolveAsync()
        {
            CrossInfo crossPoint = null;
            bool success = this.client.Reset();
            if (success)
            {
                this.position = await this.client.GetPositionAsync();
                if (this.position.Success)
                {
                    Directions allowedDirections = await this.client.GetDirectionsAsync();
                    bool isCross = allowedDirections.HasCrossPoints(Direction.Unknown);
                    if (isCross)
                    {
                        crossPoint = new CrossInfo(position.Position, Direction.North);
                        this.crossPoints.Add(crossPoint);
                    }

                    if (allowedDirections.Success)
                    {
                        await this.TraversePathAsync(crossPoint, allowedDirections);






                    }
                }

            }

            return true;

        }

        private async Task TraversePathAsync(CrossInfo crossPoint, Directions directions)
        {
            List<Direction> supportedDirections = directions.ToDirections();
            foreach (Direction directionToFollow in supportedDirections)
            {
                await this.FollowBranchAsync(crossPoint,directionToFollow);
            }
        }

        private async Task FollowBranchAsync(CrossInfo from, Direction directionToFollow)
        {
            //Moves cursor
            while (true)
            {
                await this.client.MoveAsync(directionToFollow);
                this.position = await this.client.GetPositionAsync();

                Directions allowedDirections = await this.client.GetDirectionsAsync();
                bool isCross = allowedDirections.HasCrossPoints(directionToFollow);
                if (isCross)
                {
                    var crossPoint = new CrossInfo(position.Position, Direction.North);
                    if (crossPoint.IsStart)
                    {
                        return;
                    }

                    this.crossPoints.Add(crossPoint);
                    await this.TraversePathAsync(crossPoint, allowedDirections);
                }
                else
                {
                    bool hitEnd = allowedDirections.HitEnd(directionToFollow);
                    if (hitEnd)
                    {
                        if (this.position.Position == from.Point)
                        {
                            return;
                        }

                        //Reverse branch back
                        await this.FollowBranchAsync(from, directionToFollow.Reverse());
                        return;
                    }
                }
            }
        }


        private class CrossInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CrossInfo"/> class.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <param name="from">From.</param>
            public CrossInfo(Point point, Direction from)
            {
                this.Point = point;
                this.From = from;
            }

            /// <summary>
            /// Gets the point.
            /// </summary>
            /// <value>
            /// The point.
            /// </value>
            public Point Point { get; }

            /// <summary>
            /// Gets from.
            /// </summary>
            /// <value>
            /// From.
            /// </value>
            public Direction From { get; }

            public bool IsStart => this.Point.X == 1 && this.Point.Y == 1;
        }
    }
}
