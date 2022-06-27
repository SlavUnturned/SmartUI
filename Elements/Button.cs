namespace SmartUI.Elements;

public class Button : Element
{
    public TimeSince LastClick { get; internal protected set; }
    public Button(object name, Action onClick = null) : base(name)
    {
        ClickHandler = onClick;
    }
    public Button(object name, Func<Task> onClick = null) : base(name)
    {
        ClickHandlerAsync = onClick;
    }
    public Action ClickHandler;
    public Func<Task> ClickHandlerAsync;
    public virtual TimeSpan ClickDelay { get; set; } = TimeSpan.FromSeconds(0.5);

    public virtual async void Click()
    {
        if (LastClick < ClickDelay)
            return;
        LastClick = TimeSince.Now;
        try
        {
            await ClickHandlerAsync.Invoke();
        } 
        catch 
        { 
            try
            {
                ClickHandler.Invoke();
            }
            catch { }
        }
#if DEBUG
        Console.WriteLine($"Clicked button '{Name}'");
#endif
    }
} 
