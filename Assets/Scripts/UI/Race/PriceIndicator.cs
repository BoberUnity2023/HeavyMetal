using TMPro;
using UnityEngine;

public class PriceIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text _indicator;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void ShHideow()
    {
        gameObject.SetActive(false);
    }

    public void SetPrice(int price)
    {
        _indicator.text = price.ToString();
    }
}
