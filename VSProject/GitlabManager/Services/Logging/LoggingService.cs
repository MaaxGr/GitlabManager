namespace GitlabManager.Services.Logging
{
    public static class LoggingService
    {

        public static void LogD(string text)
        {
            System.Diagnostics.Debug.WriteLine("DEBUG: " + text);
        }
        
    }
}