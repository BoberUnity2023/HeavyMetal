using UnityEngine;

public class UIStar : MonoBehaviour
{
    [SerializeField] private GameObject _on;
    [SerializeField] private GameObject _off;
    public void On()
    {
        _on.SetActive(true);
        _off.SetActive(false);
    }

    public void Off()
    {
        _on.SetActive(false);
        _off.SetActive(true);
    }
}
