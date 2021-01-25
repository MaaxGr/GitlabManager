using System.ComponentModel.DataAnnotations.Schema;

namespace GitlabManager.Services.Database.Model
{
    [Table("Accounts")]
    public class DbAccount
    {
        public int Id { get; set; }

        public string Identifier { get; set; }
        
        public string Description { get; set;  }
        
        public string HostUrl { get; set; }

        public string AuthenticationToken { get; set; }
        
        public long LastProjectUpdateAt { get; set; }
        
    }
}