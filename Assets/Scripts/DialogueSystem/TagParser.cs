using System.Collections.Generic;
using UnityEngine;

public static class TagParser
{
    //main task: mesin penggiling sebuah baris dialog
    public static DialogueLine Parse(string text, List<string> rawTags)
    {
        string speaker = null;
        int mh = 0, tensi = 0, energy = 0;
        if (rawTags != null)
        {
            foreach (string tag in rawTags)
            {
                int sep = tag.IndexOf(':');
                if (sep < 0) continue; //which is no tag atau -1

                string key = tag[..sep].Trim().ToLower();
                string val = tag[(sep + 1)..].Trim();
                switch (key)
                {
                    case "speaker":
                        speaker = val;
                        break;
                    case "mh":
                        mh = ParseInt(val, key);
                        break;
                    case "energy":
                        energy = ParseInt(val, key);
                        break;
                    case "tensi":
                        tensi = ParseInt(val, key);
                        break;
                    //tag2 lain (sfx, anim, agenda, dll) blom dihandle
                }
            }
        }
        return new DialogueLine(text, speaker, mh, tensi, energy);
    } 

    private static int ParseInt (string val, string key)
    {
        if (int.TryParse(val, out int res))
        {
            return res;
        } else
        {
            Debug.LogWarning($"[Tag Parser] value tag of '{key}' bukan angka wok: '{val}'");
            return 0;
        }
    }
}
