using UnityEngine;

//kontrak objek yg bisa diinteract
public interface IInteractable
{
    //main task: kontrak something that can interact (npc, buku, ksur, dll)
    bool CanInteract { get; }
    void Interact(); //act rinci yg terjadi klo interact
    GameObject PromptIcon { get; } //icon per obj
    Vector3 Position { get; } //ngitung pos obj yg paling deket
}

public interface IInteractionSession
{
    void OnContinue(); //func yg dicall PlayerInteractor slama E dipencet or jalan 
}
