using Unity.Cinemachine;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    //main task; nammpung subsistem abadi :v n mastiin dirinya ada di scene manapun
    [Header("Subsistem Abadi")]
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private PlayerInteractor interactor;
    [SerializeField] private DialogueManager dialogue;
    [SerializeField] private Transform player;
    [SerializeField] private CinemachineConfiner2D confiner;
    [SerializeField] private SceneTransitionManager transition;

    [Header("Speaker Abadi")]
    [SerializeField] private DialogueSpeaker[] persistentSpeaker; //surya, dll

    private static GameRoot instance;
    public static GameRoot Instance => instance;
    public PlayerInputReader InputReader => inputReader;
    public PlayerInteractor Interactor => interactor;
    public DialogueManager Dialogue => dialogue;
    public Transform Player => player;
    public CinemachineConfiner2D Confiner => confiner;
    public SceneTransitionManager Transition => transition;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (instance != null) return;
        GameObject prefab = Resources.Load<GameObject>("GameRoot");
        if (prefab == null) {
            Debug.LogError("[Prefab] prefab gameroot ga ketemu, harus ada di Assets/Resources/GameRoot.prefab");
            return;
        }

        GameObject root = Instantiate(prefab);
        root.name = prefab.name; //buang embel2 "clone" just for make it my workspace clean
        DontDestroyOnLoad(root);

    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        foreach(DialogueSpeaker speaker in persistentSpeaker)
        {
            dialogue.RegSpeaker(speaker);
        }
    }


}
