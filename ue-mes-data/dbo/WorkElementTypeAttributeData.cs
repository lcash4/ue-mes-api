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
    public class WorkElementTypeAttributeData : DbContextBase, IWorkElementTypeAttributeData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig => {
            mapperConfig.CreateMap<Model.WorkElementTypeAttribute, WorkElementTypeAttribute>();
            mapperConfig.CreateMap<Model.WorkElementType, WorkElementType>();
            mapperConfig.CreateMap<Model.WorkElementTypeAttributeListItem, WorkElementTypeAttributeListItem>()
                .ForPath(workElementTypeAttributeListItemEntity => workElementTypeAttributeListItemEntity.WorkElementTypeAttribute.WorkElementTypeAttributeId, opt => opt.MapFrom(workElementTypeAttributeListItemRecord => workElementTypeAttributeListItemRecord.WorkElementTypeAttributeId));
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public WorkElementTypeAttributeData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Simply return all work element type attributes to be used in a client list or dropdown.
        /// </summary>
        /// <returns></returns>
        public List<WorkElementTypeAttribute> GetWorkElementTypeAttributes()
        {
            List<WorkElementTypeAttribute> workElementTypeAttributes = new List<WorkElementTypeAttribute>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var workElementTypeAttributeRecords = mesProductionContext.WorkElementTypeAttributes
                    .Include(workElementTypeAttribute => workElementTypeAttribute.WorkElementType)
                    .Include(workElementTypeAttribute => workElementTypeAttribute.WorkElementTypeAttributeListItems)
                    .OrderByDescending(workElementTypeAttribute => workElementTypeAttribute.IsRequiredAtSetup)
                        .ThenBy(workElementTypeAttribute => workElementTypeAttribute.Name).ToList();

                if (workElementTypeAttributeRecords != null)
                {
                    workElementTypeAttributes = mapper.Map<List<Model.WorkElementTypeAttribute>, List<WorkElementTypeAttribute>>(workElementTypeAttributeRecords);
                }
            }
            return workElementTypeAttributes;
        }

        #endregion
    }
}
