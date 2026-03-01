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
    public interface IPartItemAttributeTypeData
    {
        /// <summary>
        /// Returns a list of <see cref="PartItemAttributeType"/> entities that match the part ID parameter
        /// </summary>
        /// <param name="partId">Unique part id to search</param>
        /// <returns></returns>
        public List<PartItemAttributeType> GetPartItemAttributeTypesByPartId(int partId);

        /// <summary>
        /// Add a range of new part order item attribute types
        /// </summary>
        /// <param name="partId">Used to ensure the objects have the correct part ID</param>
        /// <param name="partItemAttributeTypes"></param>
        public void AddPartItemAttributeTypes(int partId, List<PartItemAttributeType> partItemAttributeTypes);

        /// <summary>
        /// Will delete all <see cref="PartItemAttributeType"/> records that match the incoming part ID
        /// </summary>
        /// <param name="partId"></param>
        public void DeletePartItemAttributeTypesByPart(int partId);
    }
}
