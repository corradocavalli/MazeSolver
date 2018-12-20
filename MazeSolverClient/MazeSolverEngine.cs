#region Using

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using MazeSolverClient.Entities;
using MazeSolverClient.Helpers;
using Ruf.MazeClient;
using Ruf.MazeClient.Entities;

#endregion

namespace MazeSolverClient
{
    //http://www.manuelmarangoni.it/onemind/5804/come-uscire-da-un-labirinto-senza-perdersi-il-metodo-di-tremaux/
    //https://de.wikipedia.org/wiki/Tr%C3%A9maux%E2%80%99_Methode

    public class MazeSolverEngine
    {
        private readonly MazeClient client;
        private readonly List<CrossPoint> crossPoints = new List<CrossPoint>();
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
            this.crossPoints.Clear();
            this.client.Reset();

            this.position = await this.client.GetPositionAsync();

           
            var crossPoint = await this.GetCrossPointAsync(this.position.Position);
            if (crossPoint != null)
            {
                var direction = crossPoint.ChooseCrossDirection(Direction.Unknown, false);
                await this.FollowBranchAsync(direction, false);
            }


            return true;
        }

        private async Task FollowBranchAsync(Direction directionToFollow, bool isBackward)
        {
            //Moves cursor
            while (true)
            {
                await this.client.MoveAsync(directionToFollow);
                this.position = await this.client.GetPositionAsync();
                var crossPoint = await this.GetCrossPointAsync(this.position.Position);
                if (crossPoint != null)
                {
                    var direction = crossPoint.ChooseCrossDirection(directionToFollow, isBackward);
                    await this.FollowBranchAsync(direction, false);
                }
                else
                {
                    Directions supportedDirections = await this.client.GetDirectionsAsync();
                    bool hitEnd = supportedDirections.HitEnd(directionToFollow);
                    if (hitEnd)
                    {
                        //Reverse branch back
                        await this.FollowBranchAsync(directionToFollow.Reverse(), true);
                    }
                    else
                    {
                        directionToFollow = await this.GetNextDirectionAsync(directionToFollow);
                    }
                }
            }
        }

        private async Task<Direction> GetNextDirectionAsync(Direction from)
        {
            Directions supportedDirections = await this.client.GetDirectionsAsync();
            var directions = supportedDirections.ToDirections();
            if (directions.Contains(from))
            {
                return from;
            }

            return directions.First();
        }


        /// <summary>
        /// Gets the cross point for provided position or creates a new one if it doesn't exist.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        private async Task<CrossPoint> GetCrossPointAsync(Point position)
        {
            //Does the cross point already exist?
            var point = this.crossPoints.FirstOrDefault(p => p.Position == this.position.Position);
            if (point != null)
            {
                return point;
            }

            //Creates a brand new cross info and return it
            Directions supportedDirections = await this.client.GetDirectionsAsync();
            bool isCross = supportedDirections.IsCrossPoint();
            if (isCross)
            {
                CrossPoint crossPoint = new CrossPoint(position, supportedDirections);
                this.crossPoints.Add(crossPoint);
                return crossPoint;
            }

            return null;
        }
    }
}