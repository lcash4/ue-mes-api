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
    public class EquipmentLogicTest
    {
        // Mock
        private Mock<IEquipmentData> equipmentMock;

        // Fakes
        private Equipment validEquipment;
        private Equipment nullEquipment = null;
        private List<Equipment> emptyList = new List<Equipment>();
        private List<Equipment> validEquipmentListBySite;
        private List<Equipment> validEquipmentListBySiteAndProcess;

        [SetUp]
        public void Setup()
        {
            validEquipment = EquipmentFake.GetValidEquipment();
            validEquipmentListBySite = EquipmentFake.GetValidEquipmentListBySiteName();
            validEquipmentListBySiteAndProcess = EquipmentFake.GetValidEquipmentListBySiteNameAndProcessId();

            equipmentMock = new Mock<IEquipmentData>();
        }

        [TestCase("PHX11A-BARCODE_MARKER")]
        public void IfGivenAValidEquipmentByNameThenReturnEquipmentObjectWithData(string equipmentName)
        {
            // Arrange
            equipmentMock.Setup(x => x.GetEquipmentByName(equipmentName))
                .Returns(validEquipment);

            using (var equipmentLogic = new EquipmentLogic(equipmentMock.Object))
            {
                // Act
                var equipment = equipmentLogic.GetEquipmentByName(equipmentName);

                // Assert
                Assert.That(equipment != null && equipment.Name == equipmentName);
            }
        }

        [TestCase("PHX11A-TEST")]
        public void IfGivenAnInvalidEquipmentByNameThenReturnNull(string equipmentName)
        {
            // Arrange
            equipmentMock.Setup(x => x.GetEquipmentByName(equipmentName))
                .Returns(nullEquipment);

            using (var equipmentLogic = new EquipmentLogic(equipmentMock.Object))
            {
                // Act
                var equipment = equipmentLogic.GetEquipmentByName(equipmentName);

                // Assert
                Assert.Null(equipment);
            }
        }

        [TestCase("PHX")]
        public void IfGivenAValidSiteNameThenReturnEquipmentListWithData(string siteDisplayName)
        {
            // Arrange
            equipmentMock.Setup(x => x.GetEquipmentBySiteDisplayName(siteDisplayName))
                .Returns(validEquipmentListBySite);

            using (var equipmentLogic = new EquipmentLogic(equipmentMock.Object))
            {
                // Act
                var equipmentList = equipmentLogic.GetEquipmentBySiteDisplayName(siteDisplayName);

                // Assert
                Assert.That(equipmentList != null && equipmentList.Count > 0 && !equipmentList.Any(equipment => equipment.Line.Unit.Plant.Site.DisplayName != siteDisplayName));
            }
        }

        [TestCase("311")]
        public void IfGivenAnInvalidSiteNameThenReturnEmptyList(string siteDisplayName)
        {
            // Arrange
            equipmentMock.Setup(x => x.GetEquipmentBySiteDisplayName(siteDisplayName))
                .Returns(emptyList);

            using (var equipmentLogic = new EquipmentLogic(equipmentMock.Object))
            {
                // Act
                var equipmentList = equipmentLogic.GetEquipmentBySiteDisplayName(siteDisplayName);

                // Assert
                Assert.That(equipmentList != null && equipmentList.Count == 0);
            }
        }

        [TestCase("PHX",2)]
        public void IfGivenAValidSiteNameAndProcessIdThenReturnEquipmentListWithData(string siteDisplayName, int processId)
        {
            // Arrange
            equipmentMock.Setup(x => x.GetEquipmentBySiteDisplayNameAndProcessId(siteDisplayName, processId))
                .Returns(validEquipmentListBySiteAndProcess);

            using (var equipmentLogic = new EquipmentLogic(equipmentMock.Object))
            {
                // Act
                var equipmentList = equipmentLogic.GetEquipmentBySiteDisplayNameAndProcessId(siteDisplayName, processId);

                // Assert
                Assert.That(equipmentList != null 
                    && equipmentList.Count > 0 
                    && !equipmentList.Any(equipment => equipment.Line.Unit.Plant.Site.DisplayName != siteDisplayName)
                    && !equipmentList.Any(equipment => equipment.Process.ProcessId != processId));
            }
        }

        [TestCase("311",1)]
        public void IfGivenAnInvalidSiteNameAndProcessIdThenReturnEmptyList(string siteDisplayName, int processId)
        {
            // Arrange
            equipmentMock.Setup(x => x.GetEquipmentBySiteDisplayNameAndProcessId(siteDisplayName, processId))
                .Returns(emptyList);

            using (var equipmentLogic = new EquipmentLogic(equipmentMock.Object))
            {
                // Act
                var equipmentList = equipmentLogic.GetEquipmentBySiteDisplayNameAndProcessId(siteDisplayName, processId);

                // Assert
                Assert.That(equipmentList != null && equipmentList.Count == 0);
            }
        }

        #region Test cases just for coverage, no logic called

        [TestCase("PHX")]
        public void IfGivenNullDataClassThenReturnArgumentNullException(string siteDisplayName)
        {
            // Arrange
            // Nothing to arrange

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Attempt to act
                using (var equipmentLogic = new EquipmentLogic(null))
                {
                    var processList = equipmentLogic.GetEquipmentBySiteDisplayName(siteDisplayName);
                }
            });
        }

        #endregion
    }
}
