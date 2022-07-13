namespace SmartUI.Elements;

public class Progress : Element
{
    public Progress(object name, uint minValue = 0, uint maxValue = 100) : base(name)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public virtual int Value { get; set; }
    public virtual int PreviousValue { get; protected set; } = -1;
    public virtual uint MinValue { get; set; }
    public virtual uint MaxValue { get; set; }
    public virtual bool Reliable { get; set; } = true;
    public TimeSince LastProgressUpdate { get; internal set; }
    
    public virtual string FormatName(int value) => $"{Name}_{value}";

    public override void UpdateState(UIController controller)
    {
        base.UpdateState(controller);
        UpdateCount(controller);
    }

    public void UpdateCount(int value, ITransportConnection connection, short uiKey)
    {
        if (Value == value || value < 0) return;
        PreviousValue = Value;
        Value = Mathf.Clamp(Value, (int)MinValue, (int)MaxValue);
        UpdateCount(connection, uiKey);
    }
    
    public void UpdateCount(ITransportConnection connection, short uiKey)
    {
        UpdateElementVisibility(connection, uiKey, PreviousValue, false);
        UpdateElementVisibility(connection, uiKey, Value, true);
        LastProgressUpdate = TimeSince.Now;
    }

    private void UpdateElementVisibility(ITransportConnection connection, short uiKey, int value, bool visible)
    {
        if (value < 0) return;
        EffectManager.sendUIEffectVisibility(uiKey, connection, Reliable, FormatName(value), visible);
    }
    
    public void UpdateCount(UIController controller, int value) => UpdateCount(value, controller.Connection, controller.Key);
    public void UpdateCount(UIController controller) => UpdateCount(controller.Connection, controller.Key);
}