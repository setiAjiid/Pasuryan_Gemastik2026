using System;
using UnityEngine;

public enum CrisisResult
{
    Resolved,
    Failed
}

public abstract class CrisisResolver : MonoBehaviour
{
    //main task: handle mekanik game kalo misal tensinya max
    //blablabla masih belum pasti minigame/mekaniknya apa atau berapa

    //kalo dah selesai perlu panggil fungsi endCrisis pk delegate
    public abstract void Resolve(Action<CrisisResult> OnDone);
}
