using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IUnitOfMeasureData
    {
        /// <summary>
        /// Simply return all units of measure for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<UnitOfMeasure> GetUnitsOfMeasure();
    }
}
