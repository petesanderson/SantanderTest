namespace SantanderTest.Extensions;

public static class LongExtensions
{
    public static DateTime ToDateTime(this long unixTime)
    {
        var datetime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        datetime = datetime.AddSeconds(unixTime);
        return datetime;
    }
}
