using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
    public class UserLogicTest
    {
        // Mock
        private Mock<IUserData> userMock;

        // Fakes
        private User validActiveUser;
        private User validInactiveUser;
        private static User validNewUser;
        private User invalidNewUser;
        private User nullUser = null;
        private List<User> emptyList = new List<User>();
        private List<User> validUserListActiveOnly;
        private List<User> validUserListAll;

        [SetUp]
        public void Setup()
        {
            validActiveUser = UserFake.GetValidActiveUser();
            validInactiveUser = UserFake.GetValidInactiveUser();
            validNewUser = UserFake.GetValidNewUser();
            invalidNewUser = UserFake.GetInvalidNewUser();

            validUserListActiveOnly = UserFake.GetValidUserList().Where(user => user.IsActive).ToList();
            validUserListAll = UserFake.GetValidUserList();

            userMock = new Mock<IUserData>();
        }

        #region Get by User ID

        [TestCase(1)]
        public void IfGivenAValidUserIdForAnActiveUserThenReturnUserObjectWithData(int userId)
        {
            // Arrange
            userMock.Setup(x => x.GetUserById(userId, false))
                .Returns(validActiveUser);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var user = userLogic.GetUserById(userId, false);

                // Assert
                Assert.That(user != null && user.UserId == userId);
            }
        }

        [TestCase(2,true)]
        public void IfGivenAValidUserIdForAnInactiveUserAndIncludeInactiveIsTrueThenReturnUserObjectWithData(int userId, bool includeInactive)
        {
            // Arrange
            userMock.Setup(x => x.GetUserById(userId, includeInactive))
                .Returns(validInactiveUser);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var user = userLogic.GetUserById(userId, includeInactive);

                // Assert
                Assert.That(user != null && user.UserId == userId);
            }
        }

        [TestCase(2, false)]
        public void IfGivenAValidUserIdForAnInactiveUserAndIncludeInactiveIsFalseThenReturnNull(int userId, bool includeInactive)
        {
            // Arrange
            userMock.Setup(x => x.GetUserById(userId, includeInactive))
                .Returns(nullUser);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var user = userLogic.GetUserById(userId, includeInactive);

                // Assert
                Assert.Null(user);
            }
        }

        [TestCase(-1)]
        public void IfGivenAnInvalidUserIdForAnActiveUserThenReturnNull(int userId)
        {
            // Arrange
            userMock.Setup(x => x.GetUserById(userId, false))
                .Returns(nullUser);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var user = userLogic.GetUserById(userId, false);

                // Assert
                Assert.Null(user);
            }
        }

        #endregion

        #region Get by Login Name

        [TestCase("testUser")]
        public void IfGivenAValidUserLoginForAnActiveUserThenReturnUserObjectWithData(string loginName)
        {
            // Arrange
            userMock.Setup(x => x.GetUserByLoginName(loginName, false))
                .Returns(validActiveUser);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var user = userLogic.GetUserByLoginName(loginName, false);

                // Assert
                Assert.That(user != null && user.LoginName == loginName);
            }
        }

        [TestCase("testInactiveUser", true)]
        public void IfGivenAValidUserLoginForAnInactiveUserAndIncludeInactiveIsTrueThenReturnUserObjectWithData(string loginName, bool includeInactive)
        {
            // Arrange
            userMock.Setup(x => x.GetUserByLoginName(loginName, includeInactive))
                .Returns(validInactiveUser);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var user = userLogic.GetUserByLoginName(loginName, includeInactive);

                // Assert
                Assert.That(user != null && user.LoginName == loginName);
            }
        }

        [TestCase("testInactiveUser", false)]
        public void IfGivenAValidUserLoginForAnInactiveUserAndIncludeInactiveIsFalseThenReturnNull(string loginName, bool includeInactive)
        {
            // Arrange
            userMock.Setup(x => x.GetUserByLoginName(loginName, includeInactive))
                .Returns(nullUser);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var user = userLogic.GetUserByLoginName(loginName, includeInactive);

                // Assert
                Assert.Null(user);
            }
        }

        [TestCase("NA")]
        public void IfGivenAnInvalidUserLoginForAnActiveUserThenReturnNull(string loginName)
        {
            // Arrange
            userMock.Setup(x => x.GetUserByLoginName(loginName, false))
                .Returns(nullUser);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var user = userLogic.GetUserByLoginName(loginName, false);

                // Assert
                Assert.Null(user);
            }
        }

        #endregion

        #region Get by Email

        [TestCase("testUser@ubiquitous.energy")]
        public void IfGivenAValidUserEmailForAnActiveUserThenReturnUserObjectWithData(string emailAddress)
        {
            // Arrange
            userMock.Setup(x => x.GetUserByEmail(emailAddress, false))
                .Returns(validActiveUser);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var user = userLogic.GetUserByEmail(emailAddress, false);

                // Assert
                Assert.That(user != null && user.EmailAddress == emailAddress);
            }
        }

        [TestCase("testInactiveUser@ubiquitous.energy", true)]
        public void IfGivenAValidUserEmailForAnInactiveUserAndIncludeInactiveIsTrueThenReturnUserObjectWithData(string emailAddress, bool includeInactive)
        {
            // Arrange
            userMock.Setup(x => x.GetUserByEmail(emailAddress, includeInactive))
                .Returns(validInactiveUser);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var user = userLogic.GetUserByEmail(emailAddress, includeInactive);

                // Assert
                Assert.That(user != null && user.EmailAddress == emailAddress);
            }
        }

        [TestCase("testInactiveUser@ubiquitous.energy", false)]
        public void IfGivenAValidUserEmailForAnInactiveUserAndIncludeInactiveIsFalseThenReturnNull(string emailAddress, bool includeInactive)
        {
            // Arrange
            userMock.Setup(x => x.GetUserByEmail(emailAddress, includeInactive))
                .Returns(nullUser);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var user = userLogic.GetUserByEmail(emailAddress, includeInactive);

                // Assert
                Assert.Null(user);
            }
        }

        [TestCase("NA")]
        public void IfGivenAnInvalidUserEmailForAnActiveUserThenReturnNull(string emailAddress)
        {
            // Arrange
            userMock.Setup(x => x.GetUserByEmail(emailAddress, false))
                .Returns(nullUser);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var user = userLogic.GetUserByEmail(emailAddress, false);

                // Assert
                Assert.Null(user);
            }
        }

        #endregion

        #region Get All Users

        [TestCase(true)]
        public void IfRequestingAllUsersAndIncludeInactiveIsTrueThenReturnUserListWithData(bool includeInactive)
        {
            // Arrange
            userMock.Setup(x => x.GetAllUsers(includeInactive))
                .Returns(validUserListAll);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var userListAll = userLogic.GetAllUsers(includeInactive);

                // Assert
                // Should be at least one active and inactive user
                Assert.That(userListAll != null && userListAll.Count > 0 && userListAll.Any(user => user.IsActive) && userListAll.Any(user => !user.IsActive));
            }
        }

        [TestCase(false)]
        public void IfRequestingAllUsersAndIncludeInactiveIsFalseThenReturnUserListWithDataForActiveOnly(bool includeInactive)
        {
            // Arrange
            userMock.Setup(x => x.GetAllUsers(includeInactive))
                .Returns(validUserListActiveOnly);

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var userListAll = userLogic.GetAllUsers(includeInactive);

                // Assert
                // Should be at least one active and no inactive users
                Assert.That(userListAll != null && userListAll.Count > 0 && userListAll.Any(user => user.IsActive) && !userListAll.Any(user => !user.IsActive));
            }
        }

        #endregion

        #region Add User

        [Test]
        public void IfAddingANewUserAndTheUserDoesNotExistThenVerifyAddUserIsCalled()
        {
            // Arrange
            userMock.Setup(x => x.GetUserByEmail(validNewUser.EmailAddress, false))
                .Returns(nullUser);

            userMock.Setup(x => x.AddUser(validNewUser));

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                userLogic.AddUser(validNewUser);

                // Assert
                userMock.Verify(mock => mock.AddUser(validNewUser), Times.Once());
            }
        }

        [Test]
        public void IfAddingANewUserAndTheUserAlreadyExistsThenReturnInvalidDataException()
        {
            // Arrange
            userMock.Setup(x => x.GetUserByEmail(invalidNewUser.EmailAddress, false))
                .Returns(validActiveUser);

            userMock.Setup(x => x.AddUser(invalidNewUser));

            using (var userLogic = new UserLogic(userMock.Object))
            {
                // Act
                var existingUserException = Assert.Throws<InvalidDataException>(() => userLogic.AddUser(invalidNewUser));

                // Assert
                Assert.That(existingUserException.Message, Is.EqualTo(String.Format("User with email {0} already exists", invalidNewUser.EmailAddress)));
                userMock.Verify(mock => mock.AddUser(invalidNewUser), Times.Never);
            }
        }

        #endregion

        #region Test cases just for coverage, no logic called

        [TestCase]
        public void IfGivenNullDataClassThenReturnArgumentNullException()
        {
            // Arrange
            // Nothing to arrange

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Attempt to act
                using (var userLogic = new UserLogic(null))
                {
                    var userList = userLogic.GetAllUsers();
                }
            });
        }

        #endregion
    }
}
