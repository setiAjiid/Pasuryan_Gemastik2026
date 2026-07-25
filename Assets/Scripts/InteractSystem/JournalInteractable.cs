using UnityEngine;

public class JournalInteractable : InteractableBase
{
    [SerializeField] private JournalManager journal;

    //public override bool CanInteract => journal.HasFragments; //syarat pra-jurnal

    public override void Interact()
    {
        Debug.Log($"[Interact] Jurnal kebuka!");
        journal.OpenJournal();
    }
}
