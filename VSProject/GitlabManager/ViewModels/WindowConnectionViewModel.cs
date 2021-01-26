using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using GitlabManager.Framework;
using GitlabManager.Model;
using GitlabManager.Services.Resources;
using GitlabManager.Theme;
using static GitlabManager.Model.ConnectionState;
using Brushes = AdonisUI.Brushes;

namespace GitlabManager.ViewModels
{
    /// <summary>
    /// ViewModel for the entire ConnectionWindow that is opened by the "Test"-Button
    /// in the Accounts Page of the Main window
    /// </summary>
    public class WindowConnectionViewModel : AppViewModel
    {
        /*
         * Dependencies
         */
        private readonly ConnectionWindowModel _windowModel;
        private readonly IResources _resources;
        
        /*
         * Helper Properties
         */
        private ConnectionState ConnectionState => _windowModel.ConnectionState;
        
        /*
         * Properties
         */
        public int CurrentProgressBarValue => ConnectionState.BarProgress;
        public string StateText => GenerateFancyStateText(ConnectionState);
        public string ErrorText => ConnectionState.ErrorMessage;
        public SolidColorBrush StateTextColor => GenerateFancyStateTextColor(ConnectionState);
        public Visibility ProgressbarVisibility => GenerateProgressVisibility(ConnectionState);

        public WindowConnectionViewModel(ConnectionWindowModel windowModel, IResources resources)
        {
            _windowModel = windowModel;
            _windowModel.PropertyChanged += ConnectionWindowModelPropertyChangedHandler;
            _resources = resources;
        }

        /*
         * Init before window opens 
         */
        public void Init(string hostUrl, string authenticationToken)
        {
            _windowModel.Init(hostUrl, authenticationToken);
        }
        
        /*
         * Handle model property changes
         */
        private void ConnectionWindowModelPropertyChangedHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(ConnectionWindowModel.ConnectionState):
                    RaisePropertyChanged(nameof(CurrentProgressBarValue));
                    RaisePropertyChanged(nameof(StateText));
                    RaisePropertyChanged(nameof(ErrorText));
                    RaisePropertyChanged(nameof(StateTextColor));
                    RaisePropertyChanged(nameof(ProgressbarVisibility));
                    break;
            }
        }
        
        /*
         * Generate the text that is shown for a specific connection state
         */
        private static string GenerateFancyStateText(ConnectionState state)
        {
            return state.Type switch {
                StateType.Success => "Connection established successfully!",
                StateType.Loading => "Trying to connect...",
                StateType.Error => "Error while testing connection!",
                _ => ""
            };
        }
        
        /*
         * Generate the text color for a specific connection state
         */
        private SolidColorBrush GenerateFancyStateTextColor(ConnectionState state)
        {
            return state.Type switch
            {
                StateType.Success => _resources.GetBrush(ThemeConstants.SuccessBrush),
                StateType.Error => _resources.GetBrush(ThemeConstants.ErrorLightBrush),
                _ => _resources.GetBrush(Brushes.ForegroundBrush)
            };
        }

        private Visibility GenerateProgressVisibility(ConnectionState state)
        {
            return state.Type == StateType.Loading
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}