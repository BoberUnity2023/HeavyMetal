using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;

    public void Press()
    {
        _gameObject.SetActive(!_gameObject.activeSelf);
    }
}
