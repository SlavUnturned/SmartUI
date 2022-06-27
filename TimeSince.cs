namespace SmartUI;

public struct TimeSince
{
    public TimeSince(DateTime? from = null)
    {
        From = from.HasValue ? from.Value : DateTime.Now;
    }

    public DateTime From { get; }
    public static TimeSince Now => new();
    public static implicit operator double(TimeSince @this) => ((TimeSpan)@this).TotalSeconds;
    public static implicit operator TimeSince(double @this) => new (DateTime.Now.AddSeconds(@this));
    public static implicit operator TimeSpan(TimeSince @this) => (DateTime.Now - @this.From);
    public static implicit operator DateTime(TimeSince @this) => @this.From;
}