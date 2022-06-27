namespace SmartUI.Elements;

public class Field : Text
{
    public Field(object name, object text = null, Action<string> commitHandler = null) : base(name, text)
    {
        if (text is not null)
            Commit(text?.ToString());
        CommitHandler = commitHandler;
    }

    public Action<string> CommitHandler;
    public string Text { get; internal protected set; }
    public TimeSince LastTextCommit { get; internal protected set; }
    internal protected virtual void Commit(string text)
    {
        Text = text;
        CommitHandler?.Invoke(text);
        LastTextCommit = TimeSince.Now;
        #if DEBUG
        Console.WriteLine($"Commited field '{Name}' with text: {Text}");
        #endif
    }
}