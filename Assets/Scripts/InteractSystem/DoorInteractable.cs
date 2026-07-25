using UnityEngine;

public class DoorInteractable : InteractableBase
{
    [SerializeField] private string doorName = "pintu kamar";
    public override void Interact()
    {
        Debug.Log($"[Interact] {doorName} kebuka!");
    }
}
