using UnityEngine;

public class JournalManager : MonoBehaviour, IInteractionSession
{
    public void OpenJournal()
    {
        Debug.Log("Jurnal dah kebuka cuy");
    }
    public void OnContinue()
    {
        throw new System.NotImplementedException();
    }

    public void OnNavigate(int dir)
    {
        throw new System.NotImplementedException();
    }
}
