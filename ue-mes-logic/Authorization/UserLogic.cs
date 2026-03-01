using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Authorization;
using ue_mes_data.Authorization.Interface;
using ue_mes_data.dbo;
using ue_mes_entities.Authorization;

namespace ue_mes_logic.Authorization
{
    public class UserLogic : LogicBase
    {
        IUserData UserData { get; }

        #region Constructor

        public UserLogic(IUserData userData)
        {
            UserData = userData ?? throw new ArgumentNullException(nameof(userData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a full <see cref="User"/> entity with child entities included.
        /// </summary>
        /// <param name="userId">Unique user id for the <see cref="User"/></param>
        /// <param name="includeInactive">If set to true, user will be returned, even if inactive</param>
        /// <returns></returns>
        public User GetUserById(int userId, bool includeInactive = false)
        {
            return UserData.GetUserById(userId, includeInactive);
        }

        /// <summary>
        /// Returns a full <see cref="User"/> entity with child entities included.
        /// </summary>
        /// <param name="loginName">Unique login name for the <see cref="User"/></param>
        /// <param name="includeInactive">If set to true, user will be returned, even if inactive</param>
        /// <returns></returns>
        public User GetUserByLoginName(string loginName, bool includeInactive = false)
        {
            return UserData.GetUserByLoginName(loginName, includeInactive);
        }

        /// <summary>
        /// Returns a full <see cref="User"/> entity with child entities included.
        /// </summary>
        /// <param name="emailAddress">Unique email address for the <see cref="User"/></param>
        /// <param name="includeInactive">If set to true, user will be returned, even if inactive</param>
        /// <returns></returns>
        public User GetUserByEmail(string emailAddress, bool includeInactive = false)
        {
            return UserData.GetUserByEmail(emailAddress, includeInactive);
        }

        /// <summary>
        /// Returns all active <see cref="User"/> entity with child entities included.
        /// </summary>
        /// <param name="includeInactive">If set to true, all users will be returned, even if inactive</param>
        /// <returns></returns>
        public List<User> GetAllUsers(bool includeInactive = false)
        {
            return UserData.GetAllUsers(includeInactive);
        }

        /// <summary>
        /// Add a new user to the system.  This will typically take place when someone logs in for the first time.
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            // Need to check if the user already exists by email.  If so, return "User with email {0} already exists" exception
            if (user != null)
            {
                var existingUser = UserData.GetUserByEmail(user.EmailAddress);
                if (existingUser != null)
                    throw new InvalidDataException(String.Format("User with email {0} already exists", user.EmailAddress));
            }

            UserData.AddUser(user);
        }

        #endregion
    }
}
