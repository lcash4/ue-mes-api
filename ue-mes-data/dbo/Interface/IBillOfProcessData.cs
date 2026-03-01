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
    public interface IBillOfProcessData
    {
        /// <summary>
        /// Returns a full <see cref="BillOfProcess"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="billOfProcessId">Unique Bill of Process ID for the <see cref="BillOfProcess"/></param>
        /// <returns></returns>
        public BillOfProcess GetBillOfProcessById(int billOfProcessId);

        /// <summary>
        /// Returns a full <see cref="BillOfProcess"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="partId">Unique Part ID for the <see cref="BillOfProcess"/></param>
        /// <returns></returns>
        public BillOfProcess GetBillOfProcessByPartId(int partId);

        /// <summary>
        /// Returns a list of <see cref="BillOfProcess"/> entities with part child entities included.
        /// The starting process ID is used to find BOPs where the process sequence is 1 and matches the process.  This indicates an process where an order item unit can be started.
        /// </summary>
        /// <param name="processId">Unique Process ID for the <see cref="BillOfProcess"/></param>
        /// <returns></returns>
        public List<BillOfProcess> GetBillOfProcessesByStartingProcessId(int processId);

        /// <summary>
        /// Add a new bill of process
        /// </summary>
        /// <param name="billOfProcess"></param>
        public void AddBillOfProcess(BillOfProcess billOfProcess);

        /// <summary>
        /// Update an existing bill of process.  
        /// </summary>
        /// <param name="billOfProcess"></param>
        public void UpdateBillOfProcess(BillOfProcess billOfProcess);
    }
}
