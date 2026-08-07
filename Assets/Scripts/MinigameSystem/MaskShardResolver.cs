using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaskShardResolver : CrisisResolver
{
    [Header("Refs")]
    [SerializeField] private GameObject stage;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform[] shards; //5 pecahan dgn urutan bebas
    [SerializeField] private SpriteRenderer ghost; //topeng kosongan tujuan

    [Header("Aturan")]
    [SerializeField] private float scatterRadius = 1.6f; //sejauh apa pecahan dilempar
    [SerializeField] private float snapDist = 0.25f;
    [SerializeField] private float ghostAlpha = 0.25f;

    private Action<CrisisResult> onDone;
    private readonly List<Transform> loose = new(); //yg blom kekunci
    private Transform held;
    private Vector3 grabOffset;
    private bool running;

    public override bool IsOnTrack => held != null; //lagi ngerjain maka tensi mereda
    public float Progess => shards.Length == 0 ? 0f : 1f - (float)loose.Count / shards.Length;

    private void Awake()
    {
        stage.SetActive(false);
    }


    public override void Resolve(Action<CrisisResult> done)
    {
        onDone = done;
        running = true;
        held = null;
        loose.Clear();

        stage.transform.localPosition = new Vector3(0f, 0f, 10f); //pksa ke tengah layar
        stage.SetActive(true);

        Color c = ghost.color;
        c.a = ghostAlpha;
        ghost.color = c;
        foreach(Transform s in shards)
        {
            s.localPosition = RandomScatter();
            loose.Add(s);
        }
    }

    public override void Abort()
    {
        if (running) Finish(CrisisResult.Failed);
    }

    private Vector3 RandomScatter()
    {
        Vector2 dir = UnityEngine.Random.insideUnitCircle.normalized;
        float dist = UnityEngine.Random.Range(scatterRadius * 0.6f, scatterRadius);
        return new Vector3(dir.x * dist, dir.y * dist * 0.6f, 0f);
    }

    private void Update()
    {
        if (!running || Mouse.current == null) return;

        Vector3 mouse = MouseWorld();
        if (Mouse.current.leftButton.wasPressedThisFrame) TryGrab(mouse);
        else if (Mouse.current.leftButton.wasReleasedThisFrame) Release();

        if (held != null) held.position = mouse + grabOffset;
    }

    private Vector3 MouseWorld()
    {
        Vector3 sp = Mouse.current.position.ReadValue();
        sp.z = -cam.transform.position.z; //jarak cam ke bidang z = 0
        return cam.ScreenToWorldPoint(sp);
    }

    private void TryGrab(Vector3 mouse)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(mouse);
        foreach(Collider2D hit in hits)
        {
            Transform s = hit.transform;
            if (!loose.Contains(s)) continue; //bukan pecahan or yg udah ke kunci g bisa diambil lagi
            held = s;
            grabOffset = s.position - mouse; //biar pecahan g loncat ke titik kursor
            return;
        }

    }

    private void Release()
    {
        if (held == null) return;
        //rumah tiap pecahan sama persis di local 0,0,0
        if (held.localPosition.magnitude <= snapDist)
        {
            held.localPosition = Vector3.zero;
            loose.Remove(held);
            if (loose.Count == 0) Finish(CrisisResult.Resolved);
        }
        held = null;
    }

    private void Finish(CrisisResult res)
    {
        running = false;
        held = null;
        stage.SetActive(false);
        onDone?.Invoke(res);
        onDone = null;
    }
}
