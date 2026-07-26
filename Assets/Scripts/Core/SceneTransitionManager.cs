using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    //main task: pindahin player antarscene, which is include redupin layar, muat scene baru, put player in spawn point, ama buang scene lama
    [Header("Refs")]
    [SerializeField] private PlayerInputReader input;
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private CanvasGroup fade;
    [SerializeField] private PlayerMovement playerMove;

    [Header("Tuning")]
    [SerializeField] private float fadeDuration = 0.35f;

    public bool IsBusy { get; private set; }
    public void GoTo(string sceneName, string spawnId)
    {
        if (IsBusy) return; //ga numpuk transisi
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"[Transition] scene '{sceneName}' ga ada di Build Profiles");
            return;
        }

        StartCoroutine(Move(sceneName, spawnId));
    }

    public void PlaceAt(SpawnPoint point, float floorY)
    {
        Vector3 now = playerBody.transform.position;
        Vector3 dest = new(point.transform.position.x, floorY, now.z); //x dr spawn, y dr ruangan, z biarin
        Vector3 delta = dest - now; //hitung jarak perpindahan

        playerBody.position = dest;
        playerBody.linearVelocity = Vector2.zero; //nolin sisa momentum
        playerMove.Face(point.Facing); //nongolin mc sesuai keluarnya dia 
        cam.OnTargetObjectWarped(playerBody.transform, delta); //kamera loncat
    }

    private IEnumerator Move(string sceneName, string spawnId)
    {
        IsBusy = true;
        input.DisableMove();
        yield return Fade(1f);

        Scene old = SceneManager.GetActiveScene();
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene loaded = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(loaded);

        PlacePlayer(loaded, spawnId);
        yield return SceneManager.UnloadSceneAsync(old);
        yield return Fade(0f);

        input.EnableMove();
        IsBusy = false;
    }

    private void PlacePlayer(Scene target, string spawnId)
    {
        SceneContext ctx = FindContext(target);
        if (ctx == null)
        {
            Debug.LogError($"[Trnasition] scene {target.name} ga punya SceneContext");
            return;
        }

        SpawnPoint point = ctx.FindSpawn(spawnId);
        if (point == null)
        {
            Debug.LogError($"[Transition] spawn '{spawnId}' ga ketemeu di scene {target.name}");
            return;
        }
        PlaceAt(point, ctx.FloorY);
    }

    private SceneContext FindContext(Scene target)
    {
        foreach (SceneContext ctx in FindObjectsByType<SceneContext>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (ctx.gameObject.scene == target) return ctx;
        }
        return null;
    }

    private IEnumerator Fade(float to)
    {
        float from = fade.alpha;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fade.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
    }
}
