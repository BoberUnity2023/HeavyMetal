using UnityEngine;

[CreateAssetMenu(fileName = "Shop", menuName = "Configs/ConfigShop")]

public class ConfigShop : ScriptableObject
{
    [SerializeField] private int _priceOpenLevel1;
    [SerializeField] private int _priceOpenLevel10;
    [SerializeField] private int _priceOpenLevel100;
    [SerializeField] private int _priceHat;

    public int PriceOpenLevel1 => _priceOpenLevel1;
    public int PriceOpenLevel10 => _priceOpenLevel1;
    public int PriceOpenLevel100 => _priceOpenLevel1;
    public int PriceHat => _priceOpenLevel1;

}
