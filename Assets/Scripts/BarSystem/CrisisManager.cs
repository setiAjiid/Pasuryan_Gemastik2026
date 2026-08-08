using System.Collections.Generic;
using UnityEngine;

public class CrisisManager : MonoBehaviour
{
    //main task: handle/call mekanik/minigame saat krisis utama terjadi dan handle behaviour saat krisis selesai

    [SerializeField] private PlayerInputReader input;
    [SerializeField] private BatinState batin;
    [SerializeField] private float tensiDropPerSec = 2.8f; //tensi turun slama bener

    [Header("Mekanik Krisis")]
    [SerializeField] private List<CrisisResolver> resolvers = new();

    [Header("Tekanan Krisi")]
    [SerializeField] private float mhLeakPerSec = 0.5f;
    [SerializeField] private int mhFloor = 10; //dibawah ini krisisiny dinyatakan gagal

    private CrisisResolver active;
    private float leakCarry; //nampung pecahan cz CalcStat cuman terima int
    private float dropCarry;

    private readonly List<CrisisResolver> ready = new(); // yg lagi layak dipake

    public bool IsCrisisActive { get; private set; }

    private void OnEnable()
    {
        batin.OnTensiMax += HandleTensiMaxed;
    }

    private void OnDisable()
    {
        batin.OnTensiMax -= HandleTensiMaxed;
    }

    private void LateUpdate()
    {
        if (!IsCrisisActive) return;

        //mh bocor perlahan slama krisis
        leakCarry += mhLeakPerSec * Time.deltaTime;
        if (leakCarry >= 1f)
        {
            int drop = Mathf.FloorToInt(leakCarry);
            leakCarry -= drop;
            batin.CalcStat(-drop, 0, 0);
        }

        //tensi mereda
        if (active.IsOnTrack)
        {
            dropCarry += tensiDropPerSec * Time.deltaTime;
            if (dropCarry >= 1f)
            {
                int down = Mathf.FloorToInt(dropCarry);
                dropCarry -= down;
                batin.CalcStat(0, 0, -down);
            }
        }

        if (batin.MHVal <= mhFloor) active.Abort(); //kehabisan napas
    }


    private void HandleTensiMaxed()
    {
        if (IsCrisisActive) return;

        ready.Clear();
        foreach(CrisisResolver r in resolvers)
        {
            if (r != null && r.IsAvailable) ready.Add(r);
        }

        if (ready.Count == 0)
        {
            Debug.LogWarning("[Crisis] Tension mentok tapi blom ada resolver, so skip it");
            batin.ResetTensiExploded();
            return;
        }

        Begin(ready[Random.Range(0, ready.Count)]);
    }
    
    public void ForceCrisis(CrisisResolver res)
    {
        if (IsCrisisActive || res == null) return;
        Begin(res);
    }

    private void Begin(CrisisResolver res)
    {
        IsCrisisActive = true;
        input.DisableMove();

        leakCarry = 0f;
        dropCarry = 0f;
        active = res;
        active.Resolve(EndCrisis);
    }

    private void EndCrisis(CrisisResult res)
    {
        switch (res)
        {
            case CrisisResult.Resolved:
                batin.CalcStat(+10, +10, -20);
                break;
            case CrisisResult.Failed:
                batin.CalcStat(-10, 0, +10);
                break;
        }

        IsCrisisActive = false;
        batin.ResetTensiExploded();
        input.EnableMove();
    }


}
