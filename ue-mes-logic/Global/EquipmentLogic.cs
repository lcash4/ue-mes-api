using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Global;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Global;

namespace ue_mes_logic.Global
{
    public class EquipmentLogic : LogicBase
    {
        IEquipmentData EquipmentData { get; }

        #region Constructor

        public EquipmentLogic(IEquipmentData equipmentData)
        {
            EquipmentData = equipmentData ?? throw new ArgumentNullException(nameof(equipmentData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns an equipment for a given name.
        /// </summary>
        /// <param name="equipmentName">Unique equipment name</param>
        /// <returns></returns>
        public Equipment GetEquipmentByName(string equipmentName)
        {
            return EquipmentData.GetEquipmentByName(equipmentName);
        }

        /// <summary>
        /// Returns a list of equipment for a given site.
        /// </summary>
        /// <param name="siteDisplayName">Unique site display name</param>
        /// <returns></returns>
        public List<Equipment> GetEquipmentBySiteDisplayName(string siteDisplayName)
        {
            return EquipmentData.GetEquipmentBySiteDisplayName(siteDisplayName);
        }

        /// <summary>
        /// Returns a list of equipment for a given site and process
        /// </summary>
        /// <param name="siteDisplayName">Unique site display name</param>
        /// <param name="processId">Unique process id</param>
        /// <returns></returns>
        public List<Equipment> GetEquipmentBySiteDisplayNameAndProcessId(string siteDisplayName, int processId)
        {
            return EquipmentData.GetEquipmentBySiteDisplayNameAndProcessId(siteDisplayName, processId);
        }

        #endregion
    }
}
