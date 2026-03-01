using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// An item in inventory, which is used to keep track of internal assemblies and supplied parts.  Each item will be assigned a location and will move locations when a corresponding action takes place.
    /// For example, a lot of glass can be moved from warehouse to production when it has been allocated to an order.
    /// </summary>
    public class InventoryItem
    {
        #region Private Variables

        private string _iventoryItemAttributesConcatenated;

        #endregion

        /// <summary>
        /// Unique ID for this inventory item
        /// </summary>
        public long InventoryItemId { get; set; }

        /// <summary>
        /// Inventory item type
        /// </summary>
        public InventoryItemType InventoryItemType { get; set; }

        /// <summary>
        /// Current location for this inventory item
        /// </summary>
        public InventoryLocation InventoryLocation { get; set; }

        /// <summary>
        /// Part assigned to this inventory item
        /// </summary>
        public Part Part { get; set; }

        /// <summary>
        /// The supplier of this inventory item
        /// </summary>
        public Supplier Supplier { get; set; }

        /// <summary>
        /// The serial or lot number that is either generated in the factory, or provided by the supplier
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// The quantity is per the inventory item, which includes its location.  
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Inventory item record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the inventory item record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }

        /// <summary>
        /// List of inventory item attributes
        /// </summary>
        public List<InventoryItemAttribute> InventoryItemAttributes { get; set; }

        /// <summary>
        /// This will be a concatenated string of all attribute names and their values, uesd primarily for display purposes.
        /// </summary>
        public string InventoryItemAttributesConcatenated
        {
            get
            {
                if(this.InventoryItemAttributes != null && this.InventoryItemAttributes.Count > 0)
                    this._iventoryItemAttributesConcatenated = string.Join(", ", this.InventoryItemAttributes.Select(attribute => attribute.ItemAttributeType.Name + ": " + attribute.AttributeValue));

                return this._iventoryItemAttributesConcatenated;
            }
            set
            {
                this._iventoryItemAttributesConcatenated = value;
            }
        }
    }
}
