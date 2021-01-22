using System;

namespace GitlabManager.Utils
{
    public static class CoreExtensions
    {

        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            var unixFirstDate = new DateTime(1970, 1, 1);
            return (long) (dateTime.Subtract(unixFirstDate).TotalSeconds * 1000L);
        }


    }
}