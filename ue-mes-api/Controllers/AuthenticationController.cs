using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using ue_mes_logic.Authorization;
using Google.Apis.Auth;
using ue_mes_api.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ue_mes_api.Dto;
using ue_mes_entities.Authorization;
using System.Linq;
using Newtonsoft.Json.Linq;
using ue_mes_data.Authorization.Interface;
using ue_mes_data.Model;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationMiddleware _authenticationService;
        IOptionsSnapshot<Settings.AuthenticationSettings> AuthenticationSettings { get; }

        ILogger Logger { get; }
        IUserData UserData { get; }

        public AuthenticationController(ILogger<AuthenticationController> logger,
            IUserData userData,
            IOptionsSnapshot<Settings.AuthenticationSettings> authenticationSettingsOptions)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            UserData = userData ?? throw new ArgumentNullException(nameof(userData));
            AuthenticationSettings = authenticationSettingsOptions ?? throw new ArgumentNullException(nameof(authenticationSettingsOptions));
            _authenticationService = new AuthenticationMiddleware(userData ?? throw new ArgumentNullException(nameof(userData)));
        }

        #region Public Controller Methods

        /// <summary>
        /// Bypass auth for testing with a hard coded user
        /// <param name="hardCodedUserId">Unique user ID to force authentication with</param>
        /// <returns></returns>
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("Authenticate/HardCodeUser/{hardCodedUserId}")]
        public IActionResult AuthenticateWithHarcodedId(int hardCodedUserId)
        {
            try
            {
                using (var userLogic = new UserLogic(UserData))
                {
                    var user = userLogic.GetUserById(hardCodedUserId);
                        
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, AuthenticationMiddleware.Encrypt(AuthenticationSettings.Value.JwtEmailEncryption,user.EmailAddress)),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("g-user-id", user.UserId.ToString()),
                        new Claim("g-user-photo-link", @"http://localhost:3000/site-images/User1.png"),
                    };

                    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthenticationSettings.Value.JwtSecret));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(String.Empty,
                      String.Empty,
                      claims,
                      expires: DateTime.Now.AddSeconds(55 * 60),
                      signingCredentials: creds);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred when retrieving the user by hard coded ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when retrieving the user by hard coded ID.  Please try again or contact MES support for help."));
            }
        }

        /// <summary>
        /// Authenticate a user with Google API.  This will validate the initial response from the client
        /// <param name="googleTokenId">Unique Google token ID to validate against the Google API</param>
        /// <returns></returns>
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("Authenticate/Google")]
        public IActionResult AuthenticateWithGoogle([FromBody] string googleTokenId)
        {
            try
            {
                var payload = GoogleJsonWebSignature.ValidateAsync(googleTokenId,
                    new GoogleJsonWebSignature.ValidationSettings() { ExpirationTimeClockTolerance = new TimeSpan(0, 0, -30) } // to handle any clock drift from Google.
                ).Result;
                var user = _authenticationService.Authenticate(payload);

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, AuthenticationMiddleware.Encrypt(AuthenticationSettings.Value.JwtEmailEncryption,user.EmailAddress)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("g-user-id", user.UserId.ToString()),
                new Claim("g-user-photo-link", payload.Picture),
            };

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthenticationSettings.Value.JwtSecret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(String.Empty,
                  String.Empty,
                  claims,
                  expires: DateTime.Now.AddSeconds(55 * 60),
                  signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred when authenticating with Google.");
                return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when authenticating with Google.  Please try again or contact MES support for help."));
            }
        }

        /// <summary>
        /// Retrieve full user details from the encrypted JWT that is stored on the client
        /// <param name="jwtToken">JWT token from the client</param>
        /// <returns></returns>
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("Authenticate/GetUserFromJwt")]
        public IActionResult GetUserFromJwt([FromBody] string jwtToken)
        {
            try
            {
                var tokenPayloadResult = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);

                var userId = tokenPayloadResult.Claims.Any(claim => claim.Type == "g-user-id") ? tokenPayloadResult.Claims.Where(claim => claim.Type == "g-user-id").FirstOrDefault().Value : null;
                var userPhotoLink = tokenPayloadResult.Claims.Any(claim => claim.Type == "g-user-photo-link") ? tokenPayloadResult.Claims.Where(claim => claim.Type == "g-user-photo-link").FirstOrDefault().Value : null;

                if (userId != null)
                {
                    using (var userLogic = new UserLogic(UserData))
                    {
                        return Ok(new
                        {
                            user = userLogic.GetUserById(int.Parse(userId)),
                            userPhoto = userPhotoLink,
                        });
                    }
                }

                // no user found
                Logger.LogWarning("No user was found that matched the token paylod");
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred when retrieving the user by token.");
                return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when retrieving the user by token.  Please try again or contact MES support for help."));
            }
        }

        #endregion
    }
}
