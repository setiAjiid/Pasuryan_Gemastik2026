using UnityEngine;

public class JournalInteractable : InteractableBase
{
    private JournalManager journal; //diisi scenecontext

    public override bool CanInteract => journal != null && journal.CanOpen;


    public void BindJournal(JournalManager tar)
    {
        journal = tar;
    }

    public override void Interact()
    {
        Debug.Log($"[Interact] Jurnal kebuka!");
        journal.OpenJournal();
    }
}
