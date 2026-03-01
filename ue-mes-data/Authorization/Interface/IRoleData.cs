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
    public interface IRoleData
    {
        /// <summary>
        /// Returns a list of authorization roles in the system.  This will primarily be used for display and client dropdowns.
        /// </summary>
        /// <returns></returns>
        public List<Role> GetRoles();

        /// <summary>
        /// Returns an authorization role in the system with matching name, along with any users assigned to the role.
        /// </summary>
        /// <returns></returns>
        public Role GetRoleAndUsersInRoleByName(string roleName);
    }
}
