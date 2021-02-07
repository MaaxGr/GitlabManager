using System.ComponentModel.DataAnnotations.Schema;

namespace GitlabManager.Services.Database.Model
{
    /// <summary>
    /// Database model for ef-core for the Settings-Table
    /// </summary>
    [Table("Settings")]
    public class DbSetting
    {
        /// <summary>
        /// Internal Id of setting
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Key of setting
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Current value of setting
        /// </summary>
        public string Value { get; set; }
        
    }
}