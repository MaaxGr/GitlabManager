using System;
using System.Windows.Input;

namespace GitlabManager.Framework
{
    
    /// <summary>
    /// Generic Delegate implementation of the ICommand interface.
    /// 
    /// Inspiration: Adonis UI Demo Template
    /// https://github.com/benruehl/adonis-ui/blob/master/src/AdonisUI.Demo/Framework/Command.cs
    /// => Modified, so that it supports generics
    /// #LOC
    /// </summary>
    /// <typeparam name="T">CommandParameter Type</typeparam>
    public class AppDelegateCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public AppDelegateCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            if (parameter is T castedParameter)
            {
                return _canExecute(castedParameter);
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            _execute((T) parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;
    }
}