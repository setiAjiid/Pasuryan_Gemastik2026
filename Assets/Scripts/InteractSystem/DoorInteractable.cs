using UnityEngine;

public class DoorInteractable : InteractableBase
{
    //main task: pintu kemana saja, which is pindahin player ke scene lain dari titik spawn yg ditentukan
    [SerializeField] private string doorName = "pintu kamar";
    [SerializeField] private string targetScene;
    [SerializeField] private string targetSpawnId;

    private SceneTransitionManager transition;
    public override bool CanInteract => transition != null && !transition.IsBusy;

    public void BindTransition(SceneTransitionManager tar)
    {
        transition = tar;
    }
    public override void Interact()
    {
        Debug.Log($"[Interact] {doorName} kebuka!");
        transition.GoTo(targetScene, targetSpawnId);
    }
}
