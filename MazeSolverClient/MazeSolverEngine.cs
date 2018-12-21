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
        public event EventHandler<SolvingEventArgs> EngineStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="MazeSolverEngine"/> class.
        /// </summary>
        /// <param name="mazeClient">The maze client.</param>
        /// <exception cref="ArgumentNullException">client</exception>
        public MazeSolverEngine(MazeClient mazeClient)
        {
            this.client = mazeClient ?? throw new ArgumentNullException(nameof(mazeClient));
        }

        /// <summary>
        /// Solves the maze.
        /// </summary>
        /// <returns></returns>
        public async Task SolveAsync()
        {
            this.crossPoints.Clear();
            this.client.Reset();

            this.position = await this.client.GetPositionAsync();
            CrossPoint crossPoint = await this.GetCrossPointAsync(this.position.Position);
            if (crossPoint != null)
            {
                Direction direction = crossPoint.ChooseCrossDirection(Direction.Unknown);
                await this.TraverseBranchAsync(direction);
            }
        }

        /// <summary>
        /// Traverses the branch.
        /// </summary>
        /// <param name="directionToFollow">The direction to follow.</param>
        /// <returns>True when target reached</returns>
        private async Task<bool> TraverseBranchAsync(Direction directionToFollow)
        {
            while (true)
            {
                //Move position
                await this.client.MoveAsync(directionToFollow);
                //Get state of the new position
                var state = await this.client.GetStateAsync();
                //Report current status
                this.position = await this.client.GetPositionAsync();

                //HACK: Looks like server never returns TargetReached state, we simulated it base on current position
                bool targetReached= this.position.Position.X == 19 && this.position.Position.Y == 21;
                
                //If target reached, stop traversing
                if (state.Value == StateValue.TargetReached || targetReached)
                {
                    this.OnEngineStatus(new SolvingEventArgs(StateValue.TargetReached, this.position.Position));
                    return true;
                }
                else
                {
                    this.OnEngineStatus(new SolvingEventArgs(state.Value, this.position.Position));
                }

                //Are we on a cross point?
                var crossPoint = await this.GetCrossPointAsync(this.position.Position);
                if (crossPoint != null)
                {
                    //Get the best direction to follow at the cross point and follow it recursively
                    var direction = crossPoint.ChooseCrossDirection(directionToFollow);
                    bool reached= await this.TraverseBranchAsync(direction);
                    if (reached) return true;
                }
                else
                {
                    //Keep following the branch taking care of optional deviations
                    Directions supportedDirections = await this.client.GetDirectionsAsync();
                    directionToFollow = this.GetNextDirection(supportedDirections, directionToFollow);
                    //If we hit the end of the branch, we turn back
                    bool hitEnd = supportedDirections.HitEnd(directionToFollow);
                    if (hitEnd)
                    {
                        bool reached= await this.TraverseBranchAsync(directionToFollow.Reverse());
                        if (reached) return true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the next direction asynchronous.
        /// </summary>
        /// <param name="availableDirections">The available directions to follow.</param>
        /// <param name="from">Direction we're coming from</param>
        /// <returns>Available direction</returns>
        /// <remarks>Allow traversing taking care of optional corners</remarks>
        private Direction GetNextDirection(Directions availableDirections, Direction from)
        {
            List<Direction> directions = availableDirections.ToDirections();
            if (directions.Contains(from))
            {
                return from;
            }

            return directions.First();
        }

        /// <summary>
        /// Gets the cross point for provided position or creates a new one if it doesn't exist.
        /// </summary>
        /// <param name="position">The current maze position.</param>
        /// <returns>An existing crosspoint or a brand new one if not already hit, null if current position is not over any cross point</returns>
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

        protected virtual void OnEngineStatus(SolvingEventArgs e)
        {
            this.EngineStatus?.Invoke(this, e);
        }
    }
}