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
    public interface IBillOfProcessProcessData
    {
        /// <summary>
        /// Returns a full <see cref="BillOfProcessProcess"/> entity with child entities included.
        /// </summary>
        /// <param name="billOfProcessProcessId">Unique Bill of Process Process ID for the <see cref="BillOfProcessProcess"/></param>
        /// <returns></returns>
        public BillOfProcessProcess GetBillOfProcessProcessById(int billOfProcessProcessId);

        /// <summary>
        /// Returns a list of <see cref="BillOfProcessProcess"/> entities with child entities included.
        /// </summary>
        /// <param name="billOfProcessId">Unique Bill of Process ID to search.</param>
        /// <returns></returns>
        public List<BillOfProcessProcess> GetBillOfProcessProcessesByBillOfProcessId(int billOfProcessId);

        /// <summary>
        /// Returns a full <see cref="BillOfProcessProcess"/> entity with child entities included.
        /// This method will primarily be used to validate adding a new Bill of Process Process and preventing duplicates
        /// </summary>
        /// <param name="billOfProcesId">Unique Bill of Process ID for the <see cref="BillOfProcessProcess"/></param>
        /// <param name="processId">Unique Process ID for the <see cref="BillOfProcessProcess"/></param>
        /// <returns></returns>
        public BillOfProcessProcess GetBillOfProcessProcessByBillOfProcessIdAndProcessId(int billOfProcesId, int processId);
        
            /// <summary>
        /// Returns a list of entities with BOP Process IDs, their work elements and whether those elements have any order item unit history.
        /// </summary>
        /// <param name="billOfProcessId">Unique Bill of Process ID to search.</param>
        /// <returns></returns>
        public List<(int BillOfProcessId, int BillOfProcessProcessId, int ProcessId, int WorkElementId, bool WorkElementIsActive, int WorkElementHistoryCount)> GetBillOfProcessProcessesWithWorkElementHistoryByBillOfProcessId(int billOfProcessId);

        /// <summary>
        /// Add a new bill of process process
        /// </summary>
        /// <param name="billOfProcessProcess"></param>
        public void AddBillOfProcessProcess(BillOfProcessProcess billOfProcessProcess);

        /// <summary>
        /// Add a range of new bill of process process
        /// </summary>
        /// <param name="billOfProcessProcesses"></param>
        public void AddBillOfProcessProcesses(List<BillOfProcessProcess> billOfProcessProcesses);

        /// <summary>
        /// Add a range of new bill of process process
        /// </summary>
        /// <param name="billOfProcessProcesses"></param>
        public void AddBillOfProcessProcessesWithWorkElements(List<BillOfProcessProcess> billOfProcessProcesses);

        /// <summary>
        /// Update an existing bill of process process.  
        /// </summary>
        /// <param name="billOfProcessProcess"></param>
        public void UpdateBillOfProcessProcess(BillOfProcessProcess billOfProcessProcess);

        /// <summary>
        /// Will delete all <see cref="BillOfProcessProcess"/> records that match the incoming BillOfProcessId
        /// </summary>
        /// <param name="billOfProcessId"></param>
        public void DeleteBillOfProcessProcesses(int billOfProcessId);


        /// <summary>
        /// Will delete all <see cref="BillOfProcessProcess"/> records that match the incoming BOP Process Ids 
        /// </summary>
        /// <param name="billOfProcessProcessIds"></param>
        public void DeleteBillOfProcessProcessesById(List<int> billOfProcessProcessIds);

        /// <summary>
        /// Will set all <see cref="BillOfProcessProcess"/> records that match the incoming BOP Process Ids to IsActive = false
        /// They have to be deactivated instead of deleted because they have production data associated with them.
        /// </summary>
        /// <param name="billOfProcessProcessIds"></param>
        public void DeactivateBillOfProcessProcessesById(List<int> billOfProcessProcessIds);
    }
}
