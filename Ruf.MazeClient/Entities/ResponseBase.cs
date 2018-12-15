namespace Ruf.MazeClient.Entities
{
    /// <summary>
    /// Base response class
    /// </summary>
    public abstract class ResponseBase
    {
        public bool Success { get; internal set; }
    }
}