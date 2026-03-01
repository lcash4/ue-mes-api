using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Authorization;
using ue_mes_data.Authorization.Interface;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Authorization;

namespace ue_mes_logic.Authorization
{
    public class RoleLogic : LogicBase
    {
        IRoleData RoleData { get; }

        #region Constructor

        public RoleLogic(IRoleData roleData)
        {
            RoleData = roleData ?? throw new ArgumentNullException(nameof(roleData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a list of authorization roles in the system.  This will primarily be used for display and client dropdowns.
        /// </summary>
        /// <returns></returns>
        public List<Role> GetRoles()
        {
            return RoleData.GetRoles();            
        }

        /// <summary>
        /// Returns an authorization role in the system with matching name, along with any users assigned to the role.
        /// </summary>
        /// <returns></returns>
        public Role GetRoleAndUsersInRoleByName(string roleName)
        {
            return RoleData.GetRoleAndUsersInRoleByName(roleName);
        }

        #endregion
    }
}
