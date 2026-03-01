using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_entities.Global;

namespace ue_mes_data.dbo
{
    public class DefectData : DbContextBase, IDefectData
    {
        #region AutoMapper Config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.Defect, Defect>()
                .ReverseMap()
                    .ForMember(defectModel => defectModel.DefectCategoryId, opt => opt.MapFrom(defectEntity => defectEntity.DefectCategory.DefectCategoryId))
                    .ForMember(defectModel => defectModel.DefectCategory, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.DefectCategory, DefectCategory>()
                .ForPath(defectCategoryEntity => defectCategoryEntity.Defects, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.DefectProcess, DefectProcess>();
            mapperConfig.CreateMap<Model.Process, Process>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor
        public DefectData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return all defects for a given category.  Only return inactive defects if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="defectCategoryId"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public List<Defect> GetDefectsByCategoryId(int defectCategoryId, bool includeInactive = false)
        {
            List<Defect> defects = new List<Defect>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var defectRecords = mesProductionContext.Defects
                    .Where(defect => defect.DefectCategoryId == defectCategoryId
                        && (defect.IsActive || (!defect.IsActive && includeInactive)))
                    .Include(defect => defect.DefectCategory)
                    .Include(defect => defect.DefectProcesses)
                        .ThenInclude(defectProcess => defectProcess.Process)
                    .ToList();

                if (defectRecords != null)
                {
                    defects = mapper.Map<List<Model.Defect>, List<Defect>>(defectRecords);
                }
            }

            return defects;
        }

        #endregion
    }
}
