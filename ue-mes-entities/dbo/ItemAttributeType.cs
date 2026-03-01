namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Item Attribute Types are key attributes that are assigned to a part, which then carry into Order Items and Inventory
    /// Typically, these are coupled with the front end and adding new attributes may result in additional front end development.
    /// An example would be the length, width and thickness of glass.  Each would have their own type that could be applied to many Order Items or Inventory Items
    /// </summary>
    public class ItemAttributeType
    {
        /// <summary>
        /// Unique ID for the attribute type
        /// </summary>
        public int ItemAttributeTypeId { get; set; }

        /// <summary>
        ///  attribute type name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  attribute type description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// item attribute type record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the attribute type record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
