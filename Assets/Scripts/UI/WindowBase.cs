using UnityEngine;

public class WindowBase : MonoBehaviour
{
    [SerializeField] protected Garage _garage;
    [SerializeField] protected GameObject _window;
    public virtual void Show()
    {
        _window.SetActive(true);
    }

    public virtual void Hide()
    {
        _window.SetActive(false);
    }
}
