using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using GitlabManager.Services.Logging;
using MVVMLight.Messaging;

namespace GitlabManager.Framework
{
    public class AppViewModel
        : INotifyPropertyChanged
            , INotifyDataErrorInfo
            , IDataErrorInfo
    {
        private readonly Dictionary<string, IList<string>> _validationErrors = new Dictionary<string, IList<string>>();

        //http://dotnetpattern.com/mvvm-light-messenger
        protected IMessenger MessengerInstance = Messenger.Default;
        
        public string this[string propertyName]
        {
            get
            {
                if (string.IsNullOrEmpty(propertyName))
                    return Error;

                if (_validationErrors.ContainsKey(propertyName))
                    return string.Join(Environment.NewLine, _validationErrors[propertyName]);

                return string.Empty;
            }
        }

        public string Error => string.Join(Environment.NewLine, GetAllErrors());

        public bool HasErrors => _validationErrors.Any();

        public IEnumerable GetErrors(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName))
                return _validationErrors.SelectMany(kvp => kvp.Value);

            return _validationErrors.TryGetValue(propertyName, out var errors) ? errors : Enumerable.Empty<object>();
        }

        private IEnumerable<string> GetAllErrors()
        {
            return _validationErrors.SelectMany(kvp => kvp.Value).Where(e => !String.IsNullOrEmpty(e));
        }

        public void AddValidationError(string propertyName, string errorMessage)
        {
            if (!_validationErrors.ContainsKey(propertyName))
                _validationErrors.Add(propertyName, new List<string>());

            _validationErrors[propertyName].Add(errorMessage);
        }

        public void ClearValidationErrors(string propertyName)
        {
            if (_validationErrors.ContainsKey(propertyName))
                _validationErrors.Remove(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            LoggingService.LogD($"Handler null? {PropertyChanged == null}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Debug.WriteLine("Property Change: " + propertyName);
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