namespace SmartUI.Elements;

public class Image : Element
{
    public Image(object name, object url = null) : base(name) 
    {
        Url = url?.ToString();
    }

    public virtual string Url { get; internal protected set; }
    public virtual bool Reliable { get; set; } = true;
    public virtual bool Caching { get; set; } = true;
    public virtual bool Refresh { get; set; }
    public TimeSince LastImageUpdate { get; internal protected set; }

    public override void UpdateState(UIController controller)
    {
        base.UpdateState(controller);
        UpdateImage(controller);
    }

    public void UpdateImage(ITransportConnection connection, short uiKey, string newUrl, bool force = false)
    {
        if (string.IsNullOrWhiteSpace(newUrl) || (Url == newUrl && !force))
            return;
        Url = newUrl;
        UpdateImage(connection, uiKey);
    }
    public async void UpdateImage(ITransportConnection connection, short uiKey)
    {
        EffectManager.sendUIEffectImageURL(uiKey, connection, Reliable, Name, Url, Caching, Refresh);
        LastImageUpdate = TimeSince.Now;
    }
    public void UpdateImage(UIController controller, string newUrl, bool force = false) => UpdateImage(controller.Connection, controller.Key, newUrl, force);
    public void UpdateImage(UIController controller) => UpdateImage(controller.Connection, controller.Key);
}