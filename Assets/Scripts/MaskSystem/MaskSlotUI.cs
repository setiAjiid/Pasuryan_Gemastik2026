using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaskSlotUI : MonoBehaviour
{
    //main task: tampilan satu kotak topeng, which is gambar, harga energy, beku/nggak, kepilih/ga
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI costLabel;
    [SerializeField] private GameObject highlight;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color frozenColor = new(0.35f, 0.35f, 0.35f, 1f);

    public void Bind(MaskDefinition def, bool selectable)
    {
        icon.sprite = def.Icon;
        icon.color = selectable ? activeColor : frozenColor; //yg kekunci ttp keliatan, tapi beku
        costLabel.text = def.EnergyCost.ToString();
        costLabel.gameObject.SetActive(selectable); //harga cmn relevan klo emang bisa dibeli
        gameObject.SetActive(true);

    }

    public void SetPicked(bool on)
    {
        highlight.SetActive(on);
    }


}
