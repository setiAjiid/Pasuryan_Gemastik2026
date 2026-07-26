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

    public void PlaceAt(SpawnPoint point)
    {
        Vector3 dest = point.transform.position;
        Vector3 delta = dest - playerBody.transform.position; //hitung jarak perpindahan
        playerBody.position = dest;
        playerBody.linearVelocity = Vector2.zero; //nolin sisa momentum
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
        foreach (SpawnPoint point in FindObjectsByType<SpawnPoint>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (point.gameObject.scene != target) continue;
            if (point.SpawnId != spawnId) continue;

            PlaceAt(point);
            return;
        }

        Debug.LogError($"[Transition] spawn '{spawnId}' ga ketemeu di scene {target.name}");
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
