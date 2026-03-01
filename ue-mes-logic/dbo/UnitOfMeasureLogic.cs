using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_logic.dbo
{
    public class UnitOfMeasureLogic : LogicBase
    {
        IUnitOfMeasureData UnitOfMeasureData { get; }

        #region Constructor

        public UnitOfMeasureLogic(IUnitOfMeasureData unitOfMeasureData)
        {
            UnitOfMeasureData = unitOfMeasureData ?? throw new ArgumentNullException(nameof(unitOfMeasureData));
        }

        #endregion

        /// <summary>
        /// Simply return all units of measure for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<UnitOfMeasure> GetUnitsOfMeasure()
        {
            return UnitOfMeasureData.GetUnitsOfMeasure();
        }
    }
}
