using System;
using TMPro;
using UnityEngine;

public enum BreathPhase
{
    Inhale, 
    Exhale
}

public class BreathingResolver : CrisisResolver
{
    //main taks: jadi minigame ngatur napas
    [Header("Refs")]
    [SerializeField] private PlayerInputReader input;
    [SerializeField] private GameObject stage; //induk smua visual minigame
    [SerializeField] private Transform cursor;
    [SerializeField] private Transform fluff; //dandelion/kapas randu
    [SerializeField] private Animator heartBeat;
    [SerializeField] private float heartFast = 1.6f; //panik
    [SerializeField] private float heartCalm = 0.7f; //chill

    [HeaderAttribute("Ritme")]
    [SerializeField] private float inhaleDur = 4f;
    [SerializeField] private float exhaleDur = 6f;
    [SerializeField] private float graceDur = 0.6f; //jeda ganti fase blom dihukum

    [Header("Aturan")]
    [SerializeField] private Vector2 playArea = new(2f, 1.1f); //setengah lebar & tinggi area main
    [SerializeField] private float cursorSpeed = 1.8f;
    [SerializeField] private float hitRadius = 0.3f;
    [SerializeField] private float fillPerSec = 0.035f;
    [SerializeField] private float drainPerSec = 0.03f; //sengaja lebih pelan dr fill biar ketika salah jdi ga langsung nge hukum

    [Header("Kapas' Moves")]
    [SerializeField] private float driftRadius = 0.6f;
    [SerializeField] private float driftSpeed = 0.4f;

    [Header("Petunjuk")]
    [SerializeField] private TextMeshPro phaseLabel;
    [SerializeField] private Transform meter; //bar napas/mekar ikut progres
    [SerializeField] private float meterWidth = 1.2f;
    [SerializeField] private string inhaleText = "tarik napas... (jauhi kapas)";
    [SerializeField] private string exhaleText = "hembuskan...(arahkan ke kapas)";

    private Action<CrisisResult> onDone;
    private Vector3 fluffOrigin;
    private float phaseTime;
    private float progress;
    private bool running;

    public float Progress => progress; //dibaca ui meter napas
    public BreathPhase Phase {  get; private set; }
    public bool IsCorrect {  get; private set; } //dibaca vfx for feedbaack
    public override bool IsOnTrack => IsCorrect;
    
    
    private void Awake()
    {
        stage.SetActive(false);
    }

    private void SetPhase(BreathPhase next)
    {
        Phase = next;
        phaseTime = 0f;
        phaseLabel.text = (next == BreathPhase.Inhale) ? inhaleText : exhaleText;
    }
    public override void Resolve(Action<CrisisResult> done)
    {
        onDone = done;
        progress = 0f;
        SetPhase(BreathPhase.Inhale);
        fluffOrigin = fluff.localPosition;
        cursor.localPosition= fluffOrigin;
        running = true;

        stage.SetActive(true);
        input.EnableNavigate(); //wasd lwat ui/navigate cz player.move dimatiin slama krisis
    }

    public override void Abort()
    {
        if (running) Finish(CrisisResult.Failed);
    }

    private void Update()
    {
        if (!running) return;

        MoveCursor();
        DriftFluff();
        TickPhase();
        Score();
    }


    private void MoveCursor()
    {
        Vector3 dir = input.NavigateInput;
        Vector3 next = cursor.localPosition + cursorSpeed * Time.deltaTime * dir;

        //constraint biar kotak ga kluar layar
        next.x = Mathf.Clamp(next.x, -playArea.x, playArea.x);
        next.y = Mathf.Clamp(next.y, -playArea.y, playArea.y);
        cursor.localPosition = next;
    }

    //kapas melayang by code cz posisinya dibaca buat ngitung jarak
    private void DriftFluff()
    {
        float t = Time.time * driftSpeed;
        float x = Mathf.PerlinNoise(t, 37.3f) - 0.5f;
        float y = Mathf.PerlinNoise(91.7f, t) - 0.5f;
        fluff.localPosition = fluffOrigin + 4f * driftRadius * new Vector3(x, y, 0f);
    }

    private void TickPhase()
    {
        phaseTime += Time.deltaTime;
        float dur = (Phase == BreathPhase.Inhale) ? inhaleDur : exhaleDur;
        if (phaseTime < dur) return;
       SetPhase(Phase == BreathPhase.Inhale ? BreathPhase.Exhale : BreathPhase.Inhale);
    }

    private void Score()
    {
        bool near = Vector2.Distance(cursor.localPosition, fluff.localPosition) <= hitRadius;
        IsCorrect = (Phase == BreathPhase.Exhale) ? near : !near;

        if (IsCorrect)
        {
            progress += fillPerSec * Time.deltaTime;
            if (progress >= 1f) Finish(CrisisResult.Resolved);
        } else if (phaseTime > graceDur)
        {
            progress = Mathf.Max(0f, progress - drainPerSec * Time.deltaTime);
        }

        //bar mekar dr tengah ke kedua sisi
        meter.localScale = new Vector3(progress * meterWidth, 0.06f, 1f);
        //detak melambat seiring napas kekumpul, dan sebaliknya
        heartBeat.speed = Mathf.Lerp(heartFast, heartCalm, progress);
    }

    private void Finish(CrisisResult res)
    {
        running = false;
        stage.SetActive(false);
        input.DisableNavigate();
        onDone?.Invoke(res);
        onDone = null;
    }


}
