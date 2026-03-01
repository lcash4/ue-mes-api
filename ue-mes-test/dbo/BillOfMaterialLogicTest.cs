using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Authorization.Interface;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_logic.Authorization;
using ue_mes_logic.dbo;
using ue_mes_test.FakeObjects;

namespace ue_mes_test.dbo
{
    public class BillOfMaterialLogicTest
    {
        // Mock
        private Mock<IBillOfMaterialData> billOfMaterialMock;

        // Fakes
        private BillOfMaterial simpleBillOfMaterialWithNoParts;
        private static BillOfMaterial validNewBillOfMaterial;
        private BillOfMaterial invalidNewBillOfMaterial;
        private BillOfMaterial nullBillOfMaterial = null;
        
        [SetUp]
        public void Setup()
        {
            simpleBillOfMaterialWithNoParts = BillOfMaterialFake.GetSimpleBillOfMaterialWithNoParts();
            validNewBillOfMaterial = BillOfMaterialFake.GetValidNewBillOfMaterial();
            invalidNewBillOfMaterial = BillOfMaterialFake.GetInvalidNewBillOfMaterial();

            billOfMaterialMock = new Mock<IBillOfMaterialData>();
        }

        #region Get by BillOfMaterial ID

        [TestCase(1)]
        public void IfGivenAValidBillOfMaterialIdThenReturnBillOfMaterialObjectWithData(int billOfMaterialId)
        {
            // Arrange
            billOfMaterialMock.Setup(x => x.GetBillOfMaterialById(billOfMaterialId))
                .Returns(simpleBillOfMaterialWithNoParts);

            using (var billOfMaterialLogic = new BillOfMaterialLogic(billOfMaterialMock.Object))
            {
                // Act
                var billOfMaterial = billOfMaterialLogic.GetBillOfMaterialById(billOfMaterialId);

                // Assert
                Assert.That(billOfMaterial != null && billOfMaterial.BillOfMaterialId == billOfMaterialId);
            }
        }

        [TestCase(-1)]
        public void IfGivenAnInvalidBillOfMaterialIdForAnActiveBillOfMaterialThenReturnNull(int billOfMaterialId)
        {
            // Arrange
            billOfMaterialMock.Setup(x => x.GetBillOfMaterialById(billOfMaterialId))
                .Returns(nullBillOfMaterial);

            using (var billOfMaterialLogic = new BillOfMaterialLogic(billOfMaterialMock.Object))
            {
                // Act
                var billOfMaterial = billOfMaterialLogic.GetBillOfMaterialById(billOfMaterialId);

                // Assert
                Assert.Null(billOfMaterial);
            }
        }

        #endregion

        #region Get by Part ID

        [TestCase(2)]
        public void IfGivenAValidBillOfMaterialPartIdThenReturnBillOfMaterialObjectWithData(int partId)
        {
            // Arrange
            billOfMaterialMock.Setup(x => x.GetBillOfMaterialByPartId(partId))
                .Returns(simpleBillOfMaterialWithNoParts);

            using (var billOfMaterialLogic = new BillOfMaterialLogic(billOfMaterialMock.Object))
            {
                // Act
                var billOfMaterial = billOfMaterialLogic.GetBillOfMaterialByPartId(partId);

                // Assert
                Assert.That(billOfMaterial != null && billOfMaterial.Part.PartId == partId);
            }
        }

        [TestCase(-1)]
        public void IfGivenAnInalidBillOfMaterialPartIdThenReturnNull(int partId)
        {
            // Arrange
            billOfMaterialMock.Setup(x => x.GetBillOfMaterialByPartId(partId))
                .Returns(nullBillOfMaterial);

            using (var billOfMaterialLogic = new BillOfMaterialLogic(billOfMaterialMock.Object))
            {
                // Act
                var billOfMaterial = billOfMaterialLogic.GetBillOfMaterialByPartId(partId);

                // Assert
                Assert.Null(billOfMaterial);
            }
        }

        #endregion

        #region Add BillOfMaterial

        [Test]
        public void IfAddingANewBillOfMaterialAndThePartDoesNotHaveAnExistingThenVerifyAddBillOfMaterialIsCalled()
        {
            // Arrange
            billOfMaterialMock.Setup(x => x.GetBillOfMaterialByPartId(validNewBillOfMaterial.Part.PartId))
                .Returns(nullBillOfMaterial);

            billOfMaterialMock.Setup(x => x.AddBillOfMaterial(validNewBillOfMaterial));

            using (var billOfMaterialLogic = new BillOfMaterialLogic(billOfMaterialMock.Object))
            {
                // Act
                billOfMaterialLogic.AddBillOfMaterial(validNewBillOfMaterial);

                // Assert
                billOfMaterialMock.Verify(mock => mock.AddBillOfMaterial(validNewBillOfMaterial), Times.Once());
            }
        }

        [Test]
        public void IfAddingANewBillOfMaterialAndThePartAlreadyHasAnExistingThenReturnInvalidDataException()
        {
            // Arrange
            billOfMaterialMock.Setup(x => x.GetBillOfMaterialByPartId(invalidNewBillOfMaterial.Part.PartId))
                .Returns(invalidNewBillOfMaterial);

            billOfMaterialMock.Setup(x => x.AddBillOfMaterial(invalidNewBillOfMaterial));

            using (var billOfMaterialLogic = new BillOfMaterialLogic(billOfMaterialMock.Object))
            {
                // Act
                var existingBillOfMaterialException = Assert.Throws<InvalidDataException>(() => billOfMaterialLogic.AddBillOfMaterial(invalidNewBillOfMaterial));

                // Assert
                Assert.That(existingBillOfMaterialException.Message, Is.EqualTo(string.Format("Bill Of Material {0} already exists", invalidNewBillOfMaterial.Name)));
                billOfMaterialMock.Verify(mock => mock.AddBillOfMaterial(invalidNewBillOfMaterial), Times.Never);
            }
        }

        #endregion

        #region Update BillOfMaterial

        [Test]
        public void IfUpdatingAnExistingBillOfMaterialThenVerifyUpdateBillOfMaterialIsCalled()
        {
            // Arrange
            billOfMaterialMock.Setup(x => x.UpdateBillOfMaterial(simpleBillOfMaterialWithNoParts));

            using (var billOfMaterialLogic = new BillOfMaterialLogic(billOfMaterialMock.Object))
            {
                // Act
                billOfMaterialLogic.UpdateBillOfMaterial(simpleBillOfMaterialWithNoParts);

                // Assert
                billOfMaterialMock.Verify(mock => mock.UpdateBillOfMaterial(simpleBillOfMaterialWithNoParts), Times.Once());
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
                using (var billOfMaterialLogic = new BillOfMaterialLogic(null))
                {
                    var billOfMaterialList = billOfMaterialLogic.GetBillOfMaterialById(1);
                }
            });
        }

        #endregion
    }
}
