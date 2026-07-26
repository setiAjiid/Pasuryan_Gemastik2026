using Unity.Cinemachine;
using UnityEngine;

public class SceneContext : MonoBehaviour
{
    //main task: nyamabungin isi scene ke subsitem abadi
    [SerializeField] private Collider2D cameraBounds;

    private void Awake()
    {
        GameRoot root = GameRoot.Instance;
        if (root == null)
        {
            Debug.LogError("[SceneContext] Gameroot blum ada, cek Assets/Resources/GameRoot.preafab", this);
            return;
        }

        BindInteractables(root);
        ApplyCameraBounds(root);
    }

    private void BindInteractables(GameRoot root)
    {
        foreach (InteractableBase target in FindObjectsByType<InteractableBase>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (target.gameObject.scene != gameObject.scene) continue;
            target.Bind(root.Interactor);
        }

        foreach (DialogueTrigger trigger in FindObjectsByType<DialogueTrigger>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (trigger.gameObject.scene != gameObject.scene) continue;
            trigger.BindDialogue(root.Dialogue);
        }
    }

    private void ApplyCameraBounds(GameRoot root)
    {
        if (cameraBounds == null) return;
        CinemachineConfiner2D confiner = root.Confiner;
        confiner.BoundingShape2D = cameraBounds;
        confiner.InvalidateBoundingShapeCache(); //confiner nyimpen cache lama, so perlu dibuang
    }
}
