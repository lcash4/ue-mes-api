using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_data.Global.Interface;
using ue_mes_entities.dbo;
using ue_mes_logic.dbo;
using ue_mes_test.FakeObjects;

namespace ue_mes_test.dbo
{
    public class BillOfProcessProcessLogicTest
    {
        // Mock
        private Mock<IBillOfProcessProcessData> billOfProcessProcessMock;
        private Mock<IBillOfProcessProcessWorkElementData> billOfProcessProcessWorkElementMock;

        // Fakes
        private BillOfProcessProcess simpleBillOfProcessProcessWithNoWorkElements;
        private static BillOfProcessProcess validNewBillOfProcessProcess;
        private BillOfProcessProcess invalidNewBillOfProcessProcess;
        private BillOfProcessProcess nullBillOfProcessProcess = null;
        private List<BillOfProcessProcess> simpleBillOfProcessProcessList;
        private List<BillOfProcessProcess> emptyBillOfProcessProcessList = new List<BillOfProcessProcess>();

        [SetUp]
        public void Setup()
        {
            simpleBillOfProcessProcessWithNoWorkElements = BillOfProcessProcessFake.GetSimpleBillOfProcessProcessWithNoWorkElements();
            validNewBillOfProcessProcess = BillOfProcessProcessFake.GetValidNewBillOfProcessProcess();
            invalidNewBillOfProcessProcess = BillOfProcessProcessFake.GetInvalidNewBillOfProcessProcess();
            simpleBillOfProcessProcessList = BillOfProcessProcessFake.GetSimpleBillOfProcessProcessList();

            billOfProcessProcessMock = new Mock<IBillOfProcessProcessData>();
            billOfProcessProcessWorkElementMock = new Mock<IBillOfProcessProcessWorkElementData>();
        }

        #region Get by BillOfProcessProcess ID

        [TestCase(1)]
        public void IfGivenAValidBillOfProcessProcessIdThenReturnBillOfProcessProcessObjectWithData(int billOfProcessProcessId)
        {
            // Arrange
            billOfProcessProcessMock.Setup(x => x.GetBillOfProcessProcessById(billOfProcessProcessId))
                .Returns(simpleBillOfProcessProcessWithNoWorkElements);

            using (var billOfProcessProcessLogic = new BillOfProcessProcessLogic(billOfProcessProcessMock.Object, billOfProcessProcessWorkElementMock.Object))
            {
                // Act
                var billOfProcessProcess = billOfProcessProcessLogic.GetBillOfProcessProcessById(billOfProcessProcessId);

                // Assert
                Assert.That(billOfProcessProcess != null && billOfProcessProcess.BillOfProcessProcessId == billOfProcessProcessId);
            }
        }

        [TestCase(-1)]
        public void IfGivenAnInvalidBillOfProcessProcessIdForAnActiveBillOfProcessProcessThenReturnNull(int billOfProcessProcessId)
        {
            // Arrange
            billOfProcessProcessMock.Setup(x => x.GetBillOfProcessProcessById(billOfProcessProcessId))
                .Returns(nullBillOfProcessProcess);

            using (var billOfProcessProcessLogic = new BillOfProcessProcessLogic(billOfProcessProcessMock.Object, billOfProcessProcessWorkElementMock.Object))
            {
                // Act
                var billOfProcessProcess = billOfProcessProcessLogic.GetBillOfProcessProcessById(billOfProcessProcessId);

                // Assert
                Assert.Null(billOfProcessProcess);
            }
        }

        #endregion

        #region Add BillOfProcessProcess

        [Test]
        public void IfAddingANewBillOfProcessProcessAndTheBillOfProcessDoesNotHaveAnExistingProcessThenVerifyAddBillOfProcessProcessIsCalled()
        {
            // Arrange
            billOfProcessProcessMock.Setup(x => x.GetBillOfProcessProcessByBillOfProcessIdAndProcessId(validNewBillOfProcessProcess.BillOfProcess.BillOfProcessId, validNewBillOfProcessProcess.Process.ProcessId))
                .Returns(nullBillOfProcessProcess);

            billOfProcessProcessMock.Setup(x => x.AddBillOfProcessProcess(validNewBillOfProcessProcess));

            using (var billOfProcessProcessLogic = new BillOfProcessProcessLogic(billOfProcessProcessMock.Object, billOfProcessProcessWorkElementMock.Object))
            {
                // Act
                billOfProcessProcessLogic.AddBillOfProcessProcess(validNewBillOfProcessProcess);

                // Assert
                billOfProcessProcessMock.Verify(mock => mock.AddBillOfProcessProcess(validNewBillOfProcessProcess), Times.Once());
            }
        }

        [Test]
        public void IfAddingANewBillOfProcessProcessAndTheBillOfProcessHasAnExistingProcessThenVerifyInvalidDataExceptionIsThrown()
        {
            // Arrange
            billOfProcessProcessMock.Setup(x => x.GetBillOfProcessProcessByBillOfProcessIdAndProcessId(invalidNewBillOfProcessProcess.BillOfProcess.BillOfProcessId, invalidNewBillOfProcessProcess.Process.ProcessId))
                .Returns(invalidNewBillOfProcessProcess);

            billOfProcessProcessMock.Setup(x => x.AddBillOfProcessProcess(invalidNewBillOfProcessProcess));

            using (var billOfProcessProcessLogic = new BillOfProcessProcessLogic(billOfProcessProcessMock.Object, billOfProcessProcessWorkElementMock.Object))
            {
                // Act
                var existingBillOfProcessProcessException = Assert.Throws<InvalidDataException>(() => billOfProcessProcessLogic.AddBillOfProcessProcess(invalidNewBillOfProcessProcess));

                // Assert
                Assert.That(existingBillOfProcessProcessException.Message, Is.EqualTo(String.Format("Process for Bill of Process ID {0} and Process Id {1} already exists", invalidNewBillOfProcessProcess.BillOfProcess.BillOfProcessId, invalidNewBillOfProcessProcess.Process.ProcessId)));
                billOfProcessProcessMock.Verify(mock => mock.AddBillOfProcessProcess(invalidNewBillOfProcessProcess), Times.Never);
            }
        }

        #endregion

        #region Update BillOfProcessProcess

        [Test]
        public void IfUpdatingAnExistingBillOfProcessProcessThenVerifyUpdateBillOfProcessProcessIsCalled()
        {
            // Arrange
            billOfProcessProcessMock.Setup(x => x.UpdateBillOfProcessProcess(simpleBillOfProcessProcessWithNoWorkElements));

            using (var billOfProcessProcessLogic = new BillOfProcessProcessLogic(billOfProcessProcessMock.Object, billOfProcessProcessWorkElementMock.Object))
            {
                // Act
                billOfProcessProcessLogic.UpdateBillOfProcessProcess(simpleBillOfProcessProcessWithNoWorkElements);

                // Assert
                billOfProcessProcessMock.Verify(mock => mock.UpdateBillOfProcessProcess(simpleBillOfProcessProcessWithNoWorkElements), Times.Once());
            }
        }

        #endregion

        #region Test cases just for coverage, no logic called

        [TestCase]
        public void IfGivenNullBillOfProcessProcessDataClassThenReturnArgumentNullException()
        {
            // Arrange
            // Nothing to arrange

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Attempt to act
                using (var billOfProcessProcessLogic = new BillOfProcessProcessLogic(null, billOfProcessProcessWorkElementMock.Object))
                {
                    var billOfProcessProcessList = billOfProcessProcessLogic.GetBillOfProcessProcessById(1);
                }
            });
        }

        [TestCase]
        public void IfGivenNullBillOfProcessProcessWorkElementDataClassThenReturnArgumentNullException()
        {
            // Arrange
            // Nothing to arrange

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Attempt to act
                using (var billOfProcessProcessLogic = new BillOfProcessProcessLogic(billOfProcessProcessMock.Object, null))
                {
                    var billOfProcessProcessList = billOfProcessProcessLogic.GetBillOfProcessProcessById(1);
                }
            });
        }

        #endregion
    }
}
