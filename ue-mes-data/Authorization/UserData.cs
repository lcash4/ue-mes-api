using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Authorization.Interface;
using ue_mes_entities.Authorization;

namespace ue_mes_data.Authorization
{
    public class UserData : DbContextBase, IUserData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.User, User>()
                .ReverseMap();
            mapperConfig.CreateMap<Model.UserRole, UserRole>()
                .ReverseMap()
                    .ForMember(userRoleModel => userRoleModel.UserId, opt => opt.MapFrom(userRoleEntity => userRoleEntity.User.UserId))
                    .ForMember(userRoleModel => userRoleModel.RoleId, opt => opt.MapFrom(userRoleEntity => userRoleEntity.Role.RoleId))
                    .ForMember(userRoleModel => userRoleModel.User, opt => opt.Ignore())
                    .ForMember(userRoleModel => userRoleModel.Role, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.Role, Role>();
            mapperConfig.CreateMap<Model.UserAttribute, UserAttribute>();
            mapperConfig.CreateMap<Model.UserAttributeType, UserAttributeType>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public UserData()
        {
            mapper = mapperConfiguration.CreateMapper();
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
            User user = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var userRecord = mesProductionContext.Users
                    .Where(user => user.UserId == userId
                        && user.IsActive || (!user.IsActive && includeInactive))
                    .Include(user => user.UserRoles)
                    .ThenInclude(userRole => userRole.Role)
                    .Include(user => user.UserAttributes)
                    .ThenInclude(userAttribute => userAttribute.UserAttributeType)
                    .FirstOrDefault();

                if (userRecord != null)
                {
                    user = mapper.Map<User>(userRecord);
                }
            }

            return user;
        }

        /// <summary>
        /// Returns a full <see cref="User"/> entity with child entities included.
        /// </summary>
        /// <param name="loginName">Unique login name for the <see cref="User"/></param>
        /// <param name="includeInactive">If set to true, user will be returned, even if inactive</param>
        /// <returns></returns>
        public User GetUserByLoginName(string loginName, bool includeInactive = false)
        {
            User user = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var userRecord = mesProductionContext.Users
                    .Where(user => user.LoginName == loginName
                        && user.IsActive || (!user.IsActive && includeInactive))
                    .Include(user => user.UserRoles)
                    .ThenInclude(userRole => userRole.Role)
                    .Include(user => user.UserAttributes)
                    .ThenInclude(userAttribute => userAttribute.UserAttributeType)
                    .FirstOrDefault();

                if (userRecord != null)
                {
                    user = mapper.Map<User>(userRecord);
                }
            }

            return user;
        }

        /// <summary>
        /// Returns a full <see cref="User"/> entity with child entities included.
        /// </summary>
        /// <param name="emailAddress">Unique email address for the <see cref="User"/></param>
        /// <param name="includeInactive">If set to true, user will be returned, even if inactive</param>
        /// <returns></returns>
        public User GetUserByEmail(string emailAddress, bool includeInactive = false)
        {
            User user = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var userRecord = mesProductionContext.Users
                    .Where(user => user.EmailAddress == emailAddress
                        && user.IsActive || (!user.IsActive && includeInactive))
                    .Include(user => user.UserRoles)
                    .ThenInclude(userRole => userRole.Role)
                    .Include(user => user.UserAttributes)
                    .ThenInclude(userAttribute => userAttribute.UserAttributeType)
                    .FirstOrDefault();

                if (userRecord != null)
                {
                    user = mapper.Map<User>(userRecord);
                }
            }

            return user;
        }

        /// <summary>
        /// Returns all active <see cref="User"/> entity with child entities included.
        /// </summary>
        /// <param name="includeInactive">If set to true, all users will be returned, even if inactive</param>
        /// <returns></returns>
        public List<User> GetAllUsers(bool includeInactive = false)
        {
            List<User> users = new List<User>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var userRecords = mesProductionContext.Users
                    .Where(user => user.IsActive || (!user.IsActive && includeInactive))
                    .Include(user => user.UserRoles)
                    .ThenInclude(userRole => userRole.Role)
                    .Include(user => user.UserAttributes)
                    .ThenInclude(userAttribute => userAttribute.UserAttributeType)
                    .ToList();

                if (userRecords != null)
                {
                    users = mapper.Map<List<Model.User>, List<User>>(userRecords);
                }
            }

            return users;
        }

        /// <summary>
        /// Add a new user to the system.  This will typicall take place when someone logs in for the first time.
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            using (var roleData = new RoleData())
            {
                var systemRoles = roleData.GetRoles();
                var viewOnlyRole = systemRoles.Where(role => role.Name == "ViewOnly").First();
                var newUserRole = new UserRole() { UserRoleId = 0, User = user, Role = viewOnlyRole };

                user.UserRoles = new List<UserRole> { newUserRole };

                var userRecord = mapper.Map<Model.User>(user);
                if (userRecord != null)
                {
                    // When adding a new user, there will be a default role of "View Only".  It will be up to the system admin to add the user to different roles if needed.
                    // This ensures that if a new user logs in, they cannot make any modifications to the system data until granted access.

                    // Set the Last Modified By if present
                    userRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(user.LastModifiedBy) ? user.LastModifiedBy : null;

                    using (var mesProductionContext = new Model.MesProductionContext())
                    {
                        mesProductionContext.Users.AddRange(userRecord);
                        mesProductionContext.SaveChanges();
                    }
                }
            }
        }

        #endregion
    }
}
