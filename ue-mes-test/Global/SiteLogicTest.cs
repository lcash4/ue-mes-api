using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Global;
using ue_mes_logic.Global;
using ue_mes_test.FakeObjects;

namespace ue_mes_test.Global
{
    public class SiteLogicTest
    {
        // Mock
        private Mock<ISiteData> siteMock;

        // Fakes
        private Site validSite;
        private Site nullSite;


        [SetUp]
        public void Setup()
        {
            validSite = SiteFake.GetValidSite();
            nullSite = null;

            siteMock = new Mock<ISiteData>();   
        }

        [TestCase("PHX")]
        public void IfGivenValidSiteDisplayNameThenReturnSiteObjectWithData(string givenSiteDisplayName)
        {
            // Arrange
            siteMock.Setup(x => x.GetSiteByDisplayName(It.IsAny<string>()))
                .Returns(validSite);

            using (var siteLogic = new SiteLogic(siteMock.Object))
            {
                // Act
                var site = siteLogic.GetSiteByDisplayName(givenSiteDisplayName);

                // Assert
                Assert.That(site.Name == "Phoenix, AZ USA");
            }
        }

        [TestCase("311")]
        public void IfGivenInvalidSiteDisplayNameThenReturnNull(string givenSiteDisplayName)
        {
            // Arrange
            siteMock.Setup(x => x.GetSiteByDisplayName(It.IsAny<string>()))
                .Returns(nullSite);

            using (var siteLogic = new SiteLogic(siteMock.Object))
            {
                // Act
                var site = siteLogic.GetSiteByDisplayName(givenSiteDisplayName);

                // Assert
                Assert.IsNull(site);
            }
        }

        [TestCase("")]
        public void IfGivenEmptySiteDisplayNameThenReturnNull(string givenSiteDisplayName)
        {
            // Arrange
            siteMock.Setup(x => x.GetSiteByDisplayName(It.IsAny<string>()))
                .Returns(nullSite);

            using (var siteLogic = new SiteLogic(siteMock.Object))
            {
                // Act
                var site = siteLogic.GetSiteByDisplayName(givenSiteDisplayName);

                // Assert
                Assert.IsNull(site);
            }
        }

        #region Test cases just for coverage, no logic called

        [TestCase("")]
        public void IfGivenNullDataClassThenReturnArgumentNullException(string givenSiteDisplayName)
        {
            // Arrange
            // Nothing to arrange

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Attempt to act
                using (var siteLogic = new SiteLogic(null))
                {
                    var site = siteLogic.GetSiteByDisplayName(givenSiteDisplayName);
                }
            });
        }

        #endregion
    }
}
