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
    public class DefectCategoryData : DbContextBase, IDefectCategoryData
    {
        #region AutoMapper Config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.DefectCategory, DefectCategory>()
                .ReverseMap();
            mapperConfig.CreateMap<Model.Defect, Defect>();
            mapperConfig.CreateMap<Model.DefectProcess, DefectProcess>();
            mapperConfig.CreateMap<Model.Process, Process>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor
        public DefectCategoryData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return all defect categories, as well as their child defects and defect processes.  
        /// The amount of unique defects should not exceed a large amount, so returning the fully hydrated list will allow the caller to have full flexibility.
        /// Only include inactive defects when <paramref name="includeInactive"/> is true.
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public List<DefectCategory> GetDefectCategoriesWithDefects(bool includeInactive = false)
        {
            List<DefectCategory> defectCategories = new List<DefectCategory>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var defectCategoryRecords = mesProductionContext.DefectCategories
                    .Include(defectCategory => defectCategory.Defects.Where(defect => defect.IsActive || (!defect.IsActive && includeInactive)))
                        .ThenInclude(defect => defect.DefectProcesses)
                        .ThenInclude(defectProcess => defectProcess.Process)
                    .ToList();

                if (defectCategoryRecords != null)
                {
                    defectCategories = mapper.Map<List<Model.DefectCategory>, List<DefectCategory>>(defectCategoryRecords);
                }
            }

            return defectCategories;
        }

        #endregion
    }
}
