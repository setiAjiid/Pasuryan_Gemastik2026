using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    //main task: nampung logika daftar/lepas diri ke interactor, biar tiap objek g nulis ulang
    [SerializeField] private GameObject promptIcon;
    
    private PlayerInteractor interactor; 

    public virtual bool CanInteract => true; //override klo objek punya syarat sendiri
    public GameObject PromptIcon => promptIcon;

    public abstract void Interact();

    public void Bind(PlayerInteractor tar)
    {
        interactor = tar;
    }

    protected virtual void Awake()
    {
        if (promptIcon != null) promptIcon.SetActive(false);
    }

    protected virtual void Start()
    {
        if (interactor == null) Debug.LogError($"[{name}] blum ke Bind, apakaha ada SceneContext di scene ini?", this);
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
