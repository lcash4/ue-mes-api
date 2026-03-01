using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.Enums
{
    public enum OrderStateEnum
    {
        /// <summary>
        /// Default if unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Order has been created in the system
        /// </summary>
        Created = 1,

        /// <summary>
        /// Order is put on hold due to some constraint
        /// </summary>
        Hold = 2,

        /// <summary>
        /// Order has been verified for build and released to production
        /// </summary>
        Released = 3,

        /// <summary>
        /// Order has been started and actively building
        /// </summary>
        Started = 4,

        /// <summary>
        /// Order has completed on the factory floor
        /// </summary>
        Complete = 5,

        /// <summary>
        /// Completed order has been verified that all product has shipped and can be closed in the system
        /// </summary>
        Closed = 6
    }
}
