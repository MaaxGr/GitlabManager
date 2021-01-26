using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        [NotifyPropertyChangedInvocator]
        protected void RaisePropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}