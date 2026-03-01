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
    public class RoleData : DbContextBase, IRoleData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.Role, Role>()
                .ReverseMap();
            mapperConfig.CreateMap<Model.UserRole, UserRole>();
            mapperConfig.CreateMap<Model.User, User>();
            mapperConfig.CreateMap<Model.UserAttribute, UserAttribute>();
            mapperConfig.CreateMap<Model.UserAttributeType, UserAttributeType>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public RoleData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a list of authorization roles in the system.  This will primarily be used for display and client dropdowns.
        /// </summary>
        /// <returns></returns>
        public List<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var roleRecords = mesProductionContext.Roles
                    .Include(role => role.UserRoles)
                    .ThenInclude(userRole => userRole.User)
                    .ThenInclude(user => user.UserAttributes)
                    .ThenInclude(userAttribute => userAttribute.UserAttributeType)
                    .ToList();

                if (roleRecords != null)
                {
                    roles = mapper.Map<List<Model.Role>, List<Role>>(roleRecords);
                }
            }

            return roles;
        }

        /// <summary>
        /// Returns an authorization role in the system with matching name, along with any users assigned to the role.
        /// </summary>
        /// <returns></returns>
        public Role GetRoleAndUsersInRoleByName(string roleName)
        {
            Role role = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var roleRecord = mesProductionContext.Roles
                    .Where(role => role.Name == roleName)
                    .Include(role => role.UserRoles)
                    .ThenInclude(userRole => userRole.User)
                    .ThenInclude(user => user.UserAttributes)
                    .ThenInclude(userAttribute => userAttribute.UserAttributeType)
                    .FirstOrDefault();

                if (roleRecord != null)
                {
                    role = mapper.Map<Role>(roleRecord); ;
                }
            }

            return role;
        }

        #endregion
    }
}
