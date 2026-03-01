using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo
{
    public class BillOfMaterialPartData : DbContextBase, IBillOfMaterialPartData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.BillOfMaterialPart, BillOfMaterialPart>()
               .ReverseMap()
                   .ForMember(billOfMaterialPartModel => billOfMaterialPartModel.BillOfMaterialId, opt => opt.MapFrom(billOfMaterialPartEntity => billOfMaterialPartEntity.BillOfMaterial.BillOfMaterialId))
                   .ForMember(billOfMaterialPartModel => billOfMaterialPartModel.BillOfMaterial, opt => opt.Ignore())
                   .ForMember(billOfMaterialPartModel => billOfMaterialPartModel.PartId, opt => opt.MapFrom(billOfMaterialPartEntity => billOfMaterialPartEntity.Part.PartId))
                   .ForMember(billOfMaterialPartModel => billOfMaterialPartModel.Part, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.Part, Part>()
                .ForMember(partEntity => partEntity.PartType, opt => opt.MapFrom(partData => (int)partData.PartTypeId));
            mapperConfig.CreateMap<Model.UnitOfMeasure, UnitOfMeasure>();
        });

        private IMapper mapper;

        #endregion

        #region Constructor

        public BillOfMaterialPartData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a range of parts to a bill of material
        /// </summary>
        /// <param name="billOfMaterialParts"></param>
        public void AddBillOfMaterialParts(List<BillOfMaterialPart> billOfMaterialParts)
        {
            List<Model.BillOfMaterialPart> billOfMaterialPartRecords = new List<Model.BillOfMaterialPart>();

            billOfMaterialParts.ForEach(billOfMaterialPart =>
            {
                var billOfMaterialPartRecord = mapper.Map<Model.BillOfMaterialPart>(billOfMaterialPart);
                billOfMaterialPartRecord.BillOfMaterialPartId = 0;

                // Set the Last Modified By if present
                billOfMaterialPartRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfMaterialPart.LastModifiedBy) ? billOfMaterialPart.LastModifiedBy : null;

                billOfMaterialPartRecords.Add(billOfMaterialPartRecord);
            });

            if (billOfMaterialPartRecords != null && billOfMaterialPartRecords.Count > 0)
            {
                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.BillOfMaterialParts.AddRange(billOfMaterialPartRecords);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Will delete all <see cref="BillOfMaterialPart"/> records that match the incoming billOfMaterialId
        /// </summary>
        /// <param name="billOfMaterialId"></param>
        public void DeleteBillOfMaterialParts(int billOfMaterialId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfMaterialPartRecordsToDelete = mesProductionContext.BillOfMaterialParts
                    .Where(billOfMaterialPart => billOfMaterialPart.BillOfMaterialId == billOfMaterialId);
                    
                if (billOfMaterialPartRecordsToDelete != null)
                {
                    mesProductionContext.RemoveRange(billOfMaterialPartRecordsToDelete);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        #endregion
    }
}
