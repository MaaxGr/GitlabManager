using System;

namespace GitlabManager.Services.Logging
{
    public static class LoggingService
    {

        /// <summary>
        /// Log debug text to console
        /// </summary>
        /// <param name="text">Text that should be logged</param>
        public static void LogD(string text)
        {
            Console.WriteLine($"DEBUG: {text}");
        }
        
    }
}