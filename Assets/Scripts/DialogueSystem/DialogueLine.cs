public class DialogueLine
{
    //main task: container buat nyimpen line yg udah dipisah ama tagsnya
    public string Text { get; }
    public string Speaker { get; }
    public DialogueMode Mode { get; }
    public int MHDelta { get; }
    public int TensiDelta { get; }
    public int EnergyDelta { get; }
    public bool IsJagatLakon { get; } 

    public DialogueLine (string text, string speaker, DialogueMode mode, int mhDelta, int tensiDelta, int energyDelta, bool isJagatLakon)
    {
        Text = text;
        Speaker = speaker;
        Mode = mode;
        MHDelta = mhDelta;
        TensiDelta = tensiDelta;
        EnergyDelta = energyDelta;
        IsJagatLakon = isJagatLakon;
    }

    //for security/debug purpose
    public bool IsTagOnly => string.IsNullOrEmpty(Text);
    public bool HasResource => MHDelta != 0 || TensiDelta != 0 || EnergyDelta != 0;
}
