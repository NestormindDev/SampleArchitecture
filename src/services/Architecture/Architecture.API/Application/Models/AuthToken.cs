using Newtonsoft.Json; 

namespace Architecture.API.Application.Models
{
    /// <summary>
    /// Represents the model that carries the pair of the jwt token and the refresh token from the server to the client and also in the
    /// reverse direction.
    /// </summary>
    public partial class AuthToken
    {
        /// <summary>
        /// Holds the jwt token in either direction in the client-server communication.
        /// </summary>
        [JsonProperty("jwtToken")]
        public string JWTToken { get; set; }

        /// <summary>
        /// Holds the refresh token in either direction in the client-server communication.
        /// </summary>
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
