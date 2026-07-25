using UnityEngine;

public class DialogueTrigger : InteractableBase
{
    //main task: cek syarat pra-dialog sebelum ke dialogue manager
    //trigger saat apa? collision terjadi
    //npc pasif cuman ngasih data dan laporin jarak ke PlayerInteractor, sediain data dialog

    [SerializeField] private SpeechBubbleUI bubble;
    [SerializeField] private TextAsset text;
    [SerializeField] private DialogueManager dialogueManager;

    public SpeechBubbleUI Bubble => bubble;
    public TextAsset Text => text;
    public override bool CanInteract => !dialogueManager.IsDialogueActive; //slama dialog g aktif bisa diinteract

    public override void Interact()
    {
        dialogueManager.StartDialogue(bubble, text);
    }
}
