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
    public class ConnectionWindowModel : AppModel
    {
        /*
         * Dependencies
         */
        private readonly IGitlabService _gitlabService;

        /*
         * Input information
         */
        private string _hostUrl;
        private string _authenticationToken;

        /*
         * Properties for ViewModel
         */
        public ConnectionState ConnectionState { get; private set; }

        public ConnectionWindowModel(IGitlabService gitlabService)
        {
            _gitlabService = gitlabService;
        }

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
            Task.Delay(5 * 1000, progressReportCancellationToken).ContinueWith(_ =>
            {
                if (connectionCheckTask.IsCompleted) return;
                connectionCheckCancellationTokenSource.Cancel();
                SetState(ConnectionState.Error("Connection timeout. Valid internet connection?"));
            }, progressReportCancellationToken);
        }

        private void UpdateProgress(int progressValue, int delaySeconds, CancellationToken cancellationToken)
        {
            Task.Delay(delaySeconds * 1000, cancellationToken).ContinueWith(task =>
            {
                SetState(ConnectionState.Loading(progressValue));
            }, cancellationToken);
        }

        private void SetState(ConnectionState connectionState)
        {
            ConnectionState = connectionState;
            RaisePropertyChanged(nameof(ConnectionState));
        }
    }

    public class ConnectionState
    {
        public StateType Type { get; private set;  }
        public string ErrorMessage { get; private set;  }
        public int BarProgress { get; private set;  }

        public enum StateType
        {
            Loading, Success, Error
        }

        public static ConnectionState Success()
        {
            return new ConnectionState
            {
                Type = StateType.Success,
                ErrorMessage = "",
                BarProgress = 100
            };
        }
        
        public static ConnectionState Loading(int progress)
        {
            return new ConnectionState
            {
                Type = StateType.Loading,
                ErrorMessage = "",
                BarProgress = progress
            };
        }

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