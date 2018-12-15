#region Using

using System.Drawing;

#endregion

namespace Ruf.MazeClient.Entities
{
    /// <summary>
    /// Server current position
    /// </summary>
    /// <seealso cref="Ruf.MazeClient.Entities.ResponseBase" />
    public class CurrentPosition : ResponseBase
    {
        public Point Position { get; set; }


        public static CurrentPosition Failed()
        {
            return new CurrentPosition() {Position = new Point(-1, -1)};
        }
    }
}