using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Global;

namespace ue_mes_data.Global.Interface
{
    public interface IEquipmentData
    {
        /// <summary>
        /// Returns an equipment for a given name.
        /// </summary>
        /// <param name="equipmentName">Unique equipment name</param>
        /// <returns></returns>
        public Equipment GetEquipmentByName(string equipmentName);

        /// <summary>
        /// Returns a list of equipment for a given site.
        /// </summary>
        /// <param name="siteDisplayName">Unique site display name</param>
        /// <returns></returns>
        public List<Equipment> GetEquipmentBySiteDisplayName(string siteDisplayName);

        /// <summary>
        /// Returns a list of equipment for a given site and process
        /// </summary>
        /// <param name="siteDisplayName">Unique site display name</param>
        /// <param name="processId">Unique process id</param>
        /// <returns></returns>
        public List<Equipment> GetEquipmentBySiteDisplayNameAndProcessId(string siteDisplayName, int processId);
    }
}
