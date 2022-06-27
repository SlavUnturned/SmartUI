namespace SmartUI.Elements;

public class Text : Element
{
    public Text(object name, object text = null) : base(name) 
    {
        Value = text?.ToString();
    }

    public virtual string Value { get; internal set; }
    public virtual bool Reliable { get; set; } = true;
    public TimeSince LastTextUpdate { get; internal set; }

    public override void UpdateState(UIController controller)
    {
        base.UpdateState(controller);
        UpdateText(controller);
    }

    public void UpdateText(ITransportConnection steamId, short uiKey, object newText, bool force = false)
    {
        var newValue = newText.ToString();
        if (Value == newValue)
        {
            if (!force)
                return;
        }
        Value = newValue;
        UpdateText(steamId, uiKey);
    }
    public void UpdateText(ITransportConnection connection, short uiKey)
    {
        EffectManager.sendUIEffectText(uiKey, connection, Reliable, Name, Value);
        LastTextUpdate = TimeSince.Now;
    }
    public void UpdateText(UIController controller, object newText, bool force = false) => UpdateText(controller.Connection, controller.Key, newText, force);
    public void UpdateText(UIController controller) => UpdateText(controller.Connection, controller.Key);
}