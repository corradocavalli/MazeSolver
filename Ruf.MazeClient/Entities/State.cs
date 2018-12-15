using System;
using System.Reflection;

namespace Ruf.MazeClient.Entities
{
    /// <summary>
    /// Represents a response of maze server status
    /// </summary>
    /// <seealso cref="Ruf.MazeClient.Entities.ResponseBase" />
    public class State : ResponseBase
    {
        public StateValue Value { get; internal set; }

        internal static State Failed()
        {
            return new State() {Value = StateValue.Failed};
        }
    }
}