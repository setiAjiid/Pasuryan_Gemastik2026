public interface IDialogueView 
{
    //main taks: kontrak buat semua kotak dialog, which is DialogueManager g perlu peduli bentuknya kek apa
    void Show(string speaker, string text);
    void Hide();

}
