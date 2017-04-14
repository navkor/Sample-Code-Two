using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Auth
{
    public static class TimeExtensions
    {
        public static bool IsWithin60Minutes(this DateTimeOffset dateTime)
        {
            var now = DateTimeOffset.Now;
            var returnValue = false;
            TimeSpan difference = now - dateTime;
            returnValue = difference.Hours < 1 && difference.Minutes < 60;
            return returnValue;
        }

        public static bool IsWithin60Minutes(this DateTimeOffset dateTime, DateTimeOffset now)
        {
            var returnValue = false;
            TimeSpan difference = now - dateTime;
            returnValue = difference.Hours < 1 && difference.Minutes < 60;
            return returnValue;
        }

        public static int HowManyBadLoginsWithin60Minutes(this IList<DateTable> loginDates)
        {
            var returnValue = 0;

            // let's see how many logins we've had
            var loginList = loginDates.Where(x =>
                                x.DateType.Index == 801
                                && x.DateLine.IsWithin60Minutes());
            returnValue = loginList.Count();

            return returnValue;
        }

        public static int HowManyBadLoginsWithin60Minutes(this IList<DateTable> loginDates, DateTimeOffset now)
        {
            var returnValue = 0;

            // let's see how many logins we've had
            var loginList = loginDates.Where(x =>
                                x.DateType.Index == 801
                                && x.DateLine.IsWithin60Minutes(now));
            returnValue = loginList.Count();

            return returnValue;
        }

        public static int HowManyLockOutsWithinTime(this IList<DateTable> loginDates, int hours)
        {
            var returnValue = 0;
            var loginList = loginDates.Where(x => x.DateType.Index == 802).OrderBy(y => y.DateLine);
            TimeSpan difference = new TimeSpan();
            var now = DateTimeOffset.Now;
            foreach (var time in loginList)
            {
                difference = now - time.DateLine;
                if (difference.Hours < hours) returnValue++;
            }
            return returnValue;
        }

        public static bool IsUnlocked(this IList<DateTable> loginDates)
        {
            var returnValue = true;
            var lastLockedLogin = loginDates.Where(x => x.DateType.Index == 802).OrderBy(y => y.DateLine).FirstOrDefault();
            TimeSpan difference = new TimeSpan();
            var now = DateTimeOffset.Now;
            if (lastLockedLogin != null)
            {
                // they've been locked before
                var dateLine = lastLockedLogin.DateLine;
                difference = now - dateLine;
                if (difference.Hours < 24) returnValue = false;
            }
            return returnValue;
        }
    }
}
