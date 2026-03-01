using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Containers are used for storing and shipping assemblies built in MES only.  They will not be used to manage incoming inventory from suppliers.
    /// </summary>
    public class Container
    {
        /// <summary>
        /// Unique ID of the container record
        /// </summary>
        public long ContainerId { get; set; }

        /// <summary>
        /// The type of container
        /// </summary>
        public ContainerType ContainerType { get; set; }

        /// <summary>
        /// Current inventory location of the container
        /// </summary>
        public InventoryLocation InventoryLocation { get; set; }

        /// <summary>
        /// Unique serial number for the container
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Container record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the container record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }

        /// <summary>
        /// Order Item Unit contents of a container
        /// </summary>
        public List<ContainerOrderItemUnit> ContainerOrderItemUnits { get; set; }
    }
}
