using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Authorization;

namespace ue_mes_data.Authorization.Interface
{
    public interface IUserData
    {
        /// <summary>
        /// Returns a full <see cref="User"/> entity with child entities included.
        /// </summary>
        /// <param name="userId">Unique user id for the <see cref="User"/></param>
        /// <param name="includeInactive">If set to true, user will be returned, even if inactive</param>
        /// <returns></returns>
        public User GetUserById(int userId, bool includeInactive = false);

        /// <summary>
        /// Returns a full <see cref="User"/> entity with child entities included.
        /// </summary>
        /// <param name="loginName">Unique login name for the <see cref="User"/></param>
        /// <param name="includeInactive">If set to true, user will be returned, even if inactive</param>
        /// <returns></returns>
        public User GetUserByLoginName(string loginName, bool includeInactive = false);

        /// <summary>
        /// Returns a full <see cref="User"/> entity with child entities included.
        /// </summary>
        /// <param name="emailAddress">Unique email address for the <see cref="User"/></param>
        /// <param name="includeInactive">If set to true, user will be returned, even if inactive</param>
        /// <returns></returns>
        public User GetUserByEmail(string emailAddress, bool includeInactive = false);

        /// <summary>
        /// Returns all active <see cref="User"/> entity with child entities included.
        /// </summary>
        /// <param name="includeInactive">If set to true, all users will be returned, even if inactive</param>
        /// <returns></returns>
        public List<User> GetAllUsers(bool includeInactive = false);

        /// <summary>
        /// Add a new user to the system.  This will typicall take place when someone logs in for the first time.
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user);
    }
}
