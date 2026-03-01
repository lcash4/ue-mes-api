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
    public class ProcessLogicTest
    {
        // Mock
        private Mock<IProcessData> processMock;

        // Fakes
        private List<Process> validProcessList;
        private List<Process> processListNotInBillOfProcess;

        [SetUp]
        public void Setup()
        {
            validProcessList = ProcessFake.GetValidProcessList();
            processListNotInBillOfProcess = ProcessFake.GetValidProcessListNotInBillOfProcess();

            processMock = new Mock<IProcessData>();
        }

        [Test]
        public void IfGettingAllProcessesThenReturnProcessListWithData()
        {
            // Arrange
            processMock.Setup(x => x.GetProcesses())
                .Returns(validProcessList);

            using (var processLogic = new ProcessLogic(processMock.Object))
            {
                // Act
                var processList = processLogic.GetProcesses();

                // Assert
                Assert.That(processList.Count > 0);
            }
        }

        [TestCase("PHX",1)]
        public void IfGettingProcessesNotInBillOfProcessThenReturnProcessListWithData(string localSiteName, int billOfProcessId)
        {
            // Arrange
            processMock.Setup(x => x.GetProcessesNotInBillOfProcessForLocalSite(localSiteName, billOfProcessId))
                .Returns(processListNotInBillOfProcess);

            using (var processLogic = new ProcessLogic(processMock.Object))
            {
                // Act
                var processList = processLogic.GetProcessesNotInBillOfProcessForLocalSite(localSiteName,billOfProcessId);

                // Assert
                Assert.That(processList.Count > 0);
            }
        }

        #region Test cases just for coverage, no logic called

        [Test]
        public void IfGivenNullDataClassThenReturnArgumentNullException()
        {
            // Arrange
            // Nothing to arrange

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Attempt to act
                using (var processLogic = new ProcessLogic(null))
                {
                    var processList = processLogic.GetProcesses();
                }
            });
        }

        #endregion

    }
}
