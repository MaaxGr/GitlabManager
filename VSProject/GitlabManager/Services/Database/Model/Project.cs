namespace GitlabManager.Services.Database.Model
{
    public class Project
    {
        public int Id { get; set; }
        
        public int AccountId { get; set; }
        
        public int GitlabProjectId { get; set; }

        public string NameWithNamespace { get; set; }
        
        public string Description { get; set; }
        
        public long LastUpdated { get; set; }
        
        public string[] TagList { get; set; }
        
    }
}