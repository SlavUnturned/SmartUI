namespace SmartUI.Elements;

public class Element
{
    public Element(object name)
    {
        Name = name?.ToString();
    }
    public virtual string Name { get; internal protected set; }
    public virtual bool Visible { get; set; } = true;
    public TimeSince LastVisibilityUpdate { get; internal protected set; }
    
    public Element Alias { get; private set; }

    public Element Set(Element alias) 
    {
        if (alias is null)
            return this;
        if (Alias is not null)
            Alias.Set(alias);
        else Alias = alias;
        return this;
    }

    public virtual void UpdateState(UIController controller) 
    {
        UpdateVisibility(controller);
    }

    public void UpdateVisibility(ITransportConnection connection, short uiKey, bool newVisibility, bool force = false)
    {
        if (Visible == newVisibility)
        {
            if (!force)
                return;
        }
        Visible = newVisibility;
        if (Alias is not null)
            Alias.Visible = Visible;
        UpdateVisibility(connection, uiKey);
    }
    public void UpdateVisibility(ITransportConnection connection, short uiKey)
    {
        EffectManager.sendUIEffectVisibility(uiKey, connection, true, Name, Visible);
        LastVisibilityUpdate = TimeSince.Now;
        if (Alias is not null)
            Alias.LastVisibilityUpdate = LastVisibilityUpdate;
    }
    public void UpdateVisibility(UIController controller, bool newVisibility, bool force = false) => UpdateVisibility(controller.Connection, controller.Key, newVisibility, force);
    public void UpdateVisibility(UIController controller) => UpdateVisibility(controller.Connection, controller.Key);
}