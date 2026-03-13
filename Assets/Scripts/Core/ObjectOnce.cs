using UnityEngine;

public class ObjectOnce : MonoBehaviour
{
    [SerializeField] private string _key;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("ObjectOnce" + _key))
            Destroy(gameObject);
        else
            PlayerPrefs.SetInt("ObjectOnce" + _key, 1);
    }
}
