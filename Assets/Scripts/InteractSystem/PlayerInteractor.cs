using System.Collections.Generic;
using UnityEngine;


public class PlayerInteractor : MonoBehaviour
{
    //main tasks: sistem buat interactable object terdekat, handle butt E, n munculin/umpetin icon prompt
    [SerializeField] private PlayerInputReader input;

    private readonly List<IInteractable> candidates = new(); //smua zona yg lagi disentuh oleh player
    private IInteractable curr; //obj terdekt yg bisa diinteract yg dipilih/paling deket
    private IInteractionSession activeSession;

    private void OnEnable()
    {
        input.OnInteractPressed += HandlePress;
        input.OnNavigatePressed += HandleNavigate;
    }

    private void OnDisable()
    {
        input.OnInteractPressed -= HandlePress;
        input.OnNavigatePressed -= HandleNavigate;
    }

    private void Update()
    {
        RefreshPrompt(); //bisa ganti zona sambil jalan, nyesuaikn
    }

    public void Register(IInteractable tar) //player get in range
    {
        if (!candidates.Contains(tar)) candidates.Add(tar);
        RefreshPrompt();
    }

    public void UnRegister(IInteractable tar) //player get out range
    {
        candidates.Remove(tar);
        RefreshPrompt();
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
        }

        if (IsAlive(curr) && curr.CanInteract) curr.Interact();
    }

    private void HandleNavigate(int dir)
    {
        activeSession?.OnNavigate(dir);
    }

    private void RefreshPrompt()
    {
        PruneDead();
        IInteractable best = null;
        if (activeSession == null) best = FindNearest();
        if (best == curr) return;

        SetIcon(curr, false);
        curr = best;
        SetIcon(curr, true);
    }

    private IInteractable FindNearest()
    {
        IInteractable best = null;
        float bestDist = float.MaxValue;
        foreach (IInteractable tar in candidates)
        {
            if (!tar.CanInteract) continue;
            float dist = (tar.Position - transform.position).sqrMagnitude; 
            if (dist < bestDist)
            {
                bestDist = dist;
                best = tar;
            }
        }
        return best;
    }

    private void PruneDead() //obj yg mati bareng scene g pernah manggil OnTriggerExit2D so perlu dihapus mnual
    {
        for (int i = candidates.Count - 1; i >= 0; i--)
        {
            if (!IsAlive(candidates[i])) candidates.RemoveAt(i);
        }
        if (!IsAlive(curr)) curr = null;
    }

    //ref bertipe interface g kena operator "==" yg dioverride UnityEngine.Object
    private static bool IsAlive(IInteractable tar)
    {
        return tar is MonoBehaviour mb && mb != null;
    }

    private void SetIcon(IInteractable tar, bool on)
    {
        if (IsAlive(tar) && tar.PromptIcon != null) tar.PromptIcon.SetActive(on);
    }

}
