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
    public interface ICoaterData
    {
        /// <summary>
        /// Returns the first active layer coater for the site name
        /// </summary>
        /// <param name="siteName">Unique site name to search</param>
        /// <returns></returns>
        public Coater GetActiveLayerCoaterBySiteName(string siteName);
    }
}
