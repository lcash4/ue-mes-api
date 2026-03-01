using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Global;
using ue_mes_logic.Global;
using ue_mes_test.FakeObjects;

namespace ue_mes_test.Global
{
    public class CoaterLogicTest
    {
        // Mock
        private Mock<ICoaterData> coaterMock;

        // Fakes
        private Coater validCoater;
        private Coater nullCoater;


        [SetUp]
        public void Setup()
        {
            validCoater = CoaterFake.GetValidCoater();
            nullCoater = null;

            coaterMock = new Mock<ICoaterData>();
        }

        [TestCase("PHX")]
        public void IfGivenValidSiteDisplayNameThenReturnFirstCoaterObjectWithData(string siteDisplayName)
        {
            // Arrange
            coaterMock.Setup(x => x.GetActiveLayerCoaterBySiteName(It.IsAny<string>()))
                .Returns(validCoater);

            using (var coaterLogic = new CoaterLogic(coaterMock.Object))
            {
                // Act
                var coater = coaterLogic.GetActiveLayerCoaterBySiteName(siteDisplayName);

                // Assert
                Assert.That(coater.Equipment.Line.Unit.Plant.Site.DisplayName == siteDisplayName);
            }
        }

        [TestCase("311")]
        public void IfGivenInvalidSiteDisplayNameThenReturnNull(string siteDisplayName)
        {
            // Arrange
            coaterMock.Setup(x => x.GetActiveLayerCoaterBySiteName(It.IsAny<string>()))
                .Returns(nullCoater);

            using (var coaterLogic = new CoaterLogic(coaterMock.Object))
            {
                // Act
                var coater = coaterLogic.GetActiveLayerCoaterBySiteName(siteDisplayName);

                // Assert
                Assert.IsNull(coater);
            }
        }

        [TestCase("")]
        public void IfGivenEmptySiteDisplayNameThenReturnNull(string siteDisplayName)
        {
            // Arrange
            coaterMock.Setup(x => x.GetActiveLayerCoaterBySiteName(It.IsAny<string>()))
                .Returns(nullCoater);

            using (var coaterLogic = new CoaterLogic(coaterMock.Object))
            {
                // Act
                var coater = coaterLogic.GetActiveLayerCoaterBySiteName(siteDisplayName);

                // Assert
                Assert.IsNull(coater);
            }
        }

        #region Test cases just for coverage, no logic called

        [TestCase("")]
        public void IfGivenNullDataClassThenReturnArgumentNullException(string siteDisplayName)
        {
            // Arrange
            // Nothing to arrange

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Attempt to act
                using (var coaterLogic = new CoaterLogic(null))
                {
                    var coater = coaterLogic.GetActiveLayerCoaterBySiteName(siteDisplayName);
                }
            });
        }

        #endregion
    }
}
