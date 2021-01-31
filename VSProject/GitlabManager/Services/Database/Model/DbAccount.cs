using System.ComponentModel.DataAnnotations.Schema;

namespace GitlabManager.Services.Database.Model
{
    /// <summary>
    /// Database model for ef-core for the Accounts-Table
    /// </summary>
    [Table("Accounts")]
    public class DbAccount
    {
        /// <summary>
        /// Internal account id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of connection
        /// </summary>
        public string Identifier { get; set; }
        
        /// <summary>
        /// Description of the account
        /// </summary>
        public string Description { get; set;  }
        
        /// <summary>
        /// Gitlab URL 
        /// </summary>
        public string HostUrl { get; set; }

        /// <summary>
        /// Private token for user
        /// </summary>
        public string AuthenticationToken { get; set; }
        
        /// <summary>
        /// UNIX Timestamp when account was updated the last time
        /// </summary>
        public long LastProjectUpdateAt { get; set; }
        
    }
}