namespace ue_mes_entities.dbo
{
    /// <summary>
    /// A work element status is a fixed set of statuses that apply to Bill of Process Work Elements.  
    /// As a unit is built and traversing through the work elements, they will receive status updates along the way.  This allows the MES to reflect the current status of a unit at any time.
    /// </summary>
    public class WorkElementStatus
    {
        /// <summary>
        /// Unique ID of the work element status
        /// </summary>
        public int WorkElementStatusId { get; set; }

        /// <summary>
        /// Name of the work element status
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the work elemenet status
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Work element status record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time in UTC format
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}