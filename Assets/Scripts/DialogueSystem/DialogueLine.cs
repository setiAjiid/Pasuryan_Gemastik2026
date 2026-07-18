public class DialogueLine
{
    //main task: container buat nyimpen line yg udah dipisah ama tagsnya
    public string Text { get; }
    public string Speaker { get; }
    public int MHDelta { get; }
    public int TensiDelta { get; }
    public int EnergyDelta { get; }

    public DialogueLine (string text, string speaker, int mhDelta, int tensiDelta, int energyDelta)
    {
        Text = text;
        Speaker = speaker;
        MHDelta = mhDelta;
        TensiDelta = tensiDelta;
        EnergyDelta = energyDelta;
    }

    //for security/debug purpose
    public bool IsTagOnly => string.IsNullOrEmpty(Text);
    public bool HasResource => MHDelta != 0 || TensiDelta != 0 || EnergyDelta != 0;
}
