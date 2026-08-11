using System;
using Unity.Cinemachine;
using UnityEngine;

public class SceneContext : MonoBehaviour
{
    //main task: nyamabungin isi scene ke subsitem abadi
    [SerializeField] private Collider2D cameraBounds;
    [SerializeField] private float floorY; //garis jalan ruangan ini, semua yg mendarat di sini pake Y ini
    [SerializeField] private SpawnPoint defaultSpawn; //dipk klo scene ini diplay langsung dari editor, bukn lewat pintu

    public float FloorY => floorY;

    private void Awake()
    {
        GameRoot root = GameRoot.Instance;
        if (root == null)
        {
            Debug.LogError("[SceneContext] Gameroot blum ada, cek Assets/Resources/GameRoot.preafab", this);
            return;
        }

        BindAll<InteractableBase>(tar => tar.Bind(root.Interactor));
        BindAll<DialogueTrigger>(tar => tar.BindDialogue(root.Dialogue));
        BindAll<DialogueSpeaker>(tar => tar.BindDialogue(root.Dialogue));
        BindAll<DoorInteractable>(tar => tar.BindTransition(root.Transition));
        BindAll<JournalInteractable>(tar => tar.BindJournal(root.Journal));

        ApplyCameraBounds(root);
        ApplyDefaultSpawn(root);
    }

    public SpawnPoint FindSpawn(string spawnId)
    {
        foreach (SpawnPoint point in FindObjectsByType<SpawnPoint>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (point.gameObject.scene != gameObject.scene) continue;
            if (point.SpawnId != spawnId) continue;
            return point;
        }
        return null;
    }

    //kek template di c++, nyapu satu tipe obj di suatu scene ini doang, then jalanin aksi bindnya
    private void BindAll<T>(Action<T> bind) where T: MonoBehaviour
    {
        foreach (T target in FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (target.gameObject.scene != gameObject.scene) continue;
            bind(target);
        }
    }

    private void ApplyCameraBounds(GameRoot root)
    {
        if (cameraBounds == null) return;
        CinemachineConfiner2D confiner = root.Confiner;
        confiner.BoundingShape2D = cameraBounds;
        confiner.InvalidateBoundingShapeCache(); //confiner nyimpen cache lama, so perlu dibuang
    }

    private void ApplyDefaultSpawn(GameRoot root)
    {
        if (defaultSpawn == null) return;
        if (root.Transition.IsBusy) return; //lagi transisi lewat pintu
        root.Transition.PlaceAt(defaultSpawn, floorY);
    }

    //garis kuning selebar batas cam, biar garis jalannya kelihatan di scene view
    private void OnDrawGizmos()
    {
        if (cameraBounds == null) return;
        Gizmos.color = Color.yellow;
        Bounds area = cameraBounds.bounds;
        Gizmos.DrawLine(new Vector3(area.min.x, floorY, 0f), new Vector3(area.max.x, floorY, 0f));
    }
}
