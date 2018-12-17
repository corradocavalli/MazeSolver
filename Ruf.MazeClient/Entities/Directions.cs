using System;
using System.Collections.Generic;
using System.Text;

namespace Ruf.MazeClient.Entities
{
    /// <summary>
    /// Represents the available directions on server
    /// </summary>
    /// <seealso cref="Ruf.MazeClient.Entities.ResponseBase" />
    public class Directions: ResponseBase
    {
        public Directions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Directions"/> class.
        /// </summary>
        /// <param name="north">if set to <c>true</c> [north].</param>
        /// <param name="south">if set to <c>true</c> [south].</param>
        /// <param name="east">if set to <c>true</c> [east].</param>
        /// <param name="west">if set to <c>true</c> [west].</param>
        public Directions(bool north, bool south, bool east, bool west)
        {
            this.North = north;
            this.South = south;
            this.East = east;
            this.West = west;
        }

        public bool North { set; get; }
        public bool East { set; get; }
        public bool South { set; get; }
        public bool West { set; get; }

        public static Directions Failed()
        {
            return new Directions();
        }
    }
}
