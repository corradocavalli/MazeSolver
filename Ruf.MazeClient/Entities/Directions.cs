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
