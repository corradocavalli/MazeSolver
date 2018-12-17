#region Using



#endregion

using System.Drawing;

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