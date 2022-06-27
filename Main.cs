namespace SmartUI;

public sealed class UIManager : RocketPlugin
{
    protected override void Unload()
    {
        EffectManager.onEffectTextCommitted -= EffectTextCommittedHandler;
        EffectManager.onEffectButtonClicked -= EffectButtonClickedHandler;
    }
    protected override void Load()
    {
        EffectManager.onEffectTextCommitted += EffectTextCommittedHandler;
        EffectManager.onEffectButtonClicked += EffectButtonClickedHandler;
    }

    public static void EffectButtonClickedHandler(Player player, string buttonName)
    {
        player.ControllersAction<UIController>(x =>
        {
            x.ElementsList.ElementComponentAction<Button>(buttonName, btn => btn.Click());
            return x.OnButtonClicked(buttonName);
        });
    }

    public static void EffectTextCommittedHandler(Player player, string fieldName, string text)
    {
        player.ControllersAction<UIController>(x =>
        {
            x.ElementsList.ElementComponentAction<Field>(fieldName, field => field.Commit(text));
            return x.OnTextFieldCommited(fieldName, text);
        });
    }
}
