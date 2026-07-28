using UnityEngine;

public class DialogueTrigger : InteractableBase
{
    //main task: cek syarat pra-dialog sebelum ke dialogue manager
    //trigger saat apa? collision terjadi
    //npc pasif cuman ngasih data dan laporin jarak ke PlayerInteractor, sediain data dialog

    [SerializeField] private TextAsset text;
    
    private DialogueManager dialogueManager;

    public TextAsset Text => text;
    public override bool CanInteract => dialogueManager != null && !dialogueManager.IsDialogueActive; //slama dialog g aktif bisa diinteract

    public void BindDialogue(DialogueManager tar)
    {
        dialogueManager = tar;
    }
    public override void Interact()
    {
        dialogueManager.StartDialogue(text);
    }
}
