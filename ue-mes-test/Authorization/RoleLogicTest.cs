using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Authorization.Interface;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Authorization;
using ue_mes_logic.Authorization;
using ue_mes_logic.Global;
using ue_mes_test.FakeObjects;

namespace ue_mes_test.Authorization
{
    public class RoleLogicTest
    {
        // Mock
        private Mock<IRoleData> roleMock;

        // Fakes
        private Role validRole;
        private Role nullRole = null;
        private List<Role> emptyList = new List<Role>();
        private List<Role> validRoles;

        [SetUp]
        public void Setup()
        {
            validRole = RoleFake.GetValidRole();
            validRoles = RoleFake.GetValidRoles();

            roleMock = new Mock<IRoleData>();
        }

        [TestCase]
        public void IfRequestingAllRolesThenReturnRoleListWithData()
        {
            // Arrange
            roleMock.Setup(x => x.GetRoles())
                .Returns(validRoles);

            using (var roleLogic = new RoleLogic(roleMock.Object))
            {
                // Act
                var roles = roleLogic.GetRoles();

                // Assert
                Assert.That(roles != null && roles.Count > 0);
            }
        }

        [TestCase("Administrator")]
        public void IfGivenAValidRoleNameThenReturnRoleObjectWithData(string roleName)
        {
            // Arrange
            roleMock.Setup(x => x.GetRoleAndUsersInRoleByName(roleName))
                .Returns(validRole);

            using (var roleLogic = new RoleLogic(roleMock.Object))
            {
                // Act
                var role = roleLogic.GetRoleAndUsersInRoleByName(roleName);

                // Assert
                Assert.That(role != null && role.Name == roleName);
            }
        }

        [TestCase("311")]
        public void IfGivenAnInvalidRoleNameThenReturnNull(string roleName)
        {
            // Arrange
            roleMock.Setup(x => x.GetRoleAndUsersInRoleByName(roleName))
                .Returns(nullRole);

            using (var roleLogic = new RoleLogic(roleMock.Object))
            {
                // Act
                var role = roleLogic.GetRoleAndUsersInRoleByName(roleName);

                // Assert
                Assert.IsNull(role);
            }
        }

        #region Test cases just for coverage, no logic called

        [TestCase("Administrator")]
        public void IfGivenNullDataClassThenReturnArgumentNullException(string roleName)
        {
            // Arrange
            // Nothing to arrange

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Attempt to act
                using (var roleLogic = new RoleLogic(null))
                {
                    var role = roleLogic.GetRoleAndUsersInRoleByName(roleName);
                }
            });
        }

        #endregion
    }
}
