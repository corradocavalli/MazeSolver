#region Using

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using MazeSolverClient.Helpers;
using Ruf.MazeClient;
using Ruf.MazeClient.Entities;

#endregion

namespace MazeSolverClient
{
    //http://www.manuelmarangoni.it/onemind/5804/come-uscire-da-un-labirinto-senza-perdersi-il-metodo-di-tremaux/
    //https://de.wikipedia.org/wiki/Tr%C3%A9maux%E2%80%99_Methode

    public partial class MazeSolverEngine
    {
        private readonly MazeClient client;
        private readonly List<Entities.MazeSolverEngine.CrossInfo> crossPoints = new List<Entities.MazeSolverEngine.CrossInfo>();
        private CurrentPosition position;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entities.MazeSolverEngine"/> class.
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
            bool success = this.client.Reset();
            if (success)
            {
                this.position = await this.client.GetPositionAsync();
                var crossPoint = await this.GetCrossPointAsync(this.position.Position);
                if (crossPoint != null)
                {
                    var direction = crossPoint.ChooseCrossDirection(Direction.Unknown);
                    await this.TraversePathAsync(crossPoint, crossPoint.);
                }
            }

            return true;
        }

        private async Task FollowBranchAsync(Entities.MazeSolverEngine.CrossInfo from, Direction directionToFollow)
        {
            //Moves cursor
            while (true)
            {
                await this.client.MoveAsync(directionToFollow);
                this.position = await this.client.GetPositionAsync();
                Directions allowedDirections = await this.client.GetDirectionsAsync();

                bool isCross = allowedDirections.IsCrossPoints(directionToFollow);
                if (isCross)
                {
                    (Entities.MazeSolverEngine.CrossInfo crossInfo, bool exist) = this.GetCrossPoint(this.position.Position);
                    crossInfo.Enter(directionToFollow);

                    //System.Diagnostics.Debug.WriteLine($"Cross-{this.position.Position.X}-{this.position.Position.Y}->N:{allowedDirections.North}/S:{allowedDirections.South}/E{allowedDirections.East}/W:{allowedDirections.West}");

                    //allowedDirections = this.GetAllowedDirections(crossInfo, allowedDirections);

                    await this.TraversePathAsync(crossInfo, allowedDirections);
                }
                else
                {
                    bool hitEnd = allowedDirections.HitEnd(directionToFollow);
                    if (hitEnd)
                    {
                        //Reverse branch back
                        await this.FollowBranchAsync(from, directionToFollow.Reverse());
                    }
                }
            }
        }

        private Directions GetAllowedDirections(Entities.MazeSolverEngine.CrossInfo crossInfo, Directions allowedDirections)
        {
            if (allowedDirections.North && crossInfo.NorthMarks == 2) allowedDirections.North = false;
            if (allowedDirections.South && crossInfo.SouthMarks == 2) allowedDirections.South = false;
            if (allowedDirections.West && crossInfo.WestMarks == 2) allowedDirections.West = false;
            if (allowedDirections.East && crossInfo.EastMarks == 2) allowedDirections.East = false;

            return allowedDirections;
        }

        /// <summary>
        /// Gets the cross point for provided position or creates a new one if it doesn't exist.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        private async Task<Entities.MazeSolverEngine.CrossInfo> GetCrossPointAsync(Point position)
        {
            //Does the cross point already exist?
            var point = this.crossPoints.FirstOrDefault(p => p.Point == this.position.Position);
            if (point != null)
            {
                return point;
            }

            //Creates a brand new cross info and return it
            Directions supportedDirections = await this.client.GetDirectionsAsync();
            bool isCross = supportedDirections.IsCrossPoints(Direction.Unknown);
            if (isCross)
            {
                Entities.MazeSolverEngine.CrossInfo info = new Entities.MazeSolverEngine.CrossInfo(position, supportedDirections);
                this.crossPoints.Add(info);
                return info;
            }

            return null;
        }

        private async Task TraversePathAsync(Entities.MazeSolverEngine.CrossInfo crossPoint, Directions directions)
        {
            List<Direction> supportedDirections = directions.ToDirections();
            foreach (Direction directionToFollow in supportedDirections)
            {
                if (crossPoint.CanGo(directionToFollow))
                {
                    crossPoint.Leave(directionToFollow);
                    await this.FollowBranchAsync(crossPoint, directionToFollow);
                }
            }
        }
    }
}