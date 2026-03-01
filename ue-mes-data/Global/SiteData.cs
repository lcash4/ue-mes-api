
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Global;

namespace ue_mes_data.Global
{
    public class SiteData : DbContextBase, ISiteData
    {
        #region AutoMapper Config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig => {
            mapperConfig.CreateMap<Model.Site, Site>();
            mapperConfig.CreateMap<Model.Plant, Plant>();
        });

        private IMapper mapper;

        #endregion

        #region Constructor

        public SiteData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return a full site object for the given displayName
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public Site GetSiteByDisplayName(string displayName)
        {
            Site site = null;
            var includeChildren = true;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                //var siteRecord = mesProductionContext.Sites
                //    .Where(site => site.DisplayName == displayName)
                //    .Include(site => site.Plants)
                //    .FirstOrDefault();

                var siteRecord = mesProductionContext.Sites
                    .Where(site => site.DisplayName == displayName);

                // Conditionally add child objects.  Not sure if I'll take this route or not.  Just an example
                if (includeChildren)
                    siteRecord = siteRecord.Include(site => site.Plants);

                if (siteRecord != null)
                {
                    return mapper.Map<Site>(siteRecord.FirstOrDefault());
                }
            }
            return site;
        }

        #endregion
    }
}