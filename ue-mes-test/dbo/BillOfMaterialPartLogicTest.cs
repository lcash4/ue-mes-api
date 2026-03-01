using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_logic.dbo;
using ue_mes_test.FakeObjects;

namespace ue_mes_test.dbo
{
    public class BillOfMaterialPartLogicTest
    {
        // Mock
        private Mock<IBillOfMaterialPartData> billOfMaterialPartMock;

        // Fakes
        private BillOfMaterial simpleBillOfMaterialWithParts;

        [SetUp]
        public void Setup()
        {
            simpleBillOfMaterialWithParts = BillOfMaterialFake.GetSimpleBillOfMaterialWithParts();

            billOfMaterialPartMock = new Mock<IBillOfMaterialPartData>();
        }

        #region Update BillOfMaterial Parts

        [Test]
        public void IfUpdatingAnExistingBillOfMaterialsPartsThenVerifyDeleteAndAddIsCalled()
        {
            // Arrange
            billOfMaterialPartMock.Setup(x => x.AddBillOfMaterialParts(simpleBillOfMaterialWithParts.BillOfMaterialParts));
            billOfMaterialPartMock.Setup(x => x.DeleteBillOfMaterialParts(simpleBillOfMaterialWithParts.BillOfMaterialId));

            using (var billOfMaterialPartLogic = new BillOfMaterialPartLogic(billOfMaterialPartMock.Object))
            {
                // Act
                billOfMaterialPartLogic.UpdateBillOfMaterialPartsList(simpleBillOfMaterialWithParts.BillOfMaterialId, simpleBillOfMaterialWithParts.BillOfMaterialParts);

                // Assert
                billOfMaterialPartMock.Verify(mock => mock.AddBillOfMaterialParts(simpleBillOfMaterialWithParts.BillOfMaterialParts), Times.Once());
                billOfMaterialPartMock.Verify(mock => mock.DeleteBillOfMaterialParts(simpleBillOfMaterialWithParts.BillOfMaterialId), Times.Once());
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
                using (var billOfMaterialPartLogic = new BillOfMaterialPartLogic(null))
                {
                    billOfMaterialPartLogic.UpdateBillOfMaterialPartsList(simpleBillOfMaterialWithParts.BillOfMaterialId, simpleBillOfMaterialWithParts.BillOfMaterialParts);
                }
            });
        }

        #endregion
    }
}
