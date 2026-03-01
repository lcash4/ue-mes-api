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
    public class BillOfProcessProcessWorkElementAttributeData : DbContextBase, IBillOfProcessProcessWorkElementAttributeData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.BillOfProcessProcessWorkElementAttribute, BillOfProcessProcessWorkElementAttribute>()
            .ReverseMap()
                .ForMember(billOfProcessProcessWorkElementAttributeModel => billOfProcessProcessWorkElementAttributeModel.BillOfProcessProcessWorkElementId, opt => opt.MapFrom(billOfProcessProcessWorkElementAttributeEntity => billOfProcessProcessWorkElementAttributeEntity.BillOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId))
                .ForMember(billOfProcessProcessWorkElementAttributeModel => billOfProcessProcessWorkElementAttributeModel.WorkElementTypeAttributeId, opt => opt.MapFrom(billOfProcessProcessWorkElementAttributeEntity => billOfProcessProcessWorkElementAttributeEntity.WorkElementTypeAttribute.WorkElementTypeAttributeId))
                .ForMember(billOfProcessProcessWorkElementAttributeModel => billOfProcessProcessWorkElementAttributeModel.BillOfProcessProcessWorkElement, opt => opt.Ignore())
                .ForMember(billOfProcessProcessWorkElementAttributeModel => billOfProcessProcessWorkElementAttributeModel.WorkElementTypeAttribute, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.WorkElementTypeAttribute, WorkElementTypeAttribute>();
            mapperConfig.CreateMap<Model.WorkElementTypeAttributeListItem, WorkElementTypeAttributeListItem>()
                .ForPath(workElementTypeAttributeListItemEntity => workElementTypeAttributeListItemEntity.WorkElementTypeAttribute.WorkElementTypeAttributeId, opt => opt.MapFrom(workElementTypeAttributeListItemRecord => workElementTypeAttributeListItemRecord.WorkElementTypeAttributeId));
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public BillOfProcessProcessWorkElementAttributeData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a full <see cref="BillOfProcessProcessWorkElementAttribute"/> entity with child entities included.
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementAttributeId">Unique Bill of Process Process Work Element Attribute ID for the <see cref="BillOfProcessProcessWorkElementAttribute"/></param>
        /// <returns></returns>
        public BillOfProcessProcessWorkElementAttribute GetBillOfProcessProcessWorkElementAttributeById(int billOfProcessProcessWorkElementAttributeId)
        {
            BillOfProcessProcessWorkElementAttribute billOfProcessProcessWorkElementAttribute = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessProcessWorkElementAttributeRecord = mesProductionContext.BillOfProcessProcessWorkElementAttributes
                    .Where(billOfProcessProcessWorkElementAttribute => billOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementAttributeId == billOfProcessProcessWorkElementAttributeId)
                    .Include(billOfProcessProcessWorkElementAttribute => billOfProcessProcessWorkElementAttribute.WorkElementTypeAttribute)
                    .ThenInclude(workElementTypeAttribute => workElementTypeAttribute.WorkElementTypeAttributeListItems)
                    .FirstOrDefault();

                if (billOfProcessProcessWorkElementAttributeRecord != null)
                {
                    billOfProcessProcessWorkElementAttribute = mapper.Map<BillOfProcessProcessWorkElementAttribute>(billOfProcessProcessWorkElementAttributeRecord);
                }
            }

            return billOfProcessProcessWorkElementAttribute;
        }

        /// <summary>
        /// Update an existing bill of process process work element attribute.  
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementAttribute"></param>
        public void UpdateBillOfProcessProcessWorkElementAttribute(BillOfProcessProcessWorkElementAttribute billOfProcessProcessWorkElementAttribute)
        {
            var existingBillOfProcessProcessWorkElementAttribute = GetBillOfProcessProcessWorkElementByBillOfProcessProcessWorkElementAttributeId(billOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementAttributeId);

            if (existingBillOfProcessProcessWorkElementAttribute == null)
                throw new Exception("Object does not exist: There was an attempt to update an object that does not exist in the database.");

            if (existingBillOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementId != billOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId)
                throw new Exception("Invalid object: The object's immutable properties do not match the existing database record.");

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                //1	Pre-scribe clean	1	1
                existingBillOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementId = billOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId;
                existingBillOfProcessProcessWorkElementAttribute.WorkElementTypeAttributeId = billOfProcessProcessWorkElementAttribute.WorkElementTypeAttribute.WorkElementTypeAttributeId;
                existingBillOfProcessProcessWorkElementAttribute.AttributeValue = billOfProcessProcessWorkElementAttribute.AttributeValue;
                existingBillOfProcessProcessWorkElementAttribute.LastModifiedTime = DateTime.Now;
                existingBillOfProcessProcessWorkElementAttribute.LastModifiedTimeUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                existingBillOfProcessProcessWorkElementAttribute.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfProcessProcessWorkElementAttribute.LastModifiedBy) ? billOfProcessProcessWorkElementAttribute.LastModifiedBy : existingBillOfProcessProcessWorkElementAttribute.LastModifiedBy;

                mesProductionContext.Entry(existingBillOfProcessProcessWorkElementAttribute).State = EntityState.Modified;
                mesProductionContext.SaveChanges();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is used to get the existing record for modification or deletion.
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementAttributeId"></param>
        /// <returns></returns>
        private Model.BillOfProcessProcessWorkElementAttribute GetBillOfProcessProcessWorkElementByBillOfProcessProcessWorkElementAttributeId(int billOfProcessProcessWorkElementAttributeId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                return mesProductionContext.BillOfProcessProcessWorkElementAttributes
                    .Where(billOfProcessProcessWorkElementAttribute => billOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementAttributeId == billOfProcessProcessWorkElementAttributeId)
                    .FirstOrDefault();
            }
        }

        #endregion
    }
}
