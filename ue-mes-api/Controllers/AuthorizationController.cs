using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using ue_mes_data.Authorization.Interface;
using ue_mes_entities.Authorization;
using ue_mes_logic.Authorization;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthorizationController : ControllerBase
    {
        ILogger Logger { get; }
        IUserData UserData { get; }

        public AuthorizationController(ILogger<AuthorizationController> logger, IUserData userData)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            UserData = userData ?? throw new ArgumentNullException(nameof(userData));
        }

        #region Public Controller Methods

        /// <summary>
        /// Returns a full <see cref="User"/> entity with child entities included.
        /// <param name="loginName">Unique login name for the <see cref="User"/></param>
        /// <returns></returns>
        /// </summary>
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetByLoginName/{loginName}")]
        public User GetUserByLoginName(string loginName)
        {
            using (var userLogic = new UserLogic(UserData))
            {
                return userLogic.GetUserByLoginName(loginName);
            }
        }

        /// <summary>
        /// Returns a full <see cref="User"/> entity with child entities included.
        /// </summary>
        /// <param name="emailAddress">Unique email address for the <see cref="User"/></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetByEmail/{emailAddress}")]
        public User GetUserByEmail(string emailAddress)
        {
            using (var userLogic = new UserLogic(UserData))
            {
                return userLogic.GetUserByEmail(emailAddress);
            }
        }

        /// <summary>
        /// Returns all active <see cref="User"/> entity with child entities included.
        /// </summary>
        /// <param name="includeInactive">If set to true, all users will be returned, even if inactive</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetAll/{includeInactive}")]
        public List<User> GetAllUsers(bool includeInactive = false)
        {
            using (var userLogic = new UserLogic(UserData))
            {
                return userLogic.GetAllUsers(includeInactive);
            }
        }

        #endregion
    }
}
