using System;

namespace GitlabManager.Utils
{
    /// <summary>
    /// Extensions and Utility Methods for a better DateLibrary experience.
    /// </summary>
    public static class DateTimeUtils
    {

        public static string ToIso8691(this DateTime dateTime)
        {
            return dateTime.ToString("O");
        }
        
        public static DateTime UnixMillisToDateTime( double millis )
        {
            return new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc)
                .AddMilliseconds(millis).ToLocalTime();
        }
        
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            var unixFirstDate = new DateTime(1970, 1, 1);
            return (long) (dateTime.Subtract(unixFirstDate).TotalSeconds * 1000L);
        }
        
        public static string UnixTimestampAgoHumanReadable(long lastActivityUnixStamp)
        {
            var currentUnixStamp = DateTime.Now.ToUnixTimestamp();
            var differenceSeconds = (currentUnixStamp - lastActivityUnixStamp) / 1000;

            if (differenceSeconds <= 120)
            {
                return $"{differenceSeconds} seconds ago";
            }

            var differenceMinutes = differenceSeconds / 60;

            if (differenceMinutes <= 120)
            {
                return $"{differenceMinutes} minutes ago";
            }

            
            var differenceHours = differenceMinutes / 60;

            if (differenceHours <= 48)
            {
                return $"{differenceHours} hours ago";
            }
            
            var differenceDays = differenceHours / 24;

            if (differenceDays <= 60)
            {
                return $"{differenceDays} days ago";
            }
            
            var differenceMonths = differenceDays / 30;

            if (differenceMonths <= 24)
            {
                return $"about {differenceMonths} months ago";
            }

            var differenceYears = differenceMonths / 12;

            return $"about {differenceYears} years ago";
        }
        
    }
}