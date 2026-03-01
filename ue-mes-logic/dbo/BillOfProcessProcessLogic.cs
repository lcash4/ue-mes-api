using System.Linq;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_data.Global.Interface;
using ue_mes_data.Global;
using ue_mes_entities.dbo;

namespace ue_mes_logic.dbo
{
    public class BillOfProcessProcessLogic : LogicBase
    {
        IBillOfProcessProcessData BillOfProcessProcessData { get; }
        IBillOfProcessProcessWorkElementData BillOfProcessProcessWorkElementData { get; }

        #region Constructor

        public BillOfProcessProcessLogic(IBillOfProcessProcessData billOfProcessProcessData, 
            IBillOfProcessProcessWorkElementData billOfProcessProcessWorkElementData)
        {
            BillOfProcessProcessData = billOfProcessProcessData ?? throw new ArgumentNullException(nameof(billOfProcessProcessData));
            BillOfProcessProcessWorkElementData = billOfProcessProcessWorkElementData ?? throw new ArgumentNullException(nameof(billOfProcessProcessWorkElementData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a full <see cref="BillOfProcessProcess"/> entity with child entities included.
        /// </summary>
        /// <param name="billOfProcessProcessId">Unique Bill of Process Process ID for the <see cref="BillOfProcessProcess"/></param>
        /// <returns></returns>
        public BillOfProcessProcess GetBillOfProcessProcessById(int billOfProcessProcessId)
        {
            return BillOfProcessProcessData.GetBillOfProcessProcessById(billOfProcessProcessId);
        }

        /// <summary>
        /// Add a new bill of process process
        /// </summary>
        /// <param name="billOfProcessProcess"></param>
        public void AddBillOfProcessProcess(BillOfProcessProcess billOfProcessProcess)
        {
            // Need to check if the process already exists for the provided BillOfProcess and ProcessId.  If so, return "Process already exists" exception
            var existingBillOfProcessProcess = BillOfProcessProcessData.GetBillOfProcessProcessByBillOfProcessIdAndProcessId(billOfProcessProcess.BillOfProcess.BillOfProcessId, billOfProcessProcess.Process.ProcessId);
            if (existingBillOfProcessProcess != null)
                throw new InvalidDataException(String.Format("Process for Bill of Process ID {0} and Process Id {1} already exists", billOfProcessProcess.BillOfProcess.BillOfProcessId, billOfProcessProcess.Process.ProcessId));

            BillOfProcessProcessData.AddBillOfProcessProcess(billOfProcessProcess);
        }

        /// <summary>
        /// Update an existing bill of process process.  
        /// </summary>
        /// <param name="billOfProcessProcess"></param>
        public void UpdateBillOfProcessProcess(BillOfProcessProcess billOfProcessProcess)
        {
            BillOfProcessProcessData.UpdateBillOfProcessProcess(billOfProcessProcess);
        }

        /// <summary>
        /// Replace all existing bill of process processes with a new set.
        /// </summary>
        /// <param name="billOfProcessId">ID of the bill of process to update</param>
        /// <param name="billOfProcessProcesses">The new list of processes to assign to this bill of process</param>
        public void UpdateBillOfProcessProcessList(int billOfProcessId, List<BillOfProcessProcess> billOfProcessProcesses, string imagesFolder)
        {
            // Before we delete any records, we grab a copy of the existing data so that we can update any work elements later.
            List<int> billOfProcessProcessesToDelete = new List<int>();
            List<int> billOfProcessProcessesToDeactivate = new List<int>();

            // We also need to handle any work elements that already have production data.  If they do, they will simply be marked inactive.  If not, they can be deleted, or updated to the new ID if the process remains in the list.
            List<int> billOfProcessWorkElementsToDelete = new List<int>();
            List<int> billOfProcessWorkElementsToUpdate = new List<int>();
            List<int> billOfProcessWorkElementsToDeactivate = new List<int>();

            // Capture the BOP Processes before any changes, so that we can make updates after the new list is saved.
            var billOfProcessProcessesWithWorkElementHistoryPriorToUpdate = BillOfProcessProcessData.GetBillOfProcessProcessesWithWorkElementHistoryByBillOfProcessId(billOfProcessId);
            var billOfProcessProcessesWithWorkElementsPriorToUpdate = BillOfProcessProcessData.GetBillOfProcessProcessesByBillOfProcessId(billOfProcessId);

            // If there are work elements, we can either delete them, update them or mark them inactive.  We need to check history to decide.
            DispositionBillOfProcessProcessWorkElements(billOfProcessProcesses, billOfProcessWorkElementsToDelete, billOfProcessWorkElementsToUpdate, billOfProcessWorkElementsToDeactivate, billOfProcessProcessesWithWorkElementHistoryPriorToUpdate);

            // Now, with the work elements set, we can make decisions on the BOP processes as well.
            DispositionBillOfProcessProcesses(billOfProcessProcessesToDelete, billOfProcessProcessesToDeactivate, billOfProcessProcessesWithWorkElementHistoryPriorToUpdate);

            // If a BOP Process is remaining in the list, we need to re-hydrate its work elements with the previous values.  
            // When the new BOP Process is added, the existing IDs will be cleared in the data layer to allow them to be added as new entries.
            billOfProcessProcesses.ForEach(billOfProcessProcess =>
            {
                var existingBillOfProcessProcess = billOfProcessProcessesWithWorkElementsPriorToUpdate.Where(billOfProcessProcessWithWorkElementsPriorToUpdate => billOfProcessProcessWithWorkElementsPriorToUpdate.Process.ProcessId == billOfProcessProcess.Process.ProcessId).FirstOrDefault();

                if (existingBillOfProcessProcess != null && existingBillOfProcessProcess.BillOfProcessProcessWorkElements != null && existingBillOfProcessProcess.BillOfProcessProcessWorkElements.Count > 0)
                    billOfProcessProcess.BillOfProcessProcessWorkElements = existingBillOfProcessProcess.BillOfProcessProcessWorkElements;
            });

            // Now, we can start writing to the database.  Each list generated above will be updated\deleted accordingly, followed by an add of the new BOP Processes with any work elements included.
            BillOfProcessProcessData.DeactivateBillOfProcessProcessesById(billOfProcessProcessesToDeactivate);
            BillOfProcessProcessData.DeleteBillOfProcessProcessesById(billOfProcessProcessesToDelete);

            BillOfProcessProcessWorkElementData.DeactivateBillOfProcessProcessWorkElementsById(billOfProcessWorkElementsToDeactivate);
            BillOfProcessProcessWorkElementData.DeleteBillOfProcessProcessWorkElementsById(billOfProcessWorkElementsToDelete);

            // With old BOP processes and work elements updated, we can now add the new list
            BillOfProcessProcessData.AddBillOfProcessProcessesWithWorkElements(billOfProcessProcesses);

            // Once the new list is added, any carried over work elements of type "Image" will need their images updated as well
            var newBillOfProcessProcesses = BillOfProcessProcessData.GetBillOfProcessProcessesByBillOfProcessId(billOfProcessId);

            using (var billOfProcessProcessWorkElementLogic = new BillOfProcessProcessWorkElementLogic(BillOfProcessProcessWorkElementData))
            {
                newBillOfProcessProcesses.ForEach(billOfProcessProcess =>
                {
                    var billOfProcessProcessPriorToUpdate = billOfProcessProcessesWithWorkElementsPriorToUpdate.Where(oldBillOfProcessProcess => oldBillOfProcessProcess.Process.ProcessId == billOfProcessProcess.Process.ProcessId).FirstOrDefault();
                    if (billOfProcessProcessPriorToUpdate != null
                        && billOfProcessProcess.BillOfProcessProcessWorkElements != null
                        && billOfProcessProcess.BillOfProcessProcessWorkElements.Count > 0
                        && billOfProcessProcessPriorToUpdate.BillOfProcessProcessWorkElements != null
                        && billOfProcessProcessPriorToUpdate.BillOfProcessProcessWorkElements.Count > 0)
                    {
                        billOfProcessProcessWorkElementLogic.UpdateWorkElementImages(billOfProcessProcess.BillOfProcessProcessId, billOfProcessProcess.BillOfProcessProcessWorkElements, imagesFolder, billOfProcessProcessPriorToUpdate.BillOfProcessProcessWorkElements);

                        // Finally, any in progress or paused order item unit history will need updated to the latest IDs so the operators can continue where they left off. 
                        // This data call will simply do nothing if there are no records.
                        billOfProcessProcessWorkElementLogic.UpdateWorkElementHistory(billOfProcessProcess.BillOfProcessProcessId, billOfProcessProcessPriorToUpdate.BillOfProcessProcessWorkElements);
                    }
                });
            }
        }

        private static void DispositionBillOfProcessProcesses(List<int> billOfProcessProcessesToDelete, List<int> billOfProcessProcessesToDeactivate, List<(int BillOfProcessId, int BillOfProcessProcessId, int ProcessId, int WorkElementId, bool WorkElementIsActive, int WorkElementHistoryCount)> billOfProcessProcessesWithWorkElementHistoryPriorToUpdate)
        {
            // If a BOP Process is being removed from the list, we need to mark it inactive if it has any work element history, otherwise, it can be deleted.
            // If a BOP process is being retained in the new list, we will attempt to restore the original list of work elements as well.
            billOfProcessProcessesWithWorkElementHistoryPriorToUpdate.ForEach(billOfProcessProcessWithWorkElementHistoryPriorToUpdate =>
            {
                // If we've already processes this one, then we can bail.  The list should never be very long, so performance is not an issue here.  Will refactor later if needed.
                if (billOfProcessProcessesToDelete.Contains(billOfProcessProcessWithWorkElementHistoryPriorToUpdate.BillOfProcessProcessId) || billOfProcessProcessesToDeactivate.Contains(billOfProcessProcessWithWorkElementHistoryPriorToUpdate.BillOfProcessProcessId))
                    return;

                // First, if it has any work elements with history, go ahead and add to the Deactivate list.
                if (billOfProcessProcessesWithWorkElementHistoryPriorToUpdate.Any(billOfProcessProcessWithWorkElementHistory => billOfProcessProcessWithWorkElementHistory.ProcessId == billOfProcessProcessWithWorkElementHistoryPriorToUpdate.ProcessId && billOfProcessProcessWithWorkElementHistory.WorkElementId > 0 && billOfProcessProcessWithWorkElementHistory.WorkElementHistoryCount > 0))
                {
                    billOfProcessProcessesToDeactivate.Add(billOfProcessProcessWithWorkElementHistoryPriorToUpdate.BillOfProcessProcessId);
                }
                else
                {
                    // Otherwise, it can be deleted.
                    billOfProcessProcessesToDelete.Add(billOfProcessProcessWithWorkElementHistoryPriorToUpdate.BillOfProcessProcessId);
                }
            });
        }

        private static void DispositionBillOfProcessProcessWorkElements(List<BillOfProcessProcess> billOfProcessProcesses, List<int> billOfProcessWorkElementsToDelete, List<int> billOfProcessWorkElementsToUpdate, List<int> billOfProcessWorkElementsToDeactivate, List<(int BillOfProcessId, int BillOfProcessProcessId, int ProcessId, int WorkElementId, bool WorkElementIsActive, int WorkElementHistoryCount)> billOfProcessProcessesWithWorkElementHistoryPriorToUpdate)
        {
            billOfProcessProcessesWithWorkElementHistoryPriorToUpdate.ForEach(billOfProcessProcessWithWorkElementHistoryPriorToUpdate =>
            {
                //  If there is no history, but the process is still in the new list, then any work elements will be UPDATED to the new BOP Process ID
                //if (billOfProcessProcessWithWorkElementHistoryPriorToUpdate.WorkElementHistoryCount == 0 && billOfProcessProcessWithWorkElementHistoryPriorToUpdate.WorkElementId > 0 && billOfProcessProcesses.Any(billOfProcessProcess => billOfProcessProcess.Process.ProcessId == billOfProcessProcessWithWorkElementHistoryPriorToUpdate.ProcessId))
                //    billOfProcessWorkElementsToUpdate.Add(billOfProcessProcessWithWorkElementHistoryPriorToUpdate.WorkElementId);

                //  If there is no history, then we can DELETE any work elements associated to the BOP Process ID.  Any existing work elements will be refreshed with the new BOP Process if it remains in the list.
                if (billOfProcessProcessWithWorkElementHistoryPriorToUpdate.WorkElementHistoryCount == 0 && billOfProcessProcessWithWorkElementHistoryPriorToUpdate.WorkElementId > 0)
                    billOfProcessWorkElementsToDelete.Add(billOfProcessProcessWithWorkElementHistoryPriorToUpdate.WorkElementId);

                //  If there is any history, the work elements will be DEACTIVATED.  We can't update these to the new BOP Process ID as they could be different steps.
                if (billOfProcessProcessWithWorkElementHistoryPriorToUpdate.WorkElementHistoryCount > 0 && billOfProcessProcessWithWorkElementHistoryPriorToUpdate.WorkElementId > 0)
                    billOfProcessWorkElementsToDeactivate.Add(billOfProcessProcessWithWorkElementHistoryPriorToUpdate.WorkElementId);
            });
        }

        #endregion
    }
}
