
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_data.Global;
using ue_mes_data.Global.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_logic.dbo
{
    public class BillOfProcessLogic : LogicBase
    {
        IBillOfProcessData BillOfProcessData { get; }
        IEquipmentData EquipmentData { get; }

        #region Constructor

        public BillOfProcessLogic(IBillOfProcessData billOfProcessData, IEquipmentData equipmentData)
        {
            BillOfProcessData = billOfProcessData ?? throw new ArgumentNullException(nameof(billOfProcessData));
            EquipmentData = equipmentData ?? throw new ArgumentNullException(nameof(equipmentData));
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Returns a full <see cref="BillOfProcess"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="billOfProcessId">Unique Bill of Process ID for the <see cref="BillOfProcess"/></param>
        /// <returns></returns>
        public BillOfProcess GetBillOfProcessById(int billOfProcessId)
        {
            return BillOfProcessData.GetBillOfProcessById(billOfProcessId);
        }

        /// <summary>
        /// Returns a full <see cref="BillOfProcess"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="partId">Unique Part ID for the <see cref="BillOfProcess"/></param>
        /// <returns></returns>
        public BillOfProcess GetBillOfProcessByPartId(int partId)
        {
            return BillOfProcessData.GetBillOfProcessByPartId(partId);
        }

        /// <summary>
        /// Returns a list of <see cref="BillOfProcess"/> entities with child entities included.
        /// The starting equipment name is used to find BOPs where the process sequence is 1 and matches the equipment process.  This indicates an equipment where an order item unit can be started.
        /// </summary>
        /// <param name="equipmentName">Unique Equipment name to search by its process</param>
        /// <returns></returns>
        public List<BillOfProcess> GetBillOfProcessesByStartingEquipmentName(string equipmentName)
        {
            var equipmentToSearch = EquipmentData.GetEquipmentByName(equipmentName);
            if (equipmentToSearch == null)
                return new List<BillOfProcess>(); // Return an empty list if the equipment is not found

            return BillOfProcessData.GetBillOfProcessesByStartingProcessId(equipmentToSearch.Process.ProcessId);
        }

        /// <summary>
        /// Add a new bill of process
        /// </summary>
        /// <param name="billOfProcess"></param>
        public void AddBillOfProcess(BillOfProcess billOfProcess)
        {
            // Need to check if bill of process already exists.  If so, return "Bill Of Process already exists" exception
            var existingBillOfProcess = BillOfProcessData.GetBillOfProcessByPartId(billOfProcess.Part.PartId);
            if (existingBillOfProcess != null)
                throw new InvalidDataException(string.Format("Bill Of Process {0} already exists", billOfProcess.Name));

            // The BOP name is an auto-generated string with the format "BOP<Part.PartNumber.Replace("PT,"")><Part.Revision>"
            // This ensures the BOP names are unique and not require the caller to know the format.
            billOfProcess.Name = string.Format("BOP{0}{1}", billOfProcess.Part.PartNumber.Replace("PT", string.Empty), billOfProcess.Part.PartRevision);

            BillOfProcessData.AddBillOfProcess(billOfProcess);
        }

        /// <summary>
        /// Update an existing bill of process.  
        /// Note, the bill of process name column is NOT mutable per our own logic.  It is an auto-generated string.
        /// If the caller somehow changes it before calling this method, an exception will be thrown.
        /// </summary>
        /// <param name="billOfProcess"></param>
        public void UpdateBillOfProcess(BillOfProcess billOfProcess)
        {
            BillOfProcessData.UpdateBillOfProcess(billOfProcess);
        }

        #endregion
    }
}
