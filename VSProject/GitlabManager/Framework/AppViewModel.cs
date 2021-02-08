using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MVVMLight.Messaging;

namespace GitlabManager.Framework
{
    /// <summary>
    /// Base class of all View-Models
    ///
    /// Inspiration: Adonis UI Demo Template
    /// https://github.com/benruehl/adonis-ui/blob/master/src/AdonisUI.Demo/Framework/ViewModel.cs (#LOC)
    /// => Modified. e.g. added MVVM-Light-Messenger 
    /// </summary>
    /// <see cref="GitlabManager.ViewModels"/>
    public class AppViewModel
        : INotifyPropertyChanged
            , INotifyDataErrorInfo
            , IDataErrorInfo
    {
        private readonly Dictionary<string, IList<string>> _validationErrors = new Dictionary<string, IList<string>>();

        //http://dotnetpattern.com/mvvm-light-messenger
        protected readonly IMessenger MessengerInstance = Messenger.Default;
        
        public string this[string propertyName]
        {
            get
            {
                if (string.IsNullOrEmpty(propertyName))
                    return Error;

                return _validationErrors.ContainsKey(propertyName) ? string.Join(Environment.NewLine, _validationErrors[propertyName]) : string.Empty;
            }
        }

        public string Error => string.Join(Environment.NewLine, GetAllErrors());

        public bool HasErrors => _validationErrors.Any();

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return _validationErrors.SelectMany(kvp => kvp.Value);

            return _validationErrors.TryGetValue(propertyName, out var errors) ? errors : Enumerable.Empty<object>();
        }

        private IEnumerable<string> GetAllErrors()
        {
            return _validationErrors.SelectMany(kvp => kvp.Value).Where(e => !string.IsNullOrEmpty(e));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Raise a change of a property to the view
        /// </summary>
        /// <param name="propertyName">Name of property that was updated</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Update a property and raise change to the view
        /// </summary>
        /// <param name="storage">Reference to variable that should be updated</param>
        /// <param name="value">Value that should be set</param>
        /// <param name="propertyName">Name of property that should be updated</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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