using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    //main tasks: sistem buat interactable object terdekat, handle butt E, n munculin/umpetin icon prompt
    [SerializeField] private PlayerInputReader input;

    private IInteractable curr; //obj terdekt yg bisa diinteract
    private IInteractionSession activeSession;

    private void OnEnable()
    {
        input.OnInteractPressed += HandlePress;
    }

    private void OnDisable()
    {
        input.OnInteractPressed -= HandlePress;
    }

    public void Register(IInteractable tar) //player get in range
    {
        SetIcon(curr, false); //matiin icon lama klo ada yg ketimpa
        curr = tar;
        RefreshPrompt();
    }

    public void UnRegister(IInteractable tar) //player get out range
    {
        if (curr == tar)
        {
            SetIcon(curr, false);
            curr = null;
        }
    }

    public void BeginSession(IInteractionSession session)
    {
        activeSession = session;
        RefreshPrompt();
    }

    public void EndSession(IInteractionSession session)
    {
        if (activeSession == session) activeSession = null;
        RefreshPrompt();
    }

    private void HandlePress()
    {
        if (activeSession != null)
        {
            activeSession.OnContinue(); //klo E = dialog lanjut bris, dsb
            return;
        } else { }

        if (curr != null && curr.CanInteract) curr.Interact();
    }

    public void RefreshPrompt()
    {
        SetIcon(curr, activeSession == null && curr != null && curr.CanInteract); //which is icon bkl di showin klo no active session, in range zona, ama bisa diinteract ga 
    }

    private void SetIcon(IInteractable tar, bool on)
    {
        if (tar != null && tar.PromptIcon != null) tar.PromptIcon.SetActive(on);
    }

}
