using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Authorization;

namespace ue_mes_test.FakeObjects
{
    public class RoleFake
    {
        public static Role GetValidRole()
        {
            var validRole = new Role()
            {
                RoleId = 1,
                Name = "Administrator",
                Description = "Has full access to all features of the system"
            };

            return validRole;
        }

        public static List<Role> GetValidRoles()
        {
            var validRoles = new List<Role>()
            {
                new Role()
                {
                    RoleId = 1,
                    Name = "Administrator",
                    Description = "Has full access to all features of the system"
                },
                new Role()
                {
                    RoleId = 3,
                    Name = "OrderScheduler",
                    Description = "Has access to schedule orders in the system."
                },
                new Role()
                {
                    RoleId = 5,
                    Name = "Operator",
                    Description = "Has access to run basic work element screen functions."
                }
            };

            return validRoles;
        }
    }
}
