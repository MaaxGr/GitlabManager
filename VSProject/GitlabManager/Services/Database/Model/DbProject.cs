using System.ComponentModel.DataAnnotations.Schema;

namespace GitlabManager.Services.Database.Model
{
    [Table("Projects")]
    public class DbProject
    {
        public int Id { get; set; }
        
        public int AccountId { get; set; }
        
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