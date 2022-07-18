namespace SmartUI.Controllers;

public abstract class UIController : UnturnedPlayerComponent
{
    public UIController() 
    {
        ElementsList = 
            DefaultElements
            .Prepend(MainElement = new(ID.ToString()))
            .ToList();
    }
    public UP UPlayer => base.Player;
    public new Player Player => UPlayer.Player;
    public ITransportConnection Connection => Player.channel.owner.transportConnection;

    public virtual bool IsActive { get; private set; }
    public abstract ushort ID { get; }
    public abstract short Key { get; }
    public virtual bool Reliable { get; } = true;
    public bool Visible { get; private set; } = true;
    public virtual EPluginWidgetFlags Widgets { get; set; } = EPluginWidgetFlags.None;

    public Element this[object name] => ElementsList.FirstOrDefault(x => x.Name.SameAs(name?.ToString()));
    public T Get<T>(object name)
        where T : Element => this[name]?.Get<T>();

    public IEnumerable<Element> Elements => ElementsList;
    public Element MainElement { get; }
    internal protected List<Element> ElementsList { get; }
    internal protected virtual List<Element> DefaultElements { get; } = new();

    protected virtual void SendEffect() => EffectManager.sendUIEffect(ID, Key, Connection, Reliable);
    protected virtual void ClearEffect() => EffectManager.askEffectClearByID(ID, Connection);

    public void SetVisibility(bool visibility)
    {
        MainElement.UpdateVisibility(this, Visible = visibility);
    }

    public async void SetActive(bool state)
    {
        if (IsActive == state)
            return;
        await OnStateUpdating();
        if (state)
            SendEffect();
        else ClearEffect();
        ApplyWidgets(state);
        IsActive = state;
        await OnStateUpdated();
    }
    public void ForceUpdateElements()
    {
        foreach (var element in ElementsList)
            element.UpdateState(this);
    }
    public virtual void ApplyWidgets(bool state) => ApplyWidgets(Widgets, state);
    public void ToggleLifeMeters(bool state) => ApplyWidgets(EPluginWidgetFlags.ShowLifeMeters, state);
    public void ApplyWidgets(EPluginWidgetFlags flags, bool state)
    {
        foreach (var flag in flags.GetFlags())
            Player.setPluginWidgetFlag(flag, state);
    }
    public virtual Task OnStateUpdating() => Task.CompletedTask;
    public virtual Task OnStateUpdated() => Task.CompletedTask;
    public virtual Task OnTextFieldCommited(string name, string text) => Task.CompletedTask;
    public virtual Task OnButtonClicked(string name) => Task.CompletedTask;
}
