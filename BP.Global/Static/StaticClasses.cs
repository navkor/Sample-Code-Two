namespace BP
{
    public static class StaticClasses
    {
        public static bool SystemDebug = true;
        public static int MaxFailedLogins = 5;
        public static int MaxLockoutsIn24Hours = 3;
        public static int FirstTimeLockout = 5;
        public static int SecondTimeLockout = 60;
        public static int ThirdTimeLockout = 180;
        public static int TwentyFourHourWait = 1440;
    }
}
