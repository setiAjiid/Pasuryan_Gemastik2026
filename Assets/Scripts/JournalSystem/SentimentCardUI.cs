using TMPro;
using UnityEngine;

public class SentimentCardUI : MonoBehaviour
{
    //main task: tampilan satu kartu sentimen, label + kalimat konteks + sorotan
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private TextMeshProUGUI context;
    [SerializeField] private GameObject highlight;

    public void Bind(string labelText, string contextText)
    {
        label.text = labelText;
        context.text = contextText;
        gameObject.SetActive(true);
    }

    public  void SetPicked(bool on)
    {
        highlight.SetActive(on);
    }
}
