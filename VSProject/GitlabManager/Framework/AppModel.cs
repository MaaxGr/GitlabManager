using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GitlabManager.Annotations;

namespace GitlabManager.Framework
{
    public class AppModel : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void RaisePropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
        
    }
}