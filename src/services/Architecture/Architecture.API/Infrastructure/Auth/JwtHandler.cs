using Architecture.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Review.Code.Configuration; 
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Architecture.API.Infrastructure.Auth
{
    /// <summary>
    /// This class helps us with all the process of the token creation for the JWT based authentication scheme.
    /// </summary>
    public class JwtHandler
    {
        #region --Settings pointers--

        /// <summary>
        /// The generic application configuration from where we need to pull the generic information like encryption key.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The configuration specifically related to the JWT handling.
        /// </summary>
        private readonly JWTSettings _jwtSettings;

        /// <summary>
        /// Gives us the authentication window in minutes. This will be the duration of the valid
        /// refresh token.
        /// </summary>
        public int AuthenticationWindowInMinutes
        {
            get
            {
                return this._jwtSettings.AuthenticationWindowInMinutes;
            }
        }

        #endregion --Settings pointers--

        #region --JWT token generation--

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="configuration">Generic application configuration.</param>
        /// <param name="jwtSettings">JWT specific configuration.</param>
        public JwtHandler(IConfiguration configuration, JWTSettings jwtSettings)
        {
            this._configuration = configuration;
            this._jwtSettings = jwtSettings;
        }

        //Generating singin Credentials
        private SigningCredentials GetSigningCredentials()
        {
            byte[] key = Encoding.UTF8.GetBytes(this._configuration[ConfigurationKeyConstant.JWT_SECRET_KEY]);
            SymmetricSecurityKey secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        }

        /// <summary>
        /// This method generats the token jwt security token using the claims and the signing credentials.
        /// </summary>
        /// <param name="claims">The list of claims to be added to the token.</param>
        /// <param name="signingCredentials">The signing credentials to be used in the </param>
        /// <returns></returns>
        private JwtSecurityToken GenerateJWTSecurityToken(List<Claim> claims, SigningCredentials signingCredentials)
        {
            return (new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials: signingCredentials));
        }

        /// <summary>
        /// This method creates the claims list from the employee object that has all the attributes that go into
        /// the claim.
        /// </summary>
        /// <param name="employee">The user principle object that contains attributes that go into the claims.</param>
        /// <returns>List of claims generated from the employee object.</returns>
        private List<Claim> GetClaims(dynamic employee)
        {
            #region --Employee level claims items--
            List<Claim> claims = new List<Claim>
            {
                new Claim(type: ClaimTypes.NameIdentifier, value: employee.UserGuid.ToString()),
                new Claim(type: GenericConstant.CLAIM_EMPLOYEE_ID, value: employee.EmployeeId.ToString()),
                new Claim(type: GenericConstant.CLAIM_FIRST_NAME, employee.FirstName),
                new Claim(type: GenericConstant.CLAIM_LAST_NAME, employee.LastName),
                new Claim(type: ClaimTypes.Email, employee.Email) 
            }; 
            #endregion --Employee level claims items--
             

            #region --Employee permissions--
            //Added custom claims of permission array  
            #endregion --Employee permissions--

            return claims;
        }

      
        public string GenerateToken(dynamic employee)
        {
            JwtSecurityToken jwtSecurityToken = GenerateJWTSecurityToken(
                claims: GetClaims(employee), signingCredentials: GetSigningCredentials());

            return (new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
        }

        #endregion --JWT token generation--

        #region --Refresh token related activites--

        /// <summary>
        /// This method generates the refresh token that is a random string of characters generated from
        /// the random number generator and then base 64 encoded.
        /// </summary>
        /// <returns>Refresh token.</returns>
        public string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// This method extracts the claims principle from the token that has come back from the client and has been
        /// determined to be invalid.
        /// </summary>
        /// <param name="jwtToken">The jwt token as received from the client.</param>
        /// <returns>The claims principle corresponding to the token.</returns>
        /// <exception cref="SecurityTokenException">Exception indicating that the received jwt token is invalid.</exception>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string jwtToken)
        {
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration[ConfigurationKeyConstant.JWT_SECRET_KEY])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal principal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                //TODO: The inline string has to be moved to the resources.
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }

        #endregion --Refresh token related activites--
    }
}
