namespace ue_mes_entities.dbo
{
    /// <summary>
    /// The PartItemAttributeType class is a 1 <see cref="Part"/> to many <see cref="ItemAttributeType"/> mapping object.  
    /// Each attribute assigned to a part will be entered through the UI or received in an API call.  If the attribute is required, the user will be forced to enter a value.  Otherwise, it can be skipped
    /// </summary>
    public class PartItemAttributeType
    {
        /// <summary>
        /// Unique ID for the part item attribute
        /// </summary>
        public int PartItemAttributeTypeId { get; set; }

        /// <summary>
        /// Part for which the attributes are applied
        /// </summary>
        public Part Part{ get; set; }

        /// <summary>
        /// Item attribute type
        /// </summary>
        public ItemAttributeType ItemAttributeType { get; set; }

        /// <summary>
        /// Whether the attribute is required for this part of not
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Part order item attribute record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the order item attribute record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
