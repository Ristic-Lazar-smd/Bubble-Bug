using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountNumberTxt;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject lootPrefabGo;
    [SerializeField] private RectTransform lootGridGo;

    public void SetBuyAmount()
    {
        amountNumberTxt.text = slider.value.ToString();
    }

    public void InstatiateLoot()
    {
        for(int i = 0; i < slider.value; i++)
        {
            GameObject loot = Instantiate(lootPrefabGo, lootGridGo);
        }
    }
}
