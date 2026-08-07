using System.Collections.Generic;
using UnityEngine;

public class MaskInventory : MonoBehaviour
{
    //main task: db topeng, which is mana yg kebuka, yg aktif, or berapa level afinitasnya
    [SerializeField] private MaskDefinition[] allMasks;
    [SerializeField] private MaskType[] unlockedAtStart = { MaskType.Tenang }; //day 1 just Tenang
    [SerializeField] private MaskType[] equipped = { MaskType.Tenang, MaskType.Ceria, MaskType.Tegas }; //3 slot aktif

    private readonly Dictionary<MaskType, MaskDefinition> defs = new();
    private readonly HashSet<MaskType> unlocked = new();
    private readonly Dictionary<MaskType, int> affinity = new();

    public IReadOnlyList<MaskType> Equipped => equipped;

    private void Awake()
    {
        if (allMasks == null || allMasks.Length == 0) Debug.LogError("[MaskInventory] slot allMasks kosong wok", this);
        foreach(MaskDefinition def in allMasks)
        {
            if (def == null) continue;
            defs[def.Type] = def;
            affinity[def.Type] = 1; //lvl 1 dulu smua
        }

        foreach(MaskType type in unlockedAtStart)
        {
            unlocked.Add(type);
        }
    }

    public MaskDefinition GetDef(MaskType type)
    {
        return defs.TryGetValue(type, out MaskDefinition found) ? found : null;
    }

    public bool IsUnlocked(MaskType type)
    {
        return unlocked.Contains(type);
    }

    public void Unlock(MaskType type)
    {
        unlocked.Add(type);
    }

    public int GetAffinity(MaskType type)
    {
        return affinity.TryGetValue(type, out int lvl) ? lvl : 1;
    }



}

