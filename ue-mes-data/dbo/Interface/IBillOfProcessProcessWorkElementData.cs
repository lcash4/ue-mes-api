using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IBillOfProcessProcessWorkElementData
    {
        /// <summary>
        /// Returns a full <see cref="BillOfProcessProcessWorkElement"/> entity with child entities included.
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementId">Unique Bill of Process Process Work Element ID for the <see cref="BillOfProcessProcessWorkElement"/></param>
        /// <returns></returns>
        public BillOfProcessProcessWorkElement GetBillOfProcessProcessWorkElementById(int billOfProcessProcessWorkElementId);

        /// <summary>
        /// Returns a list of <see cref="BillOfProcessProcessWorkElement"/> entities with child entities included.
        /// </summary>
        /// <param name="billOfProcessProcessId">Unique Bill of Process Process ID for the <see cref="BillOfProcessProcessWorkElement"/> entities to return</param>
        /// <returns></returns>
        public List<BillOfProcessProcessWorkElement> GetBillOfProcessProcessWorkElementsByBillOfProcessProcessId(int billOfProcessProcessId);

        /// <summary>
        /// Add a new bill of process process work element
        /// </summary>
        /// <param name="billOfProcessProcessWorkElement"></param>
        public void AddBillOfProcessProcessWorkElement(BillOfProcessProcessWorkElement billOfProcessProcessWorkElement);

        /// <summary>
        /// Add a range of new bill of process process work elements
        /// </summary>
        /// <param name="billOfProcessProcessWorkElements"></param>
        public void AddBillOfProcessProcessWorkElements(List<BillOfProcessProcessWorkElement> billOfProcessProcessWorkElements);

        /// <summary>
        /// Update an existing bill of process process work element.  
        /// </summary>
        /// <param name="billOfProcessProcessWorkElement"></param>
        public void UpdateBillOfProcessProcessWorkElement(BillOfProcessProcessWorkElement billOfProcessProcessWorkElement);

        /// <summary>
        /// Will delete all <see cref="BillOfProcessProcessWorkElement"/> records that match the incoming BOP Process Id
        /// </summary>
        /// <param name="billOfProcessProcessId"></param>
        public void DeleteBillOfProcessProcessWorkElements(int billOfProcessProcessId);

        /// <summary>
        /// Will delete all <see cref="BillOfProcessProcessWorkElement"/> records that match the incoming BOP Process Work Element Ids
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementIds"></param>
        public void DeleteBillOfProcessProcessWorkElementsById(List<int> billOfProcessProcessWorkElementIds);

        /// <summary>
        /// Will set all <see cref="BillOfProcessProcessWorkElement"/> records that match the incoming BOP Process Work Element Ids to IsActive = false
        /// They have to be deactivated instead of deleted because they have production data associated with them.
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementIds"></param>
        public void DeactivateBillOfProcessProcessWorkElementsById(List<int> billOfProcessProcessWorkElementIds);
    }
}
