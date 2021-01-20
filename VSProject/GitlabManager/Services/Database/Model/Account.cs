namespace GitlabManager.Services.Database.Model
{
    public class Account
    {
        public int Id { get; set; }

        public string Identifier { get; set; }
        
        public string Description { get; set;  }
        
        public string HostUrl { get; set; }

        public string AuthenticationToken { get; set; }
    }
}