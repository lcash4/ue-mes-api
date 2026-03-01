using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;
using ue_mes_entities.Enums;

namespace ue_mes_data.dbo.Interface
{
    public interface IPartData
    {
        /// <summary>
        /// Return all parts and include inactive parts if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public List<Part> GetAllParts(bool includeInactive = false);

        /// <summary>
        /// Returns a list of <see cref="Part"/> entities with part child entities included.
        /// The starting process ID is used to find BOPs where the process sequence is 1 and matches the process.  This indicates an process where an order item unit can be started.
        /// </summary>
        /// <param name="processId">Unique Process ID for the <see cref="BillOfProcessProcess"/> to search</param>
        /// <returns></returns>
        public List<Part> GetPartsForBillOfProcessAtStartingEquipmentProcess(int processId);

        /// <summary>
        /// Return all parts where the part type matches a type in the partTypes list.
        /// Also, include inactive parts if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="partTypes">List of <see cref="PartTypeEnum"/> to return</param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public List<Part> GetPartsByPartTypes(List<PartTypeEnum> partTypes, bool includeInactive = false);

        /// <summary>
        /// Return a part by part number and revision
        /// </summary>
        /// <param name="partNumber"></param>
        /// <param name="partRevision"></param>
        /// <returns></returns>
        public Part GetPartByPartNumberAndRevision(string partNumber, string partRevision);

        /// <summary>
        /// Return a part entity by unique part ID
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        public Part GetPartEntityByPartId(int partId);

        /// <summary>
        /// Add a new part
        /// </summary>
        /// <param name="part"></param>
        public void AddPart(Part part);

        /// <summary>
        /// Update an existing part.  
        /// </summary>
        /// <param name="part"></param>
        public void UpdatePart(Part part);
    }
}
