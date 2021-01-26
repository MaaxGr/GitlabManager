using System;

namespace GitlabManager.Services.Logging
{
    public static class LoggingService
    {

        public static void LogD(string text)
        {
            Console.WriteLine($"DEBUG: {text}");
        }
        
    }
}