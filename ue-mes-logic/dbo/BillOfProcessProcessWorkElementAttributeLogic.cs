using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_logic.dbo
{
    public class BillOfProcessProcessWorkElementAttributeLogic : LogicBase
    {
        IBillOfProcessProcessWorkElementAttributeData BillOfProcessProcessWorkElementAttributeData { get; }

        #region Constructor

        public BillOfProcessProcessWorkElementAttributeLogic(IBillOfProcessProcessWorkElementAttributeData billOfProcessProcessWorkElementAttributeData)
        {
            BillOfProcessProcessWorkElementAttributeData = billOfProcessProcessWorkElementAttributeData ?? throw new ArgumentNullException(nameof(billOfProcessProcessWorkElementAttributeData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a full <see cref="BillOfProcessProcessWorkElementAttribute"/> entity with child entities included.
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementAttributeId">Unique Bill of Process Process Work Element Attribute ID for the <see cref="BillOfProcessProcessWorkElementAttribute"/></param>
        /// <returns></returns>
        public BillOfProcessProcessWorkElementAttribute GetBillOfProcessProcessWorkElementAttributeById(int billOfProcessProcessWorkElementAttributeId)
        {
            return BillOfProcessProcessWorkElementAttributeData.GetBillOfProcessProcessWorkElementAttributeById(billOfProcessProcessWorkElementAttributeId);
        }

        /// <summary>
        /// Update an existing bill of process process work element attribute.  
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementAttribute"></param>
        public void UpdateBillOfProcessProcessWorkElementAttribute(BillOfProcessProcessWorkElementAttribute billOfProcessProcessWorkElementAttribute)
        {
            BillOfProcessProcessWorkElementAttributeData.UpdateBillOfProcessProcessWorkElementAttribute(billOfProcessProcessWorkElementAttribute);
        }

        #endregion
    }
}
