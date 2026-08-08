using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RealityRestoreResolver : CrisisResolver
{
    //satu baris per prop, biar data balikinnya ga kepisah-pisah

    private class Piece
    {
        public Transform tr;
        public Vector3 home;
        public SpriteRenderer sr;
        public int layerId;
        public int order;
        public Collider2D col;
        public bool wasTrigger;
        public GameObject ghost;
    }

    [Header("Refs")]
    [SerializeField] private GameObject stage; //vignette doang, sengaja g nutup penuh
    [SerializeField] private Camera cam;
    [SerializeField] private string frontLayer = "Minigames";

    [Header("Aturan")]
    [SerializeField] private int maxPieces = 8;
    [SerializeField] private int minPieces = 3;
    [SerializeField] private float scatterRadius = 1.4f;
    [SerializeField] private float snapDist = 0.35f;
    [SerializeField] private Color ghostTint = new(1f, 1f, 1f, 0.22f);

    private Action<CrisisResult> onDone;
    private readonly List<Piece> pieces = new(); //smua yg dicopot
    private readonly List<Piece> loose = new(); //yg blom dibalikin
    private Piece held;
    private Vector3 grabOffset;
    private bool running;

    public override bool IsOnTrack => held != null;
    //cuma layak klo ruangan punya cukup prop bertanda
    public override bool IsAvailable => FindInScene().Count >= minPieces;

    private void Awake()
    {
        stage.SetActive(false);
    }


    public override void Resolve(Action<CrisisResult> done)
    {
        onDone = done;
        running = true;
        held = null;
        pieces.Clear();
        loose.Clear();

        stage.transform.localPosition = new Vector3(0f, 0f, 10f);
        stage.SetActive(true);

        foreach(RealityPiece src in PickPieces())
        {
            Piece p = Detach(src);
            if (p == null) continue;
            pieces.Add(p);
            loose.Add(p);
        }

        if (loose.Count == 0)
        {
            Debug.LogWarning("[Reality] ga ada RealityPiece di scene ini");
            Finish(CrisisResult.Resolved);
        }
    }

    public override void Abort()
    {
        if (running) Finish(CrisisResult.Failed);
    }

    private List<RealityPiece> FindInScene()
    {
        List<RealityPiece> found = new();
        Scene active = SceneManager.GetActiveScene();
        foreach (RealityPiece rp in FindObjectsByType<RealityPiece>(FindObjectsSortMode.None))
        {
            if (rp.gameObject.scene == active) found.Add(rp);
        }
        return found;
    }

    //cuma prop yg di scene yg lagi aktif, jgn sampe ke senggol warga GameRoot
    private List<RealityPiece> PickPieces()
    {
        List<RealityPiece> found = new();
        Scene active = SceneManager.GetActiveScene();
        foreach(RealityPiece rp in FindObjectsByType<RealityPiece>(FindObjectsSortMode.None))
        {
            if (rp.gameObject.scene == active) found.Add(rp);
        }

        //diacak dlu
        for (int i = found.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            RealityPiece tmp = found[i];
            found[i] = found[j];
            found[j] = tmp;
        }
        if (found.Count > maxPieces) found.RemoveRange(maxPieces, found.Count - maxPieces);
        return found;
    }

    private Piece Detach(RealityPiece src)
    {
        SpriteRenderer sr = src.GetComponent<SpriteRenderer>();
        Collider2D col = src.GetComponent<Collider2D>();
        if (sr == null || col == null) {
            Debug.LogWarning($"[Reality] '{src.name}' butuh SpirteRenderes + Collider2D", src);
            return null;
        }

        Piece p = new()
        {
            tr = src.transform,
            home = src.transform.position,
            sr = sr,
            layerId = sr.sortingLayerID,
            order = sr.sortingOrder,
            col = col,
            wasTrigger = col.isTrigger
        };

        p.ghost = MakeGhost(sr, p.home);
        col.isTrigger = true; //bisa diklik tnapa ndorong surya
        sr.sortingLayerName = frontLayer;
        sr.sortingOrder = 20;
        p.tr.position = p.home + RandomScatter();
        return p;
        
    }

    //ghost = sprite e direduipin
    private GameObject MakeGhost(SpriteRenderer src, Vector3 at)
    {
        GameObject go = new($"Ghost_{src.name}");
        go.transform.SetParent(stage.transform, false);
        go.transform.SetPositionAndRotation(at, src.transform.rotation);
        go.transform.localScale = src.transform.lossyScale;

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = src.sprite;
        sr.flipX = src.flipX;
        sr.flipY = src.flipY;
        sr.sharedMaterial = src.sharedMaterial;
        sr.color = ghostTint;
        sr.sortingLayerName = frontLayer;
        sr.sortingOrder = 10; //di bawah propnya but di atas vignette
        return go;
    }

    private Vector3 RandomScatter()
    {
        Vector2 dir = UnityEngine.Random.insideUnitCircle.normalized;
        float dist = UnityEngine.Random.Range(scatterRadius * 0.5f, scatterRadius);
        return new Vector3(dir.x * dist, dir.y * dist * 0.6f, 0f);
    }

    private void Update()
    {
        if (!running || Mouse.current == null) return;

        Vector3 mouse = MouseWorld();
        if (Mouse.current.leftButton.wasPressedThisFrame) TryGrab(mouse);
        else if (Mouse.current.leftButton.wasReleasedThisFrame) Release();

        if (held != null) held.tr.position = mouse + grabOffset;
    }

    private Vector3 MouseWorld()
    {
        Vector3 sp = Mouse.current.position.ReadValue();
        sp.z = -cam.transform.position.z; //jarak cam ke bida z = 0
        return cam.ScreenToWorldPoint(sp);
    }

    private void TryGrab(Vector3 mouse)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(mouse);
        foreach(Collider2D hit in hits)
        {
            Piece p = FindLoose(hit.transform);
            if (p == null) continue;
            held = p;
            grabOffset = p.tr.position - mouse;
            return;
        }


    }

    private Piece FindLoose(Transform tr)
    {
        foreach(Piece p in loose)
        {
            if (p.tr == tr) return p;
        }
        return null;
    }

    private void Release()
    {
        if (held == null) return;
        if (Vector2.Distance(held.tr.position, held.home) <= snapDist)
        {
            held.tr.position = held.home;
            if (held.ghost != null) held.ghost.SetActive(false);
            loose.Remove(held);
            if (loose.Count == 0) Finish(CrisisResult.Resolved);
        }
        held = null;
    }

    private void Finish(CrisisResult res)
    {
        running = false;
        held = null;
        //ini menang or gagal harus jalanan ini
        foreach(Piece p in pieces)
        {
            if (p.tr != null) p.tr.position = p.home;
            if (p.sr != null)
            {
                p.sr.sortingLayerID = p.layerId;
                p.sr.sortingOrder = p.order;
            }
            if (p.col != null) p.col.isTrigger = p.wasTrigger;
            if (p.ghost != null) Destroy(p.ghost);
        }

        pieces.Clear();
        loose.Clear();

        stage.SetActive(false);
        onDone?.Invoke(res);
        onDone = null;
    }


}
