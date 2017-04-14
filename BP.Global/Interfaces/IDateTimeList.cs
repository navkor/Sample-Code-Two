using System;

namespace BP
{
    public interface IDateTimeList
    {
        int ID { get; }
        DateTimeOffset DateLine { get; set; }
    }
}
