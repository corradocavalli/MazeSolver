#region Using

using System;
using System.Threading.Tasks;
using RestSharp;
using Ruf.MazeClient.Entities;

#endregion

namespace Ruf.MazeClient
{
    /// <summary>
    /// Allows client application to interact with MazeServer
    /// </summary>
    /// <seealso cref="object" />
    public class MazeClient
    {
        private readonly RestClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="MazeClient"/> class.
        /// </summary>
        /// <param name="serverUri">The server URI.</param>
        /// <exception cref="ArgumentException">Value cannot be null or whitespace. - serverUri</exception>
        public MazeClient(string serverUri)
        {
            if (string.IsNullOrWhiteSpace(serverUri)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(serverUri));
            this.client = new RestClient {BaseUrl = new Uri(serverUri)};
        }

        /// <summary>
        /// Gets the available directions.
        /// </summary>
        /// <returns>Server available directions</returns>
        public async Task<Directions> GetDirectionsAsync()
        {
            try
            {
                RestRequest request = new RestRequest("/directions");
                var response = await this.client.GetAsync<Directions>(request);
                response.Success = true;
                return response;
            }
            catch
            {
                return Directions.Failed();
            }
        }

        /// <summary>
        /// Gets the current position.
        /// </summary>
        /// <returns>Server current position</returns>
        public async Task<CurrentPosition> GetPositionAsync()
        {
            try
            {
                RestRequest request = new RestRequest("/position");
                var response = await this.client.GetAsync<CurrentPosition>(request);
                response.Success = true;
                return response;
            }
            catch
            {
                return CurrentPosition.Failed();
            }
        }

        /// <summary>
        /// Gets the server state.
        /// </summary>
        /// <returns>Current state of the server</returns>
        public async Task<State> GetStateAsync()
        {
            try
            {
                RestRequest request = new RestRequest("/state");
                State response = await this.client.GetAsync<State>(request);
                response.Success = true;
                return response;
            }
            catch
            {
                return State.Failed();
            }
        }

        /// <summary>
        /// Moves the position to provided direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns>True when command succeeded otherwise false</returns>
        public async Task<bool> MoveAsync(Direction direction)
        {
            try
            {
                RestRequest request = new RestRequest("/move", Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };

                request.AddBody(direction);
                await this.client.PostAsync<Direction>(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Resets server position
        /// </summary>
        /// <returns>True when command succeeded otherwise false</returns>
        public bool Reset()
        {
            try
            {
                RestRequest request = new RestRequest("/reset", Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };

                this.client.Post(request);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}