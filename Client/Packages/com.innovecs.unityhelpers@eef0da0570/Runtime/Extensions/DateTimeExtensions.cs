using System;

namespace Innovecs.UnityHelpers
{
    public static class DateTimeExtensions
    {
        public static DateTime Epoch => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static TimeSpan UnixUtcNow => DateTime.UtcNow.Subtract(Epoch);

        public static DateTime UnixTimeStampToDateTime(TimeSpan unixTimeStamp) =>
            UnixTimeStampToDateTime(unixTimeStamp.TotalSeconds);

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) =>
            Epoch.AddSeconds(unixTimeStamp).ToLocalTime();

        public static DateTime UnixTimeStampToUtc(double unixTimeStamp) =>
            Epoch.AddSeconds(unixTimeStamp).ToUniversalTime();
    }
}