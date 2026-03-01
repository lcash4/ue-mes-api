using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Authorization;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo
{
    public class PartItemAttributeTypeData : DbContextBase, IPartItemAttributeTypeData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.PartItemAttributeType, PartItemAttributeType>()
                .ReverseMap()
                    .ForMember(partItemAttributeTypeModel => partItemAttributeTypeModel.PartId, opt => opt.MapFrom(partItemAttributeTypeEntity => partItemAttributeTypeEntity.Part.PartId))
                    .ForMember(partItemAttributeTypeModel => partItemAttributeTypeModel.ItemAttributeTypeId, opt => opt.MapFrom(partItemAttributeTypeEntity => partItemAttributeTypeEntity.ItemAttributeType.ItemAttributeTypeId))
                    .ForMember(partItemAttributeTypeModel => partItemAttributeTypeModel.Part, opt => opt.Ignore())
                    .ForMember(partItemAttributeTypeModel => partItemAttributeTypeModel.ItemAttributeType, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.Part, Part>();
            mapperConfig.CreateMap<Model.UnitOfMeasure, UnitOfMeasure>();
            mapperConfig.CreateMap<Model.ItemAttributeType, ItemAttributeType>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public PartItemAttributeTypeData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a list of <see cref="PartItemAttributeType"/> entities that match the part ID parameter
        /// </summary>
        /// <param name="partId">Unique part id to search</param>
        /// <returns></returns>
        public List<PartItemAttributeType> GetPartItemAttributeTypesByPartId(int partId)
        {
            List<PartItemAttributeType> partItemAttributeTypes = new List<PartItemAttributeType>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var partItemAttributeTypeRecords = mesProductionContext.PartItemAttributeTypes
                    .Where(partItemAttributeType => partItemAttributeType.PartId == partId)
                    .Include(partItemAttributeType => partItemAttributeType.Part)
                    .ThenInclude(part => part.UnitOfMeasure)
                    .Include(partItemAttributeType => partItemAttributeType.ItemAttributeType)
                    .ToList();

                if (partItemAttributeTypeRecords != null)
                {
                    partItemAttributeTypes = mapper.Map<List<Model.PartItemAttributeType>, List<PartItemAttributeType>>(partItemAttributeTypeRecords);
                }
            }

            return partItemAttributeTypes;
        }

        /// <summary>
        /// Add a range of new part order item attribute types
        /// </summary>
        /// <param name="partId">Used to ensure the objects have the correct part ID</param>
        /// <param name="partItemAttributeTypes"></param>
        public void AddPartItemAttributeTypes(int partId, List<PartItemAttributeType> partItemAttributeTypes)
        {
            List<Model.PartItemAttributeType> partItemAttributeTypeRecords = new List<Model.PartItemAttributeType>();

            partItemAttributeTypes.ForEach(partItemAttributeType =>
            {
                var partItemAttributeTypeRecord = mapper.Map<Model.PartItemAttributeType>(partItemAttributeType);

                partItemAttributeTypeRecord.PartItemAttributeTypeId = 0;

                // Assign the part ID because sometimes these will be saved with a new part, meaning the object ID is not yet hydrated.
                partItemAttributeTypeRecord.PartId = partId;
                
                // Set the Last Modified By if present
                partItemAttributeTypeRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(partItemAttributeType.LastModifiedBy) ? partItemAttributeType.LastModifiedBy : null;

                partItemAttributeTypeRecords.Add(partItemAttributeTypeRecord);
            });

            if (partItemAttributeTypeRecords != null && partItemAttributeTypeRecords.Count > 0)
            {
                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.PartItemAttributeTypes.AddRange(partItemAttributeTypeRecords);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Will delete all <see cref="PartItemAttributeType"/> records that match the incoming part ID
        /// </summary>
        /// <param name="partId"></param>
        public void DeletePartItemAttributeTypesByPart(int partId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var partItemAttributeTypeRecordsToDelete = mesProductionContext.PartItemAttributeTypes
                    .Where(partItemAttributeType => partItemAttributeType.PartId == partId);

                if (partItemAttributeTypeRecordsToDelete != null)
                {
                    mesProductionContext.RemoveRange(partItemAttributeTypeRecordsToDelete);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        #endregion
    }
}
