using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Authorization;

namespace ue_mes_test.FakeObjects
{
    public class UserFake
    {
        public static User GetValidActiveUser()
        {
            return new User()
            {
                UserId = 1,
                LoginName = "testUser",
                FirstName = "Test",
                LastName = "User",
                EmailAddress = "testUser@ubiquitous.energy",
                JobTitle = "Operator",
                IsActive = true,
                UserRoles = new List<UserRole>() {
                new UserRole() {
                        Role = new Role() {
                            RoleId = 1,
                            Name = "Operator",
                            Description = "Has access to run basic work element screen functions."
                        }
                    }
                },
                UserAttributes = new List<UserAttribute> {
                    new UserAttribute() {
                        UserAttributeType = new UserAttributeType() {
                            UserAttributeTypeId = 1,
                            Name = "BadgeId",
                            Description = "RFID Badge ID Number which is used to authorize users through a badge scan"
                        },
                        UserAttributeValue = "RFID0311"
                    },
                    new UserAttribute() {
                        UserAttributeType = new UserAttributeType() {
                            UserAttributeTypeId = 2,
                            Name = "PhoneNumber",
                            Description = "Used for sending text alerts if desired"
                        },
                        UserAttributeValue = "123-456-7890"
                    }
                }
            };
        }

        public static User GetValidInactiveUser()
        {
            return new User()
            {
                UserId = 2,
                LoginName = "testInactiveUser",
                FirstName = "Inactive",
                LastName = "User",
                EmailAddress = "testInactiveUser@ubiquitous.energy",
                JobTitle = "Software Engineer",
                IsActive = false,
                UserRoles = new List<UserRole>() {
                new UserRole() {
                        Role = new Role() {
                            RoleId = 1,
                            Name = "Administrator",
                            Description = "Has full access to all features of the system."
                        }
                    }
                },
                UserAttributes = new List<UserAttribute> {
                    new UserAttribute() {
                        UserAttributeType = new UserAttributeType() {
                            UserAttributeTypeId = 1,
                            Name = "BadgeId",
                            Description = "RFID Badge ID Number which is used to authorize users through a badge scan"
                        },
                        UserAttributeValue = "RFID0456"
                    },
                    new UserAttribute() {
                        UserAttributeType = new UserAttributeType() {
                            UserAttributeTypeId = 2,
                            Name = "PhoneNumber",
                            Description = "Used for sending text alerts if desired"
                        },
                        UserAttributeValue = "123-456-0001"
                    }
                }
            };
        }

        public static User GetValidNewUser()
        {
            return new User()
            {
                UserId = 3,
                LoginName = "testUserNew",
                FirstName = "New Test",
                LastName = "User",
                EmailAddress = "testNewUser@ubiquitous.energy",
                JobTitle = "Operator",
                IsActive = true,
                UserRoles = new List<UserRole>() {
                    new UserRole() {
                        Role = new Role() {
                            RoleId = 1,
                            Name = "Operator",
                            Description = "Has access to run basic work element screen functions."
                        }
                    }
                },
                UserAttributes = new List<UserAttribute> {
                    new UserAttribute() {
                        UserAttributeType = new UserAttributeType() {
                            UserAttributeTypeId = 1,
                            Name = "BadgeId",
                            Description = "RFID Badge ID Number which is used to authorize users through a badge scan"
                        },
                        UserAttributeValue = "RFID0NEW"
                    },
                    new UserAttribute() {
                        UserAttributeType = new UserAttributeType() {
                            UserAttributeTypeId = 2,
                            Name = "PhoneNumber",
                            Description = "Used for sending text alerts if desired"
                        },
                        UserAttributeValue = "123-456-0123"
                    }
                }
            };
        }

        public static User GetInvalidNewUser()
        {
            // This will be used to attempt an add, but since this user is already in the list, it will fail.
            return GetValidActiveUser();
        }

        public static List<User> GetValidUserList()
        {
            var validUserList = new List<User>
            {
                GetValidActiveUser(),
                GetValidInactiveUser()
            };

            return validUserList;
        }
    }
}
