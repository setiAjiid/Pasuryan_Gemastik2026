using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    //main task: nampung logika daftar/lepas diri ke interactor, biar tiap objek g nulis ulang
    [SerializeField] private PlayerInteractor interactor;
    [SerializeField] private GameObject promptIcon;

    public virtual bool CanInteract => true; //override klo objek punya syarat sendiri
    public GameObject PromptIcon => promptIcon;

    public abstract void Interact();

    protected virtual void Awake()
    {
        if (promptIcon != null) promptIcon.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactor.Register(this);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactor.UnRegister(this);
        }
    }
}
