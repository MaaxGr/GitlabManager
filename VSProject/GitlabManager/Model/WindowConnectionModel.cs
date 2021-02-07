using System;
using System.Threading;
using System.Threading.Tasks;
using GitlabManager.Framework;
using GitlabManager.Services.Gitlab;

namespace GitlabManager.Model
{
    /// <summary>
    /// Window that displays, whether user has entered the correct host and auth token for a saved account
    /// </summary>
    public class WindowConnectionModel : AppModel
    {
        #region Dependencies

        private readonly IGitlabService _gitlabService;

        #endregion

        #region Private Properties

        private string _hostUrl;
        private string _authenticationToken;

        #endregion

        #region Public Properties for ViewModel
        
        /// <summary>
        /// State of the established connection (Loading/Success/Error)
        /// </summary>
        public ConnectionState ConnectionState { get; private set; }

        #endregion

        
        public WindowConnectionModel(IGitlabService gitlabService)
        {
            _gitlabService = gitlabService;
        }

        /// <summary>
        /// Init the view model with connection credentials
        /// </summary>
        /// <param name="hostUrl">Host-URL of the Gitlab Instance (e.g. https://gitlab.com)</param>
        /// <param name="authenticationToken">Personal Access Token of the User</param>
        public void Init(string hostUrl, string authenticationToken)
        {
            // init variables
            _hostUrl = hostUrl;
            _authenticationToken = authenticationToken;

            // test connection
            TryToConnect();
        }

        private void TryToConnect()
        {
            // reset progress
            SetState(ConnectionState.Loading(0));

            // prepare cancellation tokens
            var connectionCheckCancellationTokenSource = new CancellationTokenSource();
            var progressReportCancellationTokenSource = new CancellationTokenSource();
            var connectionCheckCancellationToken = connectionCheckCancellationTokenSource.Token;
            var progressReportCancellationToken = progressReportCancellationTokenSource.Token;

            // run task to check if connection can be established
            var connectionCheckTask = Task.Run(async () =>
            {
                var gitlabClient = _gitlabService.GetGitlabClient(_hostUrl, _authenticationToken);
                var (success, message) = await gitlabClient.IsConnectionEstablished();

                SetState(success ? ConnectionState.Success() : ConnectionState.Error(message));
                progressReportCancellationTokenSource.Cancel();
            }, connectionCheckCancellationToken);

            // run tasks to update progress
            UpdateProgress(20, 1, progressReportCancellationToken);
            UpdateProgress(40, 2, progressReportCancellationToken);
            UpdateProgress(60, 3, progressReportCancellationToken);
            UpdateProgress(80, 4, progressReportCancellationToken);
            UpdateProgress(100, 5, progressReportCancellationToken);

            // cancel established task
            Task.Delay(6 * 1000, progressReportCancellationToken).ContinueWith(_ =>
            {
                if (connectionCheckTask.IsCompletedSuccessfully) return;
                connectionCheckCancellationTokenSource.Cancel();
                SetState(ConnectionState.Error("Connection timeout. Valid internet connection?"));
            }, progressReportCancellationToken);
        }

        private void UpdateProgress(int progressValue, int delaySeconds, CancellationToken cancellationToken)
        {
            Task.Delay(delaySeconds * 1000, cancellationToken)
                .ContinueWith(task => { SetState(ConnectionState.Loading(progressValue)); }, cancellationToken);
        }

        private void SetState(ConnectionState connectionState)
        {
            ConnectionState = connectionState;
            RaisePropertyChanged(nameof(ConnectionState));
        }
    }

    public class ConnectionState
    {
        /// <summary>
        /// Connection State Type (Loading, Success, Error)
        /// </summary>
        public StateType Type { get; private set; }
        
        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMessage { get; private set; }
        
        /// <summary>
        /// Bar Progress in % (0-100)
        /// </summary>
        public int BarProgress { get; private set; }

        /// <summary>
        /// Connection State Type (Loading, Success, Error)
        /// </summary>
        public enum StateType
        {
            Loading,
            Success,
            Error
        }

        /// <summary>
        /// Static utility method to create a Success-State
        /// </summary>
        /// <returns>Success connection state</returns>
        public static ConnectionState Success()
        {
            return new ConnectionState
            {
                Type = StateType.Success,
                ErrorMessage = "",
                BarProgress = 100
            };
        }

        /// <summary>
        /// Static utility method to create a Loading-State
        /// </summary>
        /// <param name="progress">Current progress in %</param>
        /// <returns>Loading connection state</returns>
        public static ConnectionState Loading(int progress)
        {
            return new ConnectionState
            {
                Type = StateType.Loading,
                ErrorMessage = "",
                BarProgress = progress
            };
        }

        /// <summary>
        /// Static utility method to create a Error-State
        /// </summary>
        /// <param name="message">Error Message with description of error</param>
        /// <returns>Error connection state</returns>
        public static ConnectionState Error(string message)
        {
            return new ConnectionState
            {
                Type = StateType.Error,
                ErrorMessage = message,
                BarProgress = 100
            };
        }
    }
}