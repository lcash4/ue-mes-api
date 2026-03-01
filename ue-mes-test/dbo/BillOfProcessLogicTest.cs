using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_data.Global.Interface;
using ue_mes_entities.dbo;
using ue_mes_entities.Global;
using ue_mes_logic.dbo;
using ue_mes_test.FakeObjects;

namespace ue_mes_test.dbo
{
    public class BillOfProcessLogicTest
    {
        // Mock
        private Mock<IBillOfProcessData> billOfProcessMock;
        private Mock<IEquipmentData> equipmentMock;

        // Fakes
        private BillOfProcess simpleBillOfProcessWithNoProcesses;
        private static BillOfProcess validNewBillOfProcess;
        private BillOfProcess invalidNewBillOfProcess;
        private BillOfProcess nullBillOfProcess = null;
        private List<BillOfProcess> simpleBillOfProcessList;
        private List<BillOfProcess> emptyBillOfProcessList = new List<BillOfProcess>();

        private Equipment startingEquipmentForBillOfProcess;
        private Equipment nullEquipment = null;

        [SetUp]
        public void Setup()
        {
            simpleBillOfProcessWithNoProcesses = BillOfProcessFake.GetSimpleBillOfProcessWithNoProcesses();
            validNewBillOfProcess = BillOfProcessFake.GetValidNewBillOfProcess();
            invalidNewBillOfProcess = BillOfProcessFake.GetInvalidNewBillOfProcess();
            simpleBillOfProcessList = BillOfProcessFake.GetSimpleBillOfProcessList();

            startingEquipmentForBillOfProcess = EquipmentFake.GetValidStartingEquipmentForBillOfProcess();

            billOfProcessMock = new Mock<IBillOfProcessData>();
            equipmentMock = new Mock<IEquipmentData>();
        }

        #region Get by BillOfProcess ID

        [TestCase(1)]
        public void IfGivenAValidBillOfProcessIdThenReturnBillOfProcessObjectWithData(int billOfProcessId)
        {
            // Arrange
            billOfProcessMock.Setup(x => x.GetBillOfProcessById(billOfProcessId))
                .Returns(simpleBillOfProcessWithNoProcesses);

            using (var billOfProcessLogic = new BillOfProcessLogic(billOfProcessMock.Object, equipmentMock.Object))
            {
                // Act
                var billOfProcess = billOfProcessLogic.GetBillOfProcessById(billOfProcessId);

                // Assert
                Assert.That(billOfProcess != null && billOfProcess.BillOfProcessId == billOfProcessId);
            }
        }

        [TestCase(-1)]
        public void IfGivenAnInvalidBillOfProcessIdForAnActiveBillOfProcessThenReturnNull(int billOfProcessId)
        {
            // Arrange
            billOfProcessMock.Setup(x => x.GetBillOfProcessById(billOfProcessId))
                .Returns(nullBillOfProcess);

            using (var billOfProcessLogic = new BillOfProcessLogic(billOfProcessMock.Object, equipmentMock.Object))
            {
                // Act
                var billOfProcess = billOfProcessLogic.GetBillOfProcessById(billOfProcessId);

                // Assert
                Assert.Null(billOfProcess);
            }
        }

        #endregion

        #region Get by Part ID

        [TestCase(2)]
        public void IfGivenAValidBillOfProcessPartIdThenReturnBillOfProcessObjectWithData(int partId)
        {
            // Arrange
            billOfProcessMock.Setup(x => x.GetBillOfProcessByPartId(partId))
                .Returns(simpleBillOfProcessWithNoProcesses);

            using (var billOfProcessLogic = new BillOfProcessLogic(billOfProcessMock.Object, equipmentMock.Object))
            {
                // Act
                var billOfProcess = billOfProcessLogic.GetBillOfProcessByPartId(partId);

                // Assert
                Assert.That(billOfProcess != null && billOfProcess.Part.PartId == partId);
            }
        }

        [TestCase(-1)]
        public void IfGivenAnInalidBillOfProcessPartIdThenReturnNull(int partId)
        {
            // Arrange
            billOfProcessMock.Setup(x => x.GetBillOfProcessByPartId(partId))
                .Returns(nullBillOfProcess);

            using (var billOfProcessLogic = new BillOfProcessLogic(billOfProcessMock.Object, equipmentMock.Object))
            {
                // Act
                var billOfProcess = billOfProcessLogic.GetBillOfProcessByPartId(partId);

                // Assert
                Assert.Null(billOfProcess);
            }
        }

        #endregion

        #region Get by starting equipment name

        [TestCase("RWC11A-LAB_UV_VIS_MEASUREMENT")]
        public void IfGivenAValidStartingEquipmentNameThenReturnBillOfProcessObjectWithData(string equipmentName)
        {
            // Arrange
            equipmentMock.Setup(x => x.GetEquipmentByName(equipmentName))
                .Returns(startingEquipmentForBillOfProcess);
            billOfProcessMock.Setup(x => x.GetBillOfProcessesByStartingProcessId(startingEquipmentForBillOfProcess.Process.ProcessId))
                .Returns(simpleBillOfProcessList);

            using (var billOfProcessLogic = new BillOfProcessLogic(billOfProcessMock.Object, equipmentMock.Object))
            {
                // Act
                var billOfProcesses = billOfProcessLogic.GetBillOfProcessesByStartingEquipmentName(equipmentName);

                // Assert
                Assert.That(billOfProcesses != null && billOfProcesses.Count > 0);
            }
        }

        [TestCase("311")]
        public void IfGivenAnInvalidStartingEquipmentNameThenReturnEmptyBillOfProcessList(string equipmentName)
        {
            // Arrange
            equipmentMock.Setup(x => x.GetEquipmentByName(equipmentName))
                .Returns(nullEquipment);
            billOfProcessMock.Setup(x => x.GetBillOfProcessesByStartingProcessId(startingEquipmentForBillOfProcess.Process.ProcessId))
                .Returns(emptyBillOfProcessList);

            using (var billOfProcessLogic = new BillOfProcessLogic(billOfProcessMock.Object, equipmentMock.Object))
            {
                // Act
                var billOfProcesses = billOfProcessLogic.GetBillOfProcessesByStartingEquipmentName(equipmentName);

                // Assert
                Assert.That(billOfProcesses != null && billOfProcesses.Count == 0);
            }
        }

        #endregion

        #region Add BillOfProcess

        [Test]
        public void IfAddingANewBillOfProcessAndThePartDoesNotHaveAnExistingThenVerifyAddBillOfProcessIsCalled()
        {
            // Arrange
            billOfProcessMock.Setup(x => x.GetBillOfProcessByPartId(validNewBillOfProcess.Part.PartId))
                .Returns(nullBillOfProcess);

            billOfProcessMock.Setup(x => x.AddBillOfProcess(validNewBillOfProcess));

            using (var billOfProcessLogic = new BillOfProcessLogic(billOfProcessMock.Object, equipmentMock.Object))
            {
                // Act
                billOfProcessLogic.AddBillOfProcess(validNewBillOfProcess);

                // Assert
                billOfProcessMock.Verify(mock => mock.AddBillOfProcess(validNewBillOfProcess), Times.Once());
            }
        }

        [Test]
        public void IfAddingANewBillOfProcessAndThePartAlreadyHasAnExistingThenReturnInvalidDataException()
        {
            // Arrange
            billOfProcessMock.Setup(x => x.GetBillOfProcessByPartId(invalidNewBillOfProcess.Part.PartId))
                .Returns(invalidNewBillOfProcess);

            billOfProcessMock.Setup(x => x.AddBillOfProcess(invalidNewBillOfProcess));

            using (var billOfProcessLogic = new BillOfProcessLogic(billOfProcessMock.Object, equipmentMock.Object))
            {
                // Act
                var existingBillOfProcessException = Assert.Throws<InvalidDataException>(() => billOfProcessLogic.AddBillOfProcess(invalidNewBillOfProcess));

                // Assert
                Assert.That(existingBillOfProcessException.Message, Is.EqualTo(string.Format("Bill Of Process {0} already exists", invalidNewBillOfProcess.Name)));
                billOfProcessMock.Verify(mock => mock.AddBillOfProcess(invalidNewBillOfProcess), Times.Never);
            }
        }

        #endregion

        #region Update BillOfProcess

        [Test]
        public void IfUpdatingAnExistingBillOfProcessThenVerifyUpdateBillOfProcessIsCalled()
        {
            // Arrange
            billOfProcessMock.Setup(x => x.UpdateBillOfProcess(simpleBillOfProcessWithNoProcesses));

            using (var billOfProcessLogic = new BillOfProcessLogic(billOfProcessMock.Object, equipmentMock.Object))
            {
                // Act
                billOfProcessLogic.UpdateBillOfProcess(simpleBillOfProcessWithNoProcesses);

                // Assert
                billOfProcessMock.Verify(mock => mock.UpdateBillOfProcess(simpleBillOfProcessWithNoProcesses), Times.Once());
            }
        }

        #endregion

        #region Test cases just for coverage, no logic called

        [TestCase]
        public void IfGivenNullBillOfProcessDataClassThenReturnArgumentNullException()
        {
            // Arrange
            // Nothing to arrange

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Attempt to act
                using (var billOfProcessLogic = new BillOfProcessLogic(null, equipmentMock.Object))
                {
                    var billOfProcessList = billOfProcessLogic.GetBillOfProcessById(1);
                }
            });
        }

        [TestCase]
        public void IfGivenNullEquipmentDataClassThenReturnArgumentNullException()
        {
            // Arrange
            // Nothing to arrange

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Attempt to act
                using (var billOfProcessLogic = new BillOfProcessLogic(billOfProcessMock.Object,null))
                {
                    var billOfProcessList = billOfProcessLogic.GetBillOfProcessById(1);
                }
            });
        }

        #endregion
    }
}
