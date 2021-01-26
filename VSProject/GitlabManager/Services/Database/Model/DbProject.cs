using System.ComponentModel.DataAnnotations.Schema;

namespace GitlabManager.Services.Database.Model
{
    
    /// <summary>
    /// Database model for ef-core for the Projects-Table
    /// </summary>
    [Table("Projects")]
    public class DbProject
    {
        public int Id { get; set; }
        
        public DbAccount Account { get; set; }
        
        public int GitlabProjectId { get; set; }

        public string NameWithNamespace { get; set; }
        
        public string Description { get; set; }
        
        public long LastUpdated { get; set; }
        
        public string[] TagList { get; set; }

        public bool Stared { get; set; }

        public long StaredChangeSaved { get; set; }
        
        public string LocalFolder { get; set; }
        
    }
}