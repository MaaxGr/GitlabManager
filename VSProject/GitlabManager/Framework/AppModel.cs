using System.ComponentModel;
using GitlabManager.Annotations;

namespace GitlabManager.Framework
{
    /// <summary>
    /// Base class of all model classes
    /// </summary>
    /// <see cref="GitlabManager.Model"/>
    public class AppModel : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise a property changed event to listeners
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected void RaisePropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}