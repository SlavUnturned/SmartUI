namespace SmartUI.Elements;

public class Progress : Element
{
    public Progress(object name, uint value = 100, uint minValue = 0, uint maxValue = 100) : base(name)
    {
        Value = value;
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public virtual uint Value { get; protected set; }
    public virtual uint? PreviousValue { get; protected set; }
    public virtual uint MinValue { get; set; }
    public virtual uint MaxValue { get; set; }
    public virtual bool Reliable { get; set; } = true;
    public TimeSince LastProgressUpdate { get; internal set; }
    
    public virtual string FormatName(uint value) => $"{Name}_{value}";

    public override void UpdateState(UIController controller)
    {
        base.UpdateState(controller);
        UpdateCount(controller);
    }

    public void UpdateCount(int value, ITransportConnection connection, short uiKey)
    {
        if (Value == value || value < 0) return;
        PreviousValue = Value;
        Value = (uint)Mathf.Clamp(value, MinValue, MaxValue);
        UpdateCount(connection, uiKey);
    }
    
    public void UpdateCount(ITransportConnection connection, short uiKey)
    {
        UpdateElementVisibility(connection, uiKey, PreviousValue, false);
        UpdateElementVisibility(connection, uiKey, Value, true);
        LastProgressUpdate = TimeSince.Now;
    }

    private void UpdateElementVisibility(ITransportConnection connection, short uiKey, uint? value, bool visible)
    {
        if (value is <= 0) return;
        EffectManager.sendUIEffectVisibility(uiKey, connection, Reliable, FormatName(value.Value), visible);
    }
    
    public void UpdateCount(UIController controller, int value) => UpdateCount(value, controller.Connection, controller.Key);
    public void UpdateCount(UIController controller) => UpdateCount(controller.Connection, controller.Key);
}