using System;

namespace GitlabManager.Utils
{
    public static class DateTimeUtils
    {
        
        public static DateTime CreateDateTimeFromIso8691(string iso8601String)
        {
            return DateTime.Parse(
                iso8601String,
                null,
                System.Globalization.DateTimeStyles.RoundtripKind
            );
        }
        
        public static DateTime UnixMillisToDateTime( double millis )
        {
            return new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc)
                .AddMilliseconds(millis).ToLocalTime();
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
            
            var differenceDays = differenceMinutes / 24;

            if (differenceDays <= 60)
            {
                return $"{differenceDays} days ago";
            }
            
            var differenceMonths = differenceDays / 30;

            if (differenceMonths <= 24)
            {
                return $"about {differenceDays} months ago";
            }

            var differenceYears = differenceMonths / 12;

            return $"about {differenceYears} years ago";
        }
        
    }
}