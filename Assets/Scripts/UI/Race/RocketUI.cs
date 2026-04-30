using UnityEngine;

public class RocketUI : MonoBehaviour
{
    [SerializeField] private GameObject _back;
    [SerializeField] private GameObject _icon;

    public void SetFull()
    {
        _icon.SetActive(true);
        _back.SetActive(false);
    }

    public void SetEmpty()
    {
        _icon.SetActive(false);
        _back.SetActive(true);
    }

    public void Hide()
    {
        _icon.SetActive(false);
        _back.SetActive(false);
    }
}
