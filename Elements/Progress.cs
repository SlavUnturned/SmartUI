namespace SmartUI.Elements;

public class Progress : Element
{
    public Progress(object name, int minValue = 0, int maxValue = 100) : base(name)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public virtual int Value { get; set; }
    public virtual int MinValue { get; set; }
    public virtual int MaxValue { get; set; }
    public virtual bool Reliable { get; set; } = true;
    public TimeSince LastProgressUpdate { get; internal set; }
    
    private int previous;

    public sealed override string ToString() => $"{Name}_{Value}";
    private string PreviousToString() => $"{Name}_{previous}";
    
    public override void UpdateState(UIController controller)
    {
        base.UpdateState(controller);
        UpdateCount(controller);
    }

    public void UpdateCount(int value, ITransportConnection connection, short uiKey)
    {
        if (Value == value) return;
        previous = Value;
        Value = Mathf.Clamp(Value, MinValue, MaxValue);
        UpdateCount(connection, uiKey);
    }
    
    public void UpdateCount(ITransportConnection connection, short uiKey)
    {
        if (previous != MinValue)
            EffectManager.sendUIEffectVisibility(uiKey, connection, Reliable, PreviousToString(), false);
        EffectManager.sendUIEffectVisibility(uiKey, connection, Reliable, ToString(), true);
        LastProgressUpdate = TimeSince.Now;
    }
    
    public void UpdateCount(UIController controller, int value) => UpdateCount(value, controller.Connection, controller.Key);
    public void UpdateCount(UIController controller) => UpdateCount(controller.Connection, controller.Key);
}