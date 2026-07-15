using System.Collections.Generic;
using UnityEngine;

public class CrisisManager : MonoBehaviour
{
    //main task: handle/call mekanik/minigame saat krisis utama terjadi dan handle behaviour saat krisis selesai

    [SerializeField] private PlayerInputReader input;
    [SerializeField] private BatinState batin;

    [Header("Mekanik Krisis")]
    [SerializeField] private List<CrisisResolver> resolvers = new();

    public bool IsCrisisActive { get; private set; }

    private void OnEnable()
    {
        batin.OnTensiMax += HandleTensiMaxed;
    }

    private void OnDisable()
    {
        batin.OnTensiMax -= HandleTensiMaxed;
    }

    private void HandleTensiMaxed()
    {
        if (IsCrisisActive) return;

        if (resolvers.Count == 0)
        {
            Debug.LogWarning("[Crisis] Tension mentok tapi blom ada resolver, so skip it");
            batin.ResetTensiExploded();
            return;
        }

        IsCrisisActive = true;
        input.DisableMove();

        //random minigame as resolver (tentatif)
        int randIdx = Random.Range(0, resolvers.Count);
        CrisisResolver chosenResolver = resolvers[randIdx];
        chosenResolver.Resolve(EndCrisis);
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
