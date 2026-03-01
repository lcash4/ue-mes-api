namespace ue_mes_entities.Global
{
    public class Site
    {
        /// <summary>
        /// Unique ID for the site
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Full name of the site
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Short display name for the site
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Site record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the site record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }

        /// <summary>
        /// List of plants that are part of a <see cref="Site"/>
        /// </summary>
        public List<Plant> Plants { get; set; }
    }
}