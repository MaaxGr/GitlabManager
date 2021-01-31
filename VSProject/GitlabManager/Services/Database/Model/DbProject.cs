using System.ComponentModel.DataAnnotations.Schema;

namespace GitlabManager.Services.Database.Model
{
    
    /// <summary>
    /// Database model for ef-core for the Projects-Table
    /// </summary>
    [Table("Projects")]
    public class DbProject
    {
        /// <summary>
        /// Internal project id
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// reference to the corresponding account
        /// </summary>
        public DbAccount Account { get; set; }
        
        /// <summary>
        /// External Gitlab-Project (may be the same for different gitlab accounts)
        /// </summary>
        public int GitlabProjectId { get; set; }

        /// <summary>
        /// Human readable name with namespace
        /// </summary>
        public string NameWithNamespace { get; set; }
        
        /// <summary>
        /// Project Description 
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Timestamp when project was updated the last time
        /// </summary>
        public long LastUpdated { get; set; }
        
        /// <summary>
        /// List of tags
        /// </summary>
        public string[] TagList { get; set; }

        /// <summary>
        /// Boolean, whether project is starred
        /// </summary>
        public bool Stared { get; set; }
        
        /// <summary>
        /// Timestamp when user changed star (feature comming in later version)
        /// </summary>
        public long StaredChangeSaved { get; set; }
        
        /// <summary>
        /// Path to local folder
        /// </summary>
        public string LocalFolder { get; set; }
        
    }
}